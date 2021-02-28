namespace RESTcarsDatabase.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Price { get; set; }
        public override string ToString()
        {
            return Id + " " + Make + " " + Model + " " + Price;
        }
    }
}
