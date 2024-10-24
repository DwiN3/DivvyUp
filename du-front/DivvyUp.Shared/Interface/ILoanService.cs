
using DivvyUp_Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Shared.Interface
{
    public interface ILoanService
    {
        Task Add(LoanDto loan);
        Task Edit(LoanDto loan);
        Task Remove(int loanId);
        Task SetPerson(int loadId, int personId);
        Task SetLent(int loanId, bool lent);
        Task SetSettled(int loanId, bool isSettled);
        Task<LoanDto> GetLoan(int loanId);
        Task<List<LoanDto>> GetLoansPerson(int personId);
        Task<List<LoanDto>> GetLoans();
    }
}
