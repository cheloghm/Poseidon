namespace Poseidon.DTOs
{
    public class PassengerDTO
    {
        public string Id { get; set; }
        public bool Survived { get; set; }
        public int? Pclass { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int? Age { get; set; }
        public int SiblingsOrSpousesAboard { get; set; }
        public int ParentsOrChildrenAboard { get; set; }
        public double? Fare { get; set; }
    }
}
