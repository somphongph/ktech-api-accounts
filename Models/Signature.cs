using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ktech.accounts.Models
{
    public class Signature
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public ProfileImage profileImage { get; set; }
    }
}