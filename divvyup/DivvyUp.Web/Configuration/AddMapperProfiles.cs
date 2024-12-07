using DivvyUp.Web.Mappers;

namespace DivvyUp.Web.Configuration
{
    public static class AddMapperProfiles
    {
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserToUserDtoProfile));
            services.AddAutoMapper(typeof(PersonToPersonDtoProfile));
            services.AddAutoMapper(typeof(LoanToLoanDtoProfile));
            services.AddAutoMapper(typeof(ReceiptToReceiptDtoProfile));
            services.AddAutoMapper(typeof(ProductToProductDtoProfile));
            services.AddAutoMapper(typeof(PersonProductToPersonProductDtoProfile));
        }
    }
}