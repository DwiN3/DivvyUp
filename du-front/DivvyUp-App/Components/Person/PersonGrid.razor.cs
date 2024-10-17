using DivvyUp_App.GuiService;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using DivvyUp_Shared.Exceptions;

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

        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<PersonDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };

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

        private void CancelEdit(PersonDto person)
        {
            Grid.CancelEditRow(person);
        }

        private async Task SaveRow(PersonDto person)
        {
            try
            {
                if (person.id == 0)
                    await PersonService.AddPerson(person);
                else
                    await PersonService.EditPerson(person);
            }
            catch (DuException ex)
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
                var result = await DDialogService.OpenYesNoDialog("Usuwanie Osoby", $"Czy potwierdzasz usunięcie osoby: {person.fullName}?");
                if (result)
                    await PersonService.RemovePerson(person.id);
            }
            catch (DuException ex)
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
    }
}
