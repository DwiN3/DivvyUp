using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_App.GuiService;
using Radzen;
using DivvyUp_Shared.Exceptions;

namespace DivvyUp_App.Components.PersonProduct
{
    partial class PersonProductGrid
    {
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private IPersonProductService PersonProductService { get; set; }
        [Inject]
        private IProductService ProductService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private ProductDto Product { get; set; }

        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };


        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            Product = await ProductService.GetProduct(ProductId);
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            PersonProducts = await PersonProductService.GetPersonProductsFromProduct(ProductId);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            if (CountLastPart(0) > 0)
            {
                var personProduct = new PersonProductDto();
                PersonProducts.Add(personProduct);
                await Grid.InsertRow(personProduct);
            }
            else
            {
                DNotificationService.ShowNotification("Nie można przypisać więcej osób", NotificationSeverity.Error);
            }
        }

        private async Task EditRow(PersonProductDto personProduct)
        {
            await Grid.EditRow(personProduct);
        }

        private void CancelEdit(PersonProductDto personProduct)
        {
            Grid.CancelEditRow(personProduct);
        }

        private async Task SaveRow(PersonProductDto personProduct)
        {
            try
            {
                if (personProduct.id == 0)
                    await PersonProductService.Add(personProduct, ProductId);
                else
                    await PersonProductService.Edit(personProduct);
            }
            catch (DuException)
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

        private async Task RemoveRow(PersonProductDto personProduct)
        {
            try
            {
                await PersonProductService.Remove(personProduct.id);
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

        private async Task ChangeSettled(int personProductId, bool isChecked)
        {
            try
            {
                await PersonProductService.SetSettled(personProductId, isChecked);
            }
            catch (DuException ex)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task ChangeCompensation(int personProductId)
        {
            try
            {
                await PersonProductService.SetCompensation(personProductId);
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

        private async Task OnPersonChange(PersonProductDto personProduct, int personId)
        {
            try
            {
                personProduct.personId = personId;
                await PersonProductService.SetPerson(personProduct.id, personId);
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

        private int CountLastPart(int personProductId)
        {
            if (Product != null)
            {
                int lastParts = Product.maxQuantity;

                foreach (var personProduct in PersonProducts)
                {
                    if (personProduct.id == 0 || personProductId == personProduct.id)
                        continue;
                    
                    lastParts -= personProduct.quantity;
                }

                return lastParts;
            }

            return 0;
        }

        private string GetPersonName(int personId)
        {
            if (Persons != null && Persons.Count > 0)
            {
                PersonDto person = Persons.FirstOrDefault(e => e.id == personId);

                if (person != null)
                    return $"{person.name} {person.surname}";
            }

            return "-";
        }
    }
}
