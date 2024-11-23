using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using Radzen;
using DivvyUp_Shared.Models;

namespace DivvyUp_App.Components.PersonProduct
{
    partial class PersonProductSelectGrid
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
        [Parameter]
        public int MaxQuantity { get; set; }
        [Parameter]
        public List<int> PersonProductsUnSelectedIds { get; set; }
        [Parameter]
        public EventCallback<List<int>> PersonProductsUnSelectedIdsChanged { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private ProductDto Product { get; set; }
        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsLoading => PersonProducts == null;

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
            StateHasChanged();
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
    }
}