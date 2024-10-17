﻿using DivvyUp_App.GuiService;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Dashboard
{
    partial class DashboardGrid
    {
        [Inject]
        private UserAppService UserAppService { get; set; }

        private InfoCard Info { get; set; }
        private bool UserView { get; set; } = false;
        private bool DataAlreadySet { get; set; } = false;

        protected async override void OnAfterRender(bool firstRender)
        {
            if ((UserAppService != null) && !DataAlreadySet)
            {
                UserView = UserAppService.IsLoggedIn();
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
