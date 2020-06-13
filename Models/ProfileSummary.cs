using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tripgator.accounts.Models
{
    public class ProfileSummary
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public ProfileImage profileImage { get; set; }
    }
}