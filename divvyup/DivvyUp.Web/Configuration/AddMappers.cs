using DivvyUp.Web.Mappers;

namespace DivvyUp.Web.Configuration
{
    public static class AddMappers
    {
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserToUserDtoMapping));
            services.AddAutoMapper(typeof(PersonToPersonDtoMapping));
            services.AddAutoMapper(typeof(LoanToLoanDtoMapping));
            services.AddAutoMapper(typeof(ReceiptToReceiptDtoMapping));
            services.AddAutoMapper(typeof(ProductToProductDtoMapping));
            services.AddAutoMapper(typeof(PersonProductToPersonProductDtoMapping));
        }
    }
}