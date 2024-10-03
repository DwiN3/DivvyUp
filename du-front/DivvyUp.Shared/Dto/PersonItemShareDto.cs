using DivvyUp_Shared.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp_Shared.Dto
{
    public class PersonItemShareDto
    {
        public int id { get; set; }
        public int itemId { get; set; }
        public int personId { get; set; }
        public double partOfPrice { get; set; }
        public int quantity { get; set; }
        public int maxQuantity { get; set; }
        public bool compensation { get; set; }
        public bool settled { get; set; }

        public PersonItemShareDto()
        {
            id = 0;
            itemId = 0;
            personId = 0;
            quantity = 0;
            maxQuantity = 1;
            partOfPrice = 0;
            compensation = false;
            settled = false;
        }

        public PersonItemShareDto(int id, int itemId, int personId, double partOfPrice, int quantity, int maxQuantity, bool compensation, bool settled)
        {
            this.id = id;
            this.itemId = itemId;
            this.personId = personId;
            this.partOfPrice = partOfPrice;
            this.quantity = quantity;
            this.maxQuantity = maxQuantity;
            this.compensation = compensation;
            this.settled = settled;
        }
    }
}
