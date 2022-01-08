namespace RESTcarsDatabase.Models
{
    public class UsedCar
    {
        // id, make, model, fuelType, year, km, description, price, time, sellersId, pictureUrl

        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string FuelType { get; set; }
        public int Km { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public string Seller { get; set; }
        public string PictureUrl { get; set; }

        public override string ToString()
        {
            return $"{Id} {Make} {Model} {Year} {Km} {Price}";
        }
    }
}
