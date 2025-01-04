using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Tests
{
    public static class DataFactory
    {
        public static User CreateUser(string username, string name, string email, string password)
        {
            return new User
            {
                Username = username,
                Name = name,
                Surname = string.Empty,
                Email = email,
                Password = password
            };
        }

        public static Person CreatePerson(string name, int userId, bool isUserAccount = true)
        {
            return new Person
            {
                UserId = userId,
                Name = name,
                Surname = string.Empty,
                ReceiptsCount = 0,
                ProductsCount = 0,
                TotalAmount = 0,
                UnpaidAmount = 0,
                CompensationAmount = 0,
                LoanBalance = 0,
                UserAccount = isUserAccount
            };
        }

        public static Product CreateProduct(int receiptId, string name, decimal price, int maxQuantity = 1)
        {
            return new Product
            {
                ReceiptId = receiptId,
                Name = name,
                Price = price,
                MaxQuantity = maxQuantity,
                AvailableQuantity = maxQuantity,
                AdditionalPrice = 0,
                DiscountPercentage = 0,
                PurchasedQuantity = 0,
                Divisible = true,
                TotalPrice = price,
                Settled = false
            };
        }

        public static Receipt CreateReceipt(int userId, string name, decimal totalPrice)
        {
            return new Receipt
            {
                UserId = userId,
                Name = name,
                Date = DateOnly.FromDateTime(DateTime.Now),
                TotalPrice = totalPrice,
                DiscountPercentage = 0,
                Settled = false
            };
        }

        public static PersonProduct CreatePersonProduct(int personId, int productId, int quantity)
        {
            return new PersonProduct()
            {
                PersonId = personId,
                ProductId = productId,
                PartOfPrice = 0.0m,
                Quantity = quantity,
                Compensation = false,
                Settled = false
            };
        }
    }
}
