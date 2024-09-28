using Radzen;

namespace DivvyUp_Shared.Interface
{
    public interface IAlertService
    {
        event Action<string, AlertStyle> OnAlert;
        event Action OnCloseAlert;
        void ShowAlert(string message, AlertStyle style);
        void CloseAlert();
    }
}
