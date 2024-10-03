using DivvyUp_Shared.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp_Shared.Dto
{
    public class PersonProductDto
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int personId { get; set; }
        public double partOfPrice { get; set; }
        public int quantity { get; set; }
        public int maxQuantity { get; set; }
        public bool isCompensation { get; set; }
        public bool settled { get; set; }

        public PersonProductDto()
        {
            id = 0;
            productId = 0;
            personId = 0;
            quantity = 0;
            maxQuantity = 1;
            partOfPrice = 0;
            isCompensation = false;
            settled = false;
        }

        public PersonProductDto(int id, int productId, int personId, double partOfPrice, int quantity, int maxQuantity, bool isCompensation, bool settled)
        {
            this.id = id;
            this.productId = productId;
            this.personId = personId;
            this.partOfPrice = partOfPrice;
            this.quantity = quantity;
            this.maxQuantity = maxQuantity;
            this.isCompensation = isCompensation;
            this.settled = settled;
        }
    }
}
