using RESTcarsDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RESTcarsDatabase.Managers
{
    public class CarsManagerSqlClient : ICarsManager
    {
        public IEnumerable<Car> GetAll()
        {
            string selectString = "select * from cars";
            using (SqlConnection conn = new SqlConnection(Secrets.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectString, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Car> result = new List<Car>();
                        while (reader.Read())
                        {
                            Car car = ReadCar(reader);
                            result.Add(car);
                        }
                        return result;
                    }
                }
            }
        }

        private Car ReadCar(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string make = GuardedGet<string>(reader, 1);
            string model = GuardedGet<string>(reader, 2);
            int? price = GuardedGet<int?>(reader, 3);
            /*int? price;
            if (reader.IsDBNull(3)) { price = null; }
            else { price = reader.GetInt32(3); }*/
            Car car = new Car
            {
                Id = id,
                Make = make,
                Model = model,
                Price = price
            };
            return car;
        }

        private static T GuardedGet<T>(SqlDataReader reader, int column)
        {
            if (reader.IsDBNull(column)) return default(T);
            return reader.GetFieldValue<T>(column);
        }


        public Car GetById(int id)
        {
            string selectString = "select * from cars where id = @id";
            using (SqlConnection conn = new SqlConnection(Secrets.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return ReadCar(reader);
                        }
                        return null;
                    }
                }
            }
        }

        public Car Add(Car car)
        {
            try
            {
                string insertString = "insert into cars (make, model, price) values (@make, @model, @price);";
                using (SqlConnection conn = new SqlConnection(Secrets.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(insertString, conn))
                    {
                        GuardedAssign(command, "@make", car.Make);
                        GuardedAssign(command, "@model", car.Model);
                        GuardedAssign(command, "@price", car.Price);
                        //command.Parameters.AddWithValue("@price", car.Price);
                        int rowsAffected = command.ExecuteNonQuery();
                        //return rowsAffected; 
                        int id = GetLatestId(conn, "cars");
                        return GetById(id);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Exception translation
                throw new CarException(ex.Message);
            }
        }

        private int GetLatestId(SqlConnection connection, string tableName)
        {
            // https://www.munisso.com/2012/06/07/4-ways-to-get-identity-ids-of-inserted-rows-in-sql-server/
            string selectString = "select IDENT_CURRENT(@tableName)";
            using (SqlCommand command = new SqlCommand(selectString, connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand
                decimal id = (decimal)command.ExecuteScalar();
                return Decimal.ToInt32(id);
            }
        }

        public Car Delete(int id)
        {
            Car car = GetById(id); // TODO use same connection for get + delete
            // https://stackoverflow.com/questions/13677318/how-to-run-multiple-sql-commands-in-a-single-sql-connection
            if (car == null) return null;
            string deleteString = "delete from cars where id = @id";
            using (SqlConnection conn = new SqlConnection(Secrets.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(deleteString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    //return rowsAffected;
                    return car;
                }
            }
        }

        public Car Update(int id, Car updates)
        {
            const string updateString =
                "update cars set make=@make, model=@model, price=@price where id=@id;";
            using (SqlConnection databaseConnection = new SqlConnection(Secrets.ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateString, databaseConnection))
                {
                    updateCommand.Parameters.AddWithValue("@id", id);
                    GuardedAssign(updateCommand, "@make", updates.Make);
                    GuardedAssign(updateCommand, "@model", updates.Model);
                    GuardedAssign(updateCommand, "@price", updates.Price);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected == 0) return null;
                    updates.Id = id;
                    return updates;
                }
            }
        }

        // DBNull is not the same as C# null ...
        private static void GuardedAssign<T>(SqlCommand command, string parameterName, T value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }
    }
}