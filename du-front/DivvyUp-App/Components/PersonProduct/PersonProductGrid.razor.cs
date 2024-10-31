﻿using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using Radzen;

namespace DivvyUp_App.Components.PersonProduct
{
    partial class PersonProductGrid
    {
        [Inject]
        private IPersonHttpService PersonService { get; set; }
        [Inject]
        private IPersonProductHttpService PersonProductService { get; set; }
        [Inject]
        private IProductHttpService ProductService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonDto> PeopleAvailable { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private ProductDto Product { get; set; }
        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsGridEdit { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            Product = await ProductService.GetProduct(ProductId);
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            IsGridEdit = false;
            PersonProducts = await PersonProductService.GetPersonProductsFromProduct(ProductId);
            await LoadAvailablePersons();
            StateHasChanged();
        }

        private async Task LoadAvailablePersons()
        {
            var personProductIds = PersonProducts?.Select(pp => pp.personId).ToHashSet() ?? new HashSet<int>();
            PeopleAvailable = Persons.Where(p => !personProductIds.Contains(p.id)).ToList();
        }

        private async Task InsertRow()
        {
            if (CountLastPart(0) > 0)
            {
                IsGridEdit = true;
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
            IsGridEdit = true;
            await Grid.EditRow(personProduct);
        }

        private void CancelEdit(PersonProductDto personProduct)
        {
            IsGridEdit = false;
            Grid.CancelEditRow(personProduct);
        }

        private async Task SaveRow(PersonProductDto personProduct)
        {
            IsGridEdit = false;
            try
            {
                if (personProduct.id == 0)
                    await PersonProductService.Add(personProduct, ProductId);
                else
                    await PersonProductService.Edit(personProduct);
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
            IsGridEdit = false;
            try
            {
                await PersonProductService.Remove(personProduct.id);
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
                personProduct.personId = personId;
                await PersonProductService.SetPerson(personProduct.id, personId);
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
