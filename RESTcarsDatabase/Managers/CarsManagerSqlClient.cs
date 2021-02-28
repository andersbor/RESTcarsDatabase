using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public Car Update(int id, Car updates)
        {
            throw new NotImplementedException();
        }

        public Car Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
