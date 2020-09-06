using System.ComponentModel.DataAnnotations;

namespace ktech.accounts.Models
{
    public class Login
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}