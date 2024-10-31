using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
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
        private List<PersonProductDto> PersonProducts { get; set; }
        private RadzenDataGrid<ProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto SelectedPerson { get; set; } = new();
        private bool IsGridEdit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
            Persons = await PersonService.GetPersons();
            if (Persons != null && Persons.Count > 0)
                SelectedPerson = Persons.First();
            
            PersonProducts = await PersonProductService.GetPersonProducts();
        }

        private async Task LoadGrid()
        {
            IsGridEdit = false;
            Products = await ProductService.GetProducts(ReceiptId);
            foreach (var product in Products) 
                product.persons = await PersonService.GetPersonFromProduct(product.id);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            IsGridEdit = true;
            var product = new ProductDto();
            Products.Add(product);
            await Grid.InsertRow(product);
        }

        private async Task EditRow(ProductDto product)
        {
            IsGridEdit = true;
            await Grid.EditRow(product);

        }

        private void CancelEdit(ProductDto product)
        {
            IsGridEdit = false;
            Grid.CancelEditRow(product);
        }

        private async Task SaveRow(ProductDto product)
        {
            IsGridEdit = false;
            try
            {
                product.receiptId = ReceiptId;
                var newProduct = new ProductDto();

                if (product.id == 0) 
                    newProduct = await ProductService.Add(product);
                
                else
                    newProduct = await ProductService.Edit(product);
                
                if (!product.divisible && Persons.Count > 0)
                {
                    PersonProductDto personProduct = new PersonProductDto
                    {
                        productId = newProduct.id,
                        personId = SelectedPerson.id,
                        compensation = true,
                        partOfPrice = product.price,
                        quantity = 1
                    };
                    await PersonProductService.Add(personProduct, newProduct.id);
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
            IsGridEdit = false;
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie produktu", $"Czy potwierdzasz usunięcie produktu: {product.name}?");
                if (result)
                    await ProductService.Remove(product.id);
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
            IsGridEdit = true;
            var newProduct = new ProductDto
            {
                name = product.name,
                price = product.price,
                receiptId = product.receiptId,
                divisible = product.divisible,
                maxQuantity = product.maxQuantity
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
                await PersonProductService.SetPerson(personProduct.id, person.id);
                
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
