namespace DivvyUp_Shared.RequestDto
{
    public class AddEditPersonProductRequest
    {
        public int PersonId { get; set; }
        public int Quantity { get; set; }

        public AddEditPersonProductRequest()
        {
            PersonId = 0;
            Quantity = 0;
        }

        public AddEditPersonProductRequest(int personId, int quantity)
        {
            PersonId = personId;
            Quantity = quantity;
        }
    }
}