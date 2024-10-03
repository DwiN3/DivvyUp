using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace DivvyUp_App.Components.Item
{
    partial class ItemGrid
    {
        [Parameter]
        public int ReceiptId { get; set; }
        [Inject]
        private IItemService ItemService { get; set; }
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        private List<ItemDto> Items { get; set; }
        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<ItemDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto selectedPerson { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
            Persons = await PersonService.GetPersons();
        }

        private async Task LoadGrid()
        {
            Items = await ItemService.GetItems(ReceiptId);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var item = new ItemDto();
            Items.Add(item);
            await Grid.InsertRow(item);
        }

        private async Task EditRow(ItemDto item)
        {
            await Grid.EditRow(item);
        }

        private void CancelEdit(ItemDto item)
        {
            Grid.CancelEditRow(item);
        }

        private async Task SaveRow(ItemDto item)
        {
            try
            {
                item.receiptId = ReceiptId;

                if (item.id == 0)
                    await ItemService.AddItem(item);
                else
                    await ItemService.EditItem(item);
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

        private async Task RemoveRow(int itemId)
        {
            try
            {
                await ItemService.RemoveItem(itemId);
                AlertService.ShowAlert("Usunięto produkt", AlertStyle.Success);
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

        private async Task ChangeSettled(int itemId, bool isChecked)
        {
            try
            {
                await ItemService.SetSettledItem(itemId, isChecked);
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        
        }

        private async Task ManagePerson(int itemId)
        {
            await DDialogService.OpenPersonItemShareDialog(itemId);
        }

        private void SetPerson(ItemDto item, PersonDto person)
        {
            selectedPerson = person;
        }
    }
}
