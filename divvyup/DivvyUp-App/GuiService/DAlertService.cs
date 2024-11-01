using Radzen;

namespace DivvyUp_App.GuiService
{
    public class DAlertService
    {
        public event Action<string, AlertStyle> OnAlert;
        public event Action OnCloseAlert;

        public void ShowAlert(string message, AlertStyle style)
        {
            CloseAlert();
            OnAlert?.Invoke(message, style);
        }

        public void CloseAlert()
        {
            OnCloseAlert?.Invoke();
        }
    }
}
