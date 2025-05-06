using System.ComponentModel.DataAnnotations;

namespace NewBooktel.Models
{
    public class ContactUs
    {
        public int Contid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
