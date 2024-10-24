using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
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

        private List<LoanDto> Loans { get; set; }
        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<LoanDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto SelectedPerson { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            if (Persons != null && Persons.Count > 0)
                SelectedPerson = Persons.First();
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Loans = await LoanService.GetLoans();
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

        private void CancelEdit(LoanDto loan)
        {
            Grid.CancelEditRow(loan);
        }

        private async Task SaveRow(LoanDto loan)
        {
            try
            {
                if (loan.id == 0)
                {
                    loan.personId = SelectedPerson.id;
                    await LoanService.Add(loan);
                }
                else
                {
                    await LoanService.Edit(loan);
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
                    await LoanService.Remove(loan.id);
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
                loan.personId = personId;
                //await LoanService.SetPerson(loan.id, personId);
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
    }
}
