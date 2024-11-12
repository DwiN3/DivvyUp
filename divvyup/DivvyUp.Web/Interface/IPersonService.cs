using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;
using DivvyUp_Shared.Dto;

namespace DivvyUp.Web.Interface
{
    public interface IPersonService
    {
        Task Add(AddEditPersonRequest person);
        Task Edit(AddEditPersonRequest person, int personId);
        Task Remove(int personId);
        Task<PersonDto> GetPerson(int personId);
        Task<List<PersonDto>> GetPersons();
        Task<PersonDto> GetUserPerson();
        Task<List<PersonDto>> GetPersonFromReceipt(int receiptId);
        Task<List<PersonDto>> GetPersonFromProduct(int productId);
    }
}
