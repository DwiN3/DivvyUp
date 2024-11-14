using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DivvyUp_Shared.Models
{
    public class PersonProduct
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Person")]
        [Column("person_id")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        [ForeignKey("Product")]
        [Column("product_id")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("part_of_price")]
        public decimal PartOfPrice { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("is_compensation")]
        public bool Compensation { get; set; }

        [Column("is_settled")]
        public bool Settled { get; set; }
    }
}
