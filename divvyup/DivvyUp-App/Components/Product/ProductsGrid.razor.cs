﻿using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
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
        [Inject] IReceiptService ReceiptService { get; set; }
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
        private ReceiptDto Receipt { get; set; }
        private bool IsLoading => Products == null;
        private int MaxQuantityLimit => SelectedPersons.Count() > 0 ? SelectedPersons.Count() : 2;

        private List<PersonDto> SelectedPersons { get; set; } = new List<PersonDto>();

        protected override async Task OnInitializedAsync()
        {
            Receipt = await ReceiptService.GetReceipt(ReceiptId);
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
            SelectedPersons = new List<PersonDto>();
            var product = new ProductDto();
            product.DiscountPercentage = Receipt.DiscountPercentage;
            Products.Add(product);
            await Grid.InsertRow(product);
        }

        private async Task EditRow(ProductDto product)
        {
            SelectedPersons = new List<PersonDto>();
            if (!product.isNew && !product.Divisible)
            {
                SelectedPerson = product.Persons.FirstOrDefault();
            }
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
                AddEditProductDto request = new(product.Name, product.Price, product.Divisible, product.MaxQuantity, product.AdditionalPrice, product.PurchasedQuantity, product.DiscountPercentage, product.TotalPrice);

                if (product.isNew)
                {
                    if (product.Divisible && SelectedPersons.Count() > 0) 
                    {
                        var personIds = SelectedPersons.Select(person => person.Id).ToList();
                        await ProductService.AddWithPersons(request, ReceiptId, personIds);
                    }
                    else
                    {
                        await ProductService.AddWithPerson(request, ReceiptId, SelectedPerson.Id);
                    }
                }
                else
                {
                    var productBeforeEdit = await ProductService.GetProduct(product.Id);
                    if (product.Divisible && productBeforeEdit.Divisible)
                    {
                        var currentEntries = productBeforeEdit.MaxQuantity - productBeforeEdit.AvailableQuantity;
                        if (productBeforeEdit.MaxQuantity > product.MaxQuantity && currentEntries > product.MaxQuantity)
                        {
                            var personProductIds = await DDialogService.OpenProductPersonSelectDialog(product.Id, product.MaxQuantity, product.Name);

                            if (personProductIds != null)
                            {
                                await PersonProductService.RemoveList(product.Id, personProductIds);
                                await ProductService.Edit(request, product.Id);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            await ProductService.Edit(request, product.Id);
                        }
                    }
                    else
                    {
                        await ProductService.EditWithPerson(request, product.Id, SelectedPerson.Id);
                    }
                }
                SelectedPersons = new List<PersonDto>();
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
                SelectedPerson = Persons.FirstOrDefault();
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

        private async Task ManagePerson(int productId, string productName)
        {
            await DDialogService.OpenProductPersonDialog(productId, productName);
            await LoadGrid();
        }

        private async Task SelectPersons(ProductDto product)
        {
            SelectedPersons = await DDialogService.OpenPersonSelectDialog(product.MaxQuantity, SelectedPersons, product.Name);
            if (SelectedPersons.Count() > 0)
            {
                if (SelectedPersons.Count() > product.MaxQuantity)
                {
                    product.MaxQuantity = SelectedPersons.Count();
                }
            }
        }

        private void OnPersonSet(PersonDto person)
        {
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

        private async Task OpenDetails(bool edit, ProductDto product)
        {
            await DDialogService.OpenProductDetailsDialog(edit, product);
        }
    }
}
