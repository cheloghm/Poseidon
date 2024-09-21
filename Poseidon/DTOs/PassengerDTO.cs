using Swashbuckle.AspNetCore.Annotations;

namespace Poseidon.DTOs
{
    public class PassengerDTO
    {
        [SwaggerSchema(ReadOnly = true, Description = "Leave this field blank; MongoDB will automatically generate the ID.")]
        public string Id { get; set; }
        public bool Survived { get; set; }
        public int? Pclass { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public double? Age { get; set; }
        public int SiblingsOrSpousesAboard { get; set; }
        public int ParentsOrChildrenAboard { get; set; }
        public double? Fare { get; set; }
    }
}
