using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tripgator.accounts.Models
{
    public class Profile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
    }
}