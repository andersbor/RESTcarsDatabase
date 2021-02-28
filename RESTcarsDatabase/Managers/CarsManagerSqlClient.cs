using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RESTcarsDatabase.Models;

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
            string make = reader.GetString(1);
            string model = reader.GetString(2);
            //string publisher = reader.IsDBNull(3) ? null : reader.GetString(3);
            //decimal price = reader.GetDecimal(4);
            Car car = new Car
            {
                Id = id,
                Make = make,
                Model = model,
            };
            return car;
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
            string insertString = "insert into cars (make, model) values (@make, @model);";
            using (SqlConnection conn = new SqlConnection(Secrets.ConnectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(insertString, conn))
                {
                    command.Parameters.AddWithValue("@make", car.Make);
                    command.Parameters.AddWithValue("@model", car.Model);
                    int rowsAffected = command.ExecuteNonQuery();
                    //return rowsAffected; 
                    int id = GetLatestId(conn, "cars");
                    return GetById(id);
                }
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
                "update cars set make=@make, model=@model where id=@id;";
            using (SqlConnection databaseConnection = new SqlConnection(Secrets.ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateString, databaseConnection))
                {
                    updateCommand.Parameters.AddWithValue("@id", id);
                    updateCommand.Parameters.AddWithValue("@make", updates.Make);
                    updateCommand.Parameters.AddWithValue("@model", updates.Model);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected == 0) return null;
                    updates.Id = id;
                    return updates;
                }
            }
        }
    }
}