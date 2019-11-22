using System.ComponentModel.DataAnnotations;

namespace tripdini.accounts.Models
{
    public class Authenticate
    {
        [Required]
        public string email { get; set; }

        public string name { get; set; }
    }
}