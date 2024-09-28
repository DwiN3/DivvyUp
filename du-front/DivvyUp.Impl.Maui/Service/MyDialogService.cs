using Radzen;
using DivvyUp_Shared.Interface;
//using DivvyUp_App.Components.DDialog;

namespace DivvyUp_Impl_Maui.Service
{
    public class MyDialogService : IMyDialogService
    {
        private readonly DialogService _dialogService;

        public MyDialogService(DialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }


        public Task OpenDialog(string title, string content)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenYesNoDialog(string title, string content)
        {
            throw new NotImplementedException();
        }

        /*

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
        
        */
    }
}
