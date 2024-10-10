using DivvyUp_App.BaseComponents.DDialog;
using Radzen;


namespace DivvyUp_App.GuiService
{
    public class DDialogService
    {
        private readonly DialogService _dialogService;

        public DDialogService(DialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
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

        public async Task<bool> OpenResetPasswordDialog()
        {
            var result = await _dialogService.OpenAsync<DDialogResetPasswordCard>(
                "Zmiana hasła",
                new Dictionary<string, object> { },
                new DialogOptions()
            );

            return result != null && (bool)result;
        }

        public async Task<bool> OpenProductPersonDialog(int productId)
        {
            var result = await _dialogService.OpenAsync<DDialogProductPersonCard>(
                "Osoby przypisane do produktu",
                new Dictionary<string, object> { { "ProductId", productId } },
                new DialogOptions()
                {
                    Width = "80%",
                    Height = "75%"
                }
            );

            return result != null && (bool)result;
        }
    }
}
