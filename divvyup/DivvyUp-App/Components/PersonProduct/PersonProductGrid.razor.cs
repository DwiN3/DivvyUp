using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Radzen;
using DivvyUp_Shared.Enums;
using DivvyUp_Shared.Models;

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
        public GridMode GridMode { get; set; } = GridMode.Normal;
        [Parameter]
        public int ProductId { get; set; }
        [Parameter]
        public int MaxQuantity { get; set; }
        [Parameter]
        public List<int> PersonProductsUnSelectedIds { get; set; }
        [Parameter]
        public EventCallback<List<int>> PersonProductsUnSelectedIdsChanged { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonDto> PeopleAvailable { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private ProductDto Product { get; set; }
        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsLoading => PersonProducts == null;
        private bool NormalView => GridMode == GridMode.Normal;
        private IList<PersonProductDto> SelectedPersonProducts = new List<PersonProductDto>();

        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Product = await ProductService.GetProduct(ProductId);
            PersonProducts = await PersonProductService.GetPersonProductsFromProduct(ProductId);
            LoadAvailablePersons();
            StateHasChanged();
        }

        private async Task SetAutoCompensation()
        {
            try
            {
                await PersonProductService.SetAutoCompensation(ProductId);
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private void LoadAvailablePersons()
        {
            var productPersonIds = Product.Persons.Select(p => p.Id).ToHashSet();
            PeopleAvailable = Persons.Where(p => !productPersonIds.Contains(p.Id)).ToList();
        }


        private async Task InsertRow()
        {
            if (Product.AvailableQuantity > 0)
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

        private async Task CancelEdit(PersonProductDto personProduct)
        {
            Grid.CancelEditRow(personProduct);
            await LoadGrid();
        }

        private async Task SaveRow(PersonProductDto personProduct)
        {
            try
            {
                AddEditPersonProductDto request = new(personProduct.PersonId, personProduct.Quantity);

                if (personProduct.Id == 0)
                    await PersonProductService.Add(request, ProductId);
                else
                    await PersonProductService.Edit(request, personProduct.Id);
            }
            catch (DException)
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

        private async Task ChangeCompensation(int personProductId)
        {
            try
            {
                await PersonProductService.SetCompensation(personProductId);
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

        private async Task OnPersonChange(PersonProductDto personProduct, int personId)
        {
            try
            {
                personProduct.PersonId = personId;
                await PersonProductService.SetPerson(personProduct.Id, personId);
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification("Ta osoba już jest wpisana do tego produktu", NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private string GetPersonName(int personId)
        {
            if (Persons != null && Persons.Count > 0)
            {
                PersonDto person = Persons.FirstOrDefault(e => e.Id == personId);

                if (person != null)
                    return $"{person.Name} {person.Surname}";
            }

            return "-";
        }

        private async Task ChangeSelected(PersonProductDto personProduct, bool isChecked)
        {
            var totalQuantity = SelectedPersonProducts.Sum(p => p.Quantity);

            if ((totalQuantity + personProduct.Quantity <= MaxQuantity || !isChecked))
            {
                if (isChecked)
                {
                    SelectedPersonProducts.Add(personProduct);
                }
                else
                {
                    SelectedPersonProducts.Remove(personProduct);
                }
            }
            else
            {
                DNotificationService.ShowNotification("Przekroczono możliwą ilość przypisań do osób", NotificationSeverity.Error, 1500);
            }

            var allProductIds = PersonProducts.Select(p => p.Id).ToList();
            PersonProductsUnSelectedIds = allProductIds
                .Except(SelectedPersonProducts.Select(p => p.Id))
                .ToList();

            await PersonProductsUnSelectedIdsChanged.InvokeAsync(PersonProductsUnSelectedIds);
        }
    }
}
