﻿using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using Radzen;
using DivvyUp_Shared.RequestDto;

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
        private List<PersonDto> PeopleAvailable { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }
        private ProductDto Product { get; set; }
        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private bool IsGridEdit { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.GetPersons();
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            IsGridEdit = false;
            Product = await ProductService.GetProduct(ProductId);
            PersonProducts = await PersonProductService.GetPersonProductsFromProduct(ProductId);
            LoadAvailablePersons();
            StateHasChanged();
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
            AddEditPersonProductRequest request = new()
            {
                PersonId = personProduct.PersonId,
                Quantity = personProduct.Quantity
            };

            try
            {
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
            IsGridEdit = false;
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
    }
}
