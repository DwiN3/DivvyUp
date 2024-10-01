using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace DivvyUp_App.Components.Product
{
    partial class ProductsGrid
    {
        [Parameter]
        public int ReceiptId { get; set; }
        [Inject]
        private IProductService ProductService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private List<ProductDto> Products { get; set; }
        private RadzenDataGrid<ProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Products = await ProductService.ShowProducts(ReceiptId);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var product = new ProductDto();
            Products.Add(product);
            await Grid.InsertRow(product);
        }

        private async Task EditRow(ProductDto product)
        {
            await Grid.EditRow(product);
        }

        private void CancelEdit(ProductDto product)
        {
            Grid.CancelEditRow(product);
        }

        private async Task SaveRow(ProductDto product)
        {
            try
            {
                product.receiptId = ReceiptId;

                if (product.productId == 0)
                    await ProductService.AddProduct(product);
                else
                    await ProductService.EditProduct(product);
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

        private async Task RemoveRow(int productId)
        {
            try
            {
                await ProductService.RemoveProduct(productId);
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

        private async Task ChangeSettled(int productId, bool isChecked)
        {
            try
            {
                await ProductService.SetSettledProduct(productId, isChecked);
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }
        
        }
    }
}
