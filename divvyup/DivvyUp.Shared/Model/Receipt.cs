using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DivvyUp_Shared.Model
{
    public class Receipt
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("is_settled")]
        public bool Settled { get; set; }
    }
}
