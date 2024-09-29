using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace DivvyUp_App.Components.Person
{
    partial class PersonGrid : ComponentBase
    {
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }

        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<PersonDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Persons = await PersonService.ShowPersons();
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
                if (person.personId == 0)
                    await PersonService.AddPerson(person);
                else
                    await PersonService.EditPerson(person);
            }
            catch (InvalidOperationException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async void RemoveRow(int personId)
        {
            try
            {
                await PersonService.RemovePerson(personId);
                AlertService.ShowAlert("Usunięto osobę", AlertStyle.Success);
            }
            catch (InvalidOperationException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }
    }
}
