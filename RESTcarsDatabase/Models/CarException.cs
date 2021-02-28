using System;

namespace RESTcarsDatabase.Models
{
    public class CarException : Exception
    {
        public CarException(string message) : base(message)
        {
        }
    }
}
