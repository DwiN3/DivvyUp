using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Enum;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Loan
{
    partial class LoanGrid : ComponentBase
    {
        [Inject]
        private ILoanService LoanService { get; set; }
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        [Parameter]
        public LoanGridMode GridMode { get; set; }
        [Parameter]
        public int PersonId { get; set; }

        private List<LoanDto> Loans { get; set; }
        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<LoanDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto SelectedPerson { get; set; } = new();
        private DateOnly DateFrom { get; set; }
        private DateOnly DateTo { get; set; }
        private bool ShowAllLoans = false;

        protected override async Task OnInitializedAsync()
        {
            await SetCurrentMonth();
            if (GridMode == LoanGridMode.All)
            {
                Persons = await PersonService.GetPersons();
                var userAccount = Persons.Single(r => r.UserAccount);
                Persons.Remove(userAccount);
                if (Persons != null && Persons.Count > 0)
                    SelectedPerson = Persons.First();
            }
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            if (GridMode == LoanGridMode.All)
            {
                if(ShowAllLoans)
                    Loans = await LoanService.GetLoans();
                else 
                    Loans = await LoanService.GetLoansByDataRange(DateFrom, DateTo);
            }
            else
            {
                Loans = await LoanService.GetPersonLoans(PersonId);
            }

            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var loan = new LoanDto();
            Loans.Add(loan);
            await Grid.InsertRow(loan);
        }

        private async Task EditRow(LoanDto loan)
        {
            await Grid.EditRow(loan);
        }

        private async Task CancelEdit(LoanDto loan)
        {
            Grid.CancelEditRow(loan);
            await LoadGrid();
        }

        private async Task SaveRow(LoanDto loan)
        {
            try
            {
                AddEditLoanDto request = new(loan.PersonId, loan.Date, loan.Amount, loan.Lent);

                if (loan.Id == 0)
                {
                    if(GridMode == LoanGridMode.All)
                        loan.PersonId = SelectedPerson.Id;
                    else
                        loan.PersonId = PersonId;

                    await LoanService.Add(request);
                }
                else
                {
                    await LoanService.Edit(request, loan.Id);
                }
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async Task RemoveRow(LoanDto loan)
        {
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie osoby", $"Czy potwierdzasz usunięcie pożyczki?");
                if (result)
                    await LoanService.Remove(loan.Id);
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async Task ChangeSettled(int loanId, bool isSettled)
        {
            try
            {
                await LoanService.SetSettled(loanId, isSettled);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task ChangeLent(int loanId, bool lent)
        {
            try
            {
                await LoanService.SetLent(loanId, lent);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async Task OnPersonChange(LoanDto loan, int personId)
        {
            try
            {
                loan.PersonId = personId;
                await LoanService.SetPerson(loan.Id, personId);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private void OnPersonSet(object personObject)
        {
            if (personObject is PersonDto person)
                SelectedPerson = person;
        }

        private async Task SetCurrentMonth()
        {
            DateFrom = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
            int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTo = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, dayInMonth);
            await LoadGrid();
        }
    }
}
