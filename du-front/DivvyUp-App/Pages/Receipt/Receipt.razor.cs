using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using DivvyUp_Web.Api.Interface;
using DivvyUp_App.Components.ReceiptComponents;

namespace DivvyUp_App.Pages.Receipt
{
    partial class Receipt
    {
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        private ReceiptGrid ReceiptGrid { get; set; }


        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task Logout()
        {
            await LocalStorage.SetItemAsync("authToken", "");
            Navigation.NavigateTo("/");
        }
    }
}
