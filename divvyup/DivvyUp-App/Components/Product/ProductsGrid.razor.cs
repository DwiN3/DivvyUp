using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Product
{
    partial class ProductsGrid
    {
        [Inject]
        private IProductService ProductService { get; set; }
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private IPersonProductService PersonProductService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        [Parameter]
        public int ReceiptId { get; set; }

        private List<ProductDto> Products { get; set; }
        private List<PersonDto> Persons { get; set; }
        private RadzenDataGrid<ProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto SelectedPerson { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
            Persons = await PersonService.GetPersons();
            if (Persons != null && Persons.Count > 0)
                SelectedPerson = Persons.First();
        }

        private async Task LoadGrid()
        {
            Products = await ProductService.GetProductsFromReceipt(ReceiptId);
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

        private async Task CancelEdit(ProductDto product)
        {
            Grid.CancelEditRow(product);
            await LoadGrid();
        }

        private async Task SaveRow(ProductDto product)
        {
            try
            {
                AddEditProductDto request = new(product.Name, product.Price, product.Divisible, product.MaxQuantity);

                var newProduct = new ProductDto();

                if (product.Id == 0) 
                    newProduct = await ProductService.Add(request, ReceiptId);
                
                else
                    newProduct = await ProductService.Edit(request, product.Id);
                
                if (!product.Divisible && Persons.Count > 0)
                {
                    AddEditPersonProductDto requestPersonproduct = new()
                    {
                        PersonId = SelectedPerson.Id,
                        Quantity = 1,
                    };

                    await PersonProductService.Add(requestPersonproduct, newProduct.Id);
                }
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

        private async Task RemoveRow(ProductDto product)
        {
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie produktu", $"Czy potwierdzasz usunięcie produktu: {product.Name}?");
                if (result)
                    await ProductService.Remove(product.Id);
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

        private async Task Duplicate(ProductDto product)
        {
            var newProduct = new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                ReceiptId = product.ReceiptId,
                Divisible = product.Divisible,
                MaxQuantity = product.MaxQuantity
            };
            Products.Add(newProduct);
            await Grid.InsertRow(newProduct);
        }

        private async Task ChangeSettled(int productId, bool isChecked)
        {
            try
            {
                await ProductService.SetSettled(productId, isChecked);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task ManagePerson(int productId)
        {
            var result = await DDialogService.OpenProductPersonDialog(productId);
            if (!result) 
                await LoadGrid();
        }

        private void OnPersonSet(object personObject)
        {
            if (personObject is PersonDto person)
                SelectedPerson = person;
        }

        private async Task OnPersonChange(int productId, PersonDto person)
        {
            var personProductList = await PersonProductService.GetPersonProductsFromProduct(productId);
            var personProduct = personProductList.First();
            try
            {
                await PersonProductService.SetPerson(personProduct.Id, person.Id);
                
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
    }
}
