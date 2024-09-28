using DivvyUp_Shared.Interface;
using Radzen;

namespace DivvyUp_Impl_Maui.Service
{
    public class DAlertService
    {
        public event Action<string, AlertStyle> OnAlert;
        public event Action OnCloseAlert;

        public void ShowAlert(string message, AlertStyle style)
        {
            OnAlert?.Invoke(message, style);
        }

        public void CloseAlert()
        {
            OnCloseAlert?.Invoke();
        }
    }
}
