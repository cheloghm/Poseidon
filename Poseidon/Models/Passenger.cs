using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
{
    public class Passenger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Survived")]
        public int Survived { get; set; }

        [BsonElement("Pclass")]
        public int Pclass { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Sex")]
        public string Sex { get; set; }

        [BsonElement("Age")]
        public double? Age { get; set; }

        [BsonElement("Siblings/Spouses Aboard")]
        public int SiblingsOrSpousesAboard { get; set; }

        [BsonElement("Parents/Children Aboard")]
        public int ParentsOrChildrenAboard { get; set; }

        [BsonElement("Fare")]
        public double Fare { get; set; }
    }
}
