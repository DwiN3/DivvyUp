using DivvyUp_App.Service.Gui;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Dashboard
{
    partial class DashboardGrid
    {
        [Inject]
        private UserAppService UserApp { get; set; }

        private InfoCard Info { get; set; }
        private bool UserView { get; set; } = false;
        private bool DataAlreadySet { get; set; } = false;

        protected async override void OnAfterRender(bool firstRender)
        {
            if ((UserApp != null) && !DataAlreadySet)
            {
                UserView = UserApp.IsLoggedIn();
                StateHasChanged();

                if (Info != null && UserView)
                {
                    DataAlreadySet = true;
                    await Info.SetInfo();
                }
            }
        }
    }
}
