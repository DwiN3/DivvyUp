using DivvyUp_App.GuiService;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Dashboard
{
    partial class DashboardGrid
    {
        [Inject]
        private UserAppService UserAppService { get; set; }

        private bool UserView { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (UserAppService != null)
            {
                UserView = UserAppService.IsLoggedIn();
                StateHasChanged();
            }
        }
    }
}
