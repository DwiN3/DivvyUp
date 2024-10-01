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
            await _dialogService.OpenAsync<DialogCard>(
                title,
                new Dictionary<string, object> { { "Content", content } },
                new DialogOptions()
            );
        }

        public async Task<bool> OpenYesNoDialog(string title, string content)
        {
            var result = await _dialogService.OpenAsync<DialogYesNoCard>(
                title,
                new Dictionary<string, object> { { "Content", content } },
                new DialogOptions()
            );

            return result != null && (bool)result;
        }

        public async Task OpenProductPersonDialog(int productId)
        {
            await _dialogService.OpenAsync<DialogProductPersonCard>(
                "Osoby przypisane do produktu",
                new Dictionary<string, object> { { "ProductId", productId } },
                new DialogOptions()
            );
        }
    }
}
