namespace Poseidon.Models
{
    public class PassengerSearchCriteria
    {
        public string Name { get; set; }
        public int? Pclass { get; set; }
        public string Sex { get; set; }
        public double? MinAge { get; set; }
        public double? MaxAge { get; set; }
        public double? MinFare { get; set; }
        public double? MaxFare { get; set; }
    }
}
