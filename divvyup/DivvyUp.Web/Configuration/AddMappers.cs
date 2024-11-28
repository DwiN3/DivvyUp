using DivvyUp.Web.Data;
using DivvyUp.Web.Mappers;
using DivvyUp.Web.Middlewares;
using DivvyUp.Web.Services;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Interfaces;

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