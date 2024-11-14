﻿using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Enum;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
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
        private bool IsGridEdit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await SetCurrentMonth();
            if (GridMode == LoanGridMode.All)
            {
                Persons = await PersonService.GetPersons();
                var userAccount = Persons.Single(r => r.userAccount);
                Persons.Remove(userAccount);
                if (Persons != null && Persons.Count > 0)
                    SelectedPerson = Persons.First();
            }
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            IsGridEdit = false;
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
            IsGridEdit = true;
            var loan = new LoanDto();
            Loans.Add(loan);
            await Grid.InsertRow(loan);
        }

        private async Task EditRow(LoanDto loan)
        {
            IsGridEdit = true;
            await Grid.EditRow(loan);
        }

        private void CancelEdit(LoanDto loan)
        {
            IsGridEdit = false;
            Grid.CancelEditRow(loan);
        }

        private async Task SaveRow(LoanDto loan)
        {
            IsGridEdit = false;
            try
            {
                AddEditLoanRequest request = new()
                {
                    PersonId = loan.personId,
                    Date = loan.date,
                    Lent = loan.lent,
                    Amount = (decimal)loan.amount,
                };

                if (loan.id == 0)
                {
                    if(GridMode == LoanGridMode.All)
                        loan.personId = SelectedPerson.id;
                    else
                        loan.personId = PersonId;

                    await LoanService.Add(request);
                }
                else
                {
                    await LoanService.Edit(request, loan.id);
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
            IsGridEdit = false;
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
                await LoanService.SetPerson(loan.id, personId);
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
