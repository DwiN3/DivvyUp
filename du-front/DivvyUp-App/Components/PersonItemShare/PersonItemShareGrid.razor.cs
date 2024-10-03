using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radzen;

namespace DivvyUp_App.Components.PersonItemShare
{
    partial class PersonItemShareGrid
    {
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private IPersonItemShareService PersonItemShareService { get; set; }
        [Inject]
        private IItemService ItemService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }

        [Parameter]
        public int ItemId { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonItemShareDto> PersonItems { get; set; }
        private ItemDto Item { get; set; }

        private RadzenDataGrid<PersonItemShareDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };


        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            Item = await ItemService.GetItem(ItemId);
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            PersonItems = await PersonItemShareService.GetPersonItemShares(ItemId);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            if (CountLastPart() > 0)
            {
                var personItem = new PersonItemShareDto();
                PersonItems.Add(personItem);
                await Grid.InsertRow(personItem);
            }
            else
            {
                AlertService.ShowAlert("Nie można przypisać więcej osób", AlertStyle.Secondary);
            }
        }

        private async Task EditRow(PersonItemShareDto personItem)
        {
            await Grid.EditRow(personItem);
        }

        private void CancelEdit(PersonItemShareDto personItem)
        {
            Grid.CancelEditRow(personItem);
        }

        private async Task SaveRow(PersonItemShareDto personItem)
        {
            try
            {
                if (personItem.id == 0)
                    await PersonItemShareService.AddPersonItemShare(personItem, ItemId);
            }
            catch (InvalidOperationException)
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

        private async Task RemoveRow(int personItemId)
        {
            try
            {
                await PersonItemShareService.RemovePersonItemShare(personItemId);
            }
            catch (InvalidOperationException)
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

        private async Task ChangeSettled(int personItemId, bool isChecked)
        {
            try
            {
                await PersonItemShareService.SetPersonItemShare(personItemId, isChecked);
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task ChangeCompensation(int personItemId)
        {
            try
            {
                await PersonItemShareService.SetCompensationPersonItemShare(personItemId);
                await LoadGrid();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        }

        private int CountLastPart()
        {
            int lastParts = Item.maxQuantity;
            
            foreach (var personItem in PersonItems)
                lastParts -= personItem.quantity;

            return lastParts;
        }
    }
}
