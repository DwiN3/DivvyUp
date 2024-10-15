using System.Text;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using DivvyUp_Shared.Exceptions;

namespace DivvyUp_App.Components.Product
{
    partial class ProductsGrid
    {
        [Parameter]
        public int ReceiptId { get; set; }
        [Inject]
        private IProductService ProductService { get; set; }
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private IPersonProductService PersonProductService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }

        private List<ProductDto> Products { get; set; }
        private List<PersonDto> Persons { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private RadzenDataGrid<ProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private PersonDto SelectedPerson { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
            Persons = await PersonService.GetPersons();
            if (Persons != null && Persons.Count > 0)
            {
                SelectedPerson = Persons.First();
            }
            PersonProducts = await PersonProductService.GetPersonProducts();
        }

        private async Task LoadGrid()
        {
            Products = await ProductService.GetProducts(ReceiptId);
            foreach (var product in Products) 
                product.persons = await PersonService.GetPersonFromProduct(product.id);
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
                var newProduct = new ProductDto();

                if (product.id == 0) 
                    newProduct = await ProductService.AddProduct(product);
                
                else
                    newProduct = await ProductService.EditProduct(product);
                
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
                    await PersonProductService.AddPersonProduct(personProduct, newProduct.id);
                }
            }
            catch (ValidException ex)
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
            catch (ValidException ex)
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
            catch (ValidException ex)
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
                await PersonProductService.ChangePersonPersonProduct(personProduct.id, person.id);
                
            }
            catch (ValidException ex)
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
