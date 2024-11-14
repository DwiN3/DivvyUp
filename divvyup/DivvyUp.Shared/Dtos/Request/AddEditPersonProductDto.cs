namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditPersonProductDto
    {
        public int PersonId { get; set; }
        public int Quantity { get; set; }

        public AddEditPersonProductDto()
        {
            PersonId = 0;
            Quantity = 0;
        }

        public AddEditPersonProductDto(int personId, int quantity)
        {
            PersonId = personId;
            Quantity = quantity;
        }
    }
}