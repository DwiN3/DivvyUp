using DivvyUp_Shared.RequestDto;
using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface ILoanService
    {
        Task Add(AddEditLoanRequest request);
        Task Edit(AddEditLoanRequest request, int loanId);
        Task Remove(int loanId);
        Task SetPerson(int loanId, int personId);
        Task SetSettled(int loanId, bool settled);
        Task SetLent(int loanId, bool lent);
        Task<LoanDto> GetLoan(int personId);
        Task<List<LoanDto>> GetLoans();
        Task<List<LoanDto>> GetPersonLoans(int personId);
        Task<List<LoanDto>> GetLoansByDataRange(DateOnly from, DateOnly to);
    }
}
