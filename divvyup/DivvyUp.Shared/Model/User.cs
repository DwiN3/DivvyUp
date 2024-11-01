using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DivvyUp_Shared.Model
{
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }
    }
}
