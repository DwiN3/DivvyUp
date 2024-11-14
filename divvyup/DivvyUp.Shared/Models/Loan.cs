using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DivvyUp_Shared.Models
{
    public class Loan
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Person")]
        [Column("person_id")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }


        [Column("is_lent")]
        public bool Lent { get; set; }

        [Column("is_settled")]
        public bool Settled { get; set; }
    }
}
