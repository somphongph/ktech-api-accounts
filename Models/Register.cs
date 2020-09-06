using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ktech.accounts.Models
{
    public class Register
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [MaxLength(300)]
        public string name { get; set; }

        [Required]
        [MaxLength(200)]
        public string email { get; set; }

        // [Required]
        // [MaxLength(50)]
        // public string password { get; set; }

        // [Required]
        // [MaxLength(50)]
        // [Compare("password")]
        // public string confirmPassword { get; set; }         
    }
}