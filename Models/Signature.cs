using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace apiaccounts.Models
{
    public class Signature
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public ProfileImage profileImage { get; set; }
    }
}