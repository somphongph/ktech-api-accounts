using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ktech.accounts.Models
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [Required]
        [MaxLength(300)]
        public string name { get; set; }

        [Required]
        [MaxLength(200)]
        public string email { get; set; }

        [MaxLength(100)]
        public string username { get; set; }

        public ProfileImage profileImage { get; set; }

        [BsonDateTimeOptions]
        public DateTime lastLogon { get; set; } = DateTime.Now;

        [BsonDateTimeOptions]
        public DateTime createdAt { get; set; } = DateTime.Now;
        
        // [Required]
        // public byte[] passwordHash { get; set; }

        // [Required]
        // public byte[] passwordSalt { get; set; }
    }
}