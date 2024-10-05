using DivvyUp_App.GuiService;
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
            Persons = await PersonService.GetPersons();
            if (Persons != null && Persons.Count > 0)
            {
                SelectedPerson = Persons.First();
            }
            PersonProducts = await PersonProductService.GetPersonProducts();
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Products = await ProductService.GetProducts(ReceiptId);
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

                if (product.id == 0)
                {
                    var newProduct = await ProductService.AddProduct(product);
                    if (!product.divisible && Persons.Count > 0)
                    {
                        PersonProductDto personProduct = new PersonProductDto
                        {
                            productId = newProduct.id,
                            personId = SelectedPerson.id,
                            maxQuantity = product.maxQuantity,
                            compensation = true,
                            partOfPrice = product.price,
                            quantity = 1
                        };
                        await PersonProductService.AddPersonProduct(personProduct, newProduct.id);
                    }
                }
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

        private async Task ManagePerson(int productId)
        {
            await DDialogService.OpenProductPersonDialog(productId);
        }

        private void OnPersonSet(object personObject)
        {
            if (personObject is PersonDto person)
            {
                SelectedPerson = person;
            }
        }

        private List<PersonDto> GetPersonNames(int productId)
        {
            List<PersonDto> persons = new List<PersonDto>();

            if (PersonProducts != null)
            {
                foreach (var personProduct in PersonProducts)
                {
                    if (personProduct.productId == productId)
                    {
                        var person = Persons.FirstOrDefault(c => c.id == personProduct.personId);
                        if (person != null)
                            persons.Add(person);
                    }
                }
            }

            return persons;
        }
    }
}
