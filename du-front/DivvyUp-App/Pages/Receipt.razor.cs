using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using DivvyUp_App.Components.ReceiptComponents;
using DivvyUp_Shared.Interface;

namespace DivvyUp_App.Pages
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
    }
}
