using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Radzen;

namespace DivvyUp_App.Components.PersonProduct
{
    partial class PersonProductFromPersonGrid
    {
        [Inject]
        private IPersonProductService PersonProductService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        [Parameter]
        public int PersonId { get; set; }

        private PersonDto Person { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsLoading => PersonProducts == null;

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            PersonProducts = await PersonProductService.GetPersonProductsFromPerson(PersonId);
        }

        private async Task EditRow(PersonProductDto personProduct)
        {
            await Grid.EditRow(personProduct);
        }

        private async Task CancelEdit(PersonProductDto personProduct)
        {
            Grid.CancelEditRow(personProduct);
            await LoadGrid();
        }

        private async Task RemoveRow(PersonProductDto personProduct)
        {
            try
            {
                await PersonProductService.Remove(personProduct.Id);
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

        private async Task ChangeSettled(int personProductId, bool isChecked)
        {
            try
            {
                await PersonProductService.SetSettled(personProductId, isChecked);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task OpenPersonProductList(int productId, string productName)
        {
            await DDialogService.OpenProductPersonDialog(productId, productName);
            await LoadGrid();
        }
    }
}
