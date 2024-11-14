using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface ILoanService
    {
        Task Add(AddEditLoanDto request);
        Task Edit(AddEditLoanDto request, int loanId);
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
