using DivvyUp_App.BaseComponents.DLoadingPanel;
using DivvyUp_App.Services.Gui;
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
        private int ChartsToLoad { get; set; } = 6;
        private bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(500);
            UserView = UserApp.IsLoggedIn();
            if (!UserView)
            {
                IsLoading = false;
            }
            StateHasChanged();
        }

        private void OnChartLoaded()
        {
            ChartsToLoad--;
            if (ChartsToLoad == 0)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
    }
}
