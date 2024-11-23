using DivvyUp_App.BaseComponents.DDialog;
using DivvyUp_App.BaseComponents.DDialog.Loan;
using DivvyUp_App.BaseComponents.DDialog.Person;
using DivvyUp_App.BaseComponents.DDialog.PersonProduct;
using DivvyUp_App.BaseComponents.DDialog.User;
using DivvyUp_Shared.Dtos.Entity;
using Radzen;


namespace DivvyUp_App.Services.Gui
{
    public class DDialogService
    {
        private readonly DialogService _dialogService;

        public DDialogService(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task OpenDialog(string title, string content)
        {
            await _dialogService.OpenAsync<DDialogCard>(
                title,
                new Dictionary<string, object> { { "Content", content } },
                new DialogOptions()
            );
        }

        public async Task<bool> OpenYesNoDialog(string title, string content)
        {
            var result = await _dialogService.OpenAsync<DDialogYesNoCard>(
                title,
                new Dictionary<string, object> { { "Content", content } },
                new DialogOptions()
            );

            return result != null && (bool)result;
        }

        public async Task OpenResetPasswordDialog()
        { 
            await _dialogService.OpenAsync<DDialogResetPasswordCard>(
                "Zmiana hasła",
                new Dictionary<string, object> { },
                new DialogOptions()
            );
        }

        public async Task<bool> OpenProductPersonDialog(int productId)
        {
            var result = await _dialogService.OpenAsync<DDialogPersonProductCard>(
                "Osoby przypisane do produktu",
                new Dictionary<string, object> { { "ProductId", productId } },
                new DialogOptions()
                {
                    Width = "85%",
                    Height = "85%"
                }
            );

            return result != null && (bool)result;
        }

        public async Task<bool> OpenLoanDialog(int personId)
        {
            var result = await _dialogService.OpenAsync<DDialogLoanCard>(
                "Pożyczki osoby",
                new Dictionary<string, object> { { "PersonId", personId } },
                new DialogOptions()
                {
                    Width = "85%",
                    Height = "85%"
                }
            );

            return result != null && (bool)result;
        }

        public async Task<List<int>> OpenProductPersonSelectDialog(int productId, int maxQuantity)
        {
            var result = await _dialogService.OpenAsync<DDialogPersonProductSelectCard>(
                "Wybierz osoby, które zostaną przypisane do produktu",
                new Dictionary<string, object>
                {
                    { "ProductId", productId },
                    { "MaxQuantity", maxQuantity }
                },
                new DialogOptions
                {
                    Width = "85%",
                    Height = "85%"
                }
            );

            return result as List<int>;
        }

        public async Task<bool> OpenPersonProductDialog(int personId)
        {
            var result = await _dialogService.OpenAsync<DDialogPersonProductFromPersonCard>(
                "Przypisania osoby do produktów",
                new Dictionary<string, object> { { "PersonId", personId } },
                new DialogOptions()
                {
                    Width = "85%",
                    Height = "85%"
                }
            );

            return result != null && (bool)result;
        }

        public async Task<List<PersonDto>> OpenPersonSelectDialog(int maxQuantity, List<PersonDto> SelectedPersons)
        {
            var result = await _dialogService.OpenAsync<DDialogPersonSelectCard>(
                "Wybierz osoby które zostaną wpisane do tego produktu",
                new Dictionary<string, object>
                {
                    { "MaxQuantity", maxQuantity },
                    { "SelectedPersons", SelectedPersons}
                },
                new DialogOptions
                {
                    Width = "85%",
                    Height = "85%"
                }
            );

            return result as List<PersonDto>;
        }
    }
}
