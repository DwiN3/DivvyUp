using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DivvyUp_Shared.Model
{
    public class Person
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

        [Column("surname")]
        public string Surname { get; set; }

        [Column("receipts_count")]
        public int ReceiptsCount { get; set; }

        [Column("products_count")]
        public int ProductsCount { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("unpaid_amount")]
        public decimal UnpaidAmount { get; set; }

        [Column("loan_balance")]
        public decimal LoanBalance { get; set; }

        [Column("user_account")]
        public bool UserAccount { get; set; }
    }
}
