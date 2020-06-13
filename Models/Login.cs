using System.ComponentModel.DataAnnotations;

namespace tripgator.accounts.Models
{
    public class Login
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}