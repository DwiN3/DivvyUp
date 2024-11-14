using Radzen;

namespace DivvyUp_App.Services.Gui
{
    public class DNotificationService
    {
        private readonly NotificationService _notificationService;

        public DNotificationService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ShowNotification(string message, NotificationSeverity notificationType)
        {
            _notificationService.Notify(new NotificationMessage
            {
                Detail = message,
                Severity = notificationType,
                CloseOnClick = true,
                Duration = 3500,
                Style = "width: 100%;"
            });
        }
    }
}
