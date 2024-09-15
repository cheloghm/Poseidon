using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
{
    public class Token
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string UserId { get; set; }  // Link token to user
        public string JwtToken { get; set; }  // The actual token
        public DateTime Expiration { get; set; }  // Token expiration date
    }
}
