using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Enums;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Person
{
    partial class PersonGrid : ComponentBase
    {
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        [Parameter] 
        public PersonGridMode GridMode { get; set; } = PersonGridMode.Normal;
        [Parameter]
        public int MaxQuantityInProduct { get; set; }
        [Parameter]
        public List<PersonDto> SelectedPersons { get; set; } = new List<PersonDto>();
        [Parameter]
        public EventCallback<List<PersonDto>> SelectedPersonsChanged { get; set; }

        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<PersonDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsLoading => Persons == null;
        private bool NormalView => GridMode == PersonGridMode.Normal;
        private IList<PersonDto> SelectedPersonsList { get; set; } = new List<PersonDto>();

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Persons = await PersonService.GetPersons();
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var person = new PersonDto();
            Persons.Add(person);
            await Grid.InsertRow(person);
        }

        private async Task EditRow(PersonDto person)
        {
            await Grid.EditRow(person);
        }

        private async Task CancelEdit(PersonDto person)
        {
            Grid.CancelEditRow(person);
            await LoadGrid();
        }

        private async Task SaveRow(PersonDto person)
        {
            try
            {
                AddEditPersonDto request = new()
                {
                    Name = person.Name,
                    Surname = person.Surname
                };
                if (person.Id == 0)
                    await PersonService.Add(request);
                else
                    await PersonService.Edit(request, person.Id);
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

        private async Task RemoveRow(PersonDto person)
        {
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie osoby", $"Czy potwierdzasz usunięcie osoby: {person.FullName}?");
                if (result)
                    await PersonService.Remove(person.Id);
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

        private async Task ManageLoanPerson(int personId)
        {
            var result = await DDialogService.OpenLoanDialog(personId);
            if (!result)
                await LoadGrid();
        }

        private async Task ManagePersonProduct(int personId)
        {
            var result = await DDialogService.OpenPersonProductDialog(personId);
            if (!result)
                await LoadGrid();
        }

        private async Task ChangeSelected(PersonDto person, bool isChecked)
        {
            if (isChecked)
            {
                SelectedPersonsList.Add(person);
            }
            else
            {
                SelectedPersonsList.Remove(person);
            }

            SelectedPersons = SelectedPersonsList.ToList();
            await SelectedPersonsChanged.InvokeAsync(SelectedPersons);
        }
    }
}
