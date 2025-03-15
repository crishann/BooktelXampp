using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewBooktel.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //[NotMapped] // Prevents it from being stored in the DB
        //public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }

}
