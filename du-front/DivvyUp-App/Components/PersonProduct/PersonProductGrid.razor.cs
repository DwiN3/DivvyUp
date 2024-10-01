using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_App.Components.PersonProduct
{
    partial class PersonProductGrid
    {
        [Inject]
        private IPersonService PersonService { get; set; }
        [Inject]
        private IPersonProductService PersonProductService { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        private List<PersonDto> Persons { get; set; }
        private List<PersonProductDto> PersonProducts { get; set; }

        private RadzenDataGrid<PersonProductDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };


        protected override async Task OnInitializedAsync()
        {
            Persons = await PersonService.ShowPersons();
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            PersonProducts = await PersonProductService.ShowPersonProducts(ProductId);
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var personProduct = new PersonProductDto();
            PersonProducts.Add(personProduct);
            await Grid.InsertRow(personProduct);
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
                if (personProduct.personProductId == 0)
                    await PersonProductService.AddProductPerson(personProduct, ProductId);
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

        private async Task RemoveRow(int personProductId)
        {
            try
            {
                await PersonProductService.RemoveProductPerson(personProductId);
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

        private async Task ChangeSettled(int personProduct, bool isChecked)
        {
            try
            {
                await PersonProductService.SetSettledProductPerson(personProduct, isChecked);
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
            }

        }
    }
}
