using DivvyUp_App.BaseComponents.DLoadingPanel;
using DivvyUp_App.Services.Gui;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Dashboard
{
    partial class DashboardGrid
    {
        [Inject]
        private UserAppService UserApp { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private bool UserView { get; set; } = false;
        private int ChartsToLoad { get; set; } = 6;
        private int TotalCharts { get; set; } = 6;
        private bool IsLoading { get; set; } = true;
        private double Value { get; set; }

        // DO Poprawki
        protected override async Task OnInitializedAsync()
        {
            await InitializeUserApp();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (UserApp != null && UserApp.IsLoggedIn())
            {
                IsLoading = true;
                await InitializeUserApp();
            }
        }

        private async Task InitializeUserApp()
        {
            if (UserApp != null)
            {
                await Task.Delay(500);
                UserView = UserApp.IsLoggedIn();
                if (!UserView)
                {
                    IsLoading = false;
                }

                StateHasChanged();
            }
        }

        private void OnChartLoaded()
        {
            ChartsToLoad--;
            Value = Math.Round(((TotalCharts - ChartsToLoad) / (double)TotalCharts) * 100);
            if (ChartsToLoad == 0)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
    }
}
