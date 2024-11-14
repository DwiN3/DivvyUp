namespace DivvyUp_Shared.Dtos.Entity
{
    public class PersonProductDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PersonId { get; set; }
        public decimal PartOfPrice { get; set; }
        public int Quantity { get; set; }
        public bool Compensation { get; set; }
        public bool Settled { get; set; }

        public PersonDto Person { get; set; }

        public PersonProductDto()
        {
            Id = 0;
            ProductId = 0;
            PersonId = 0;
            Quantity = 0;
            PartOfPrice = 0;
            Compensation = false;
            Settled = false;
        }

        public PersonProductDto(int id, int productId, int personId, decimal partOfPrice, int quantity, bool compensation, bool settled)
        {
            Id = id;
            ProductId = productId;
            PersonId = personId;
            PartOfPrice = partOfPrice;
            Quantity = quantity;
            Compensation = compensation;
            Settled = settled;
        }
    }
}
