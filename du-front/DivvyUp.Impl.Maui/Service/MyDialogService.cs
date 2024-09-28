using DivvyUp_Shared.Interface;
using Radzen;

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
    }
}
