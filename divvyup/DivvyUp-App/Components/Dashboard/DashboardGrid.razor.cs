using DivvyUp_App.BaseComponents.DLoadingPanel;
using DivvyUp_App.Services.Gui;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Dashboard
{
    partial class DashboardGrid : IDisposable
    {
        [Inject]
        private UserStateProvider UserStateProvider { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private bool DataIsReady { get; set; } = false; 
        private bool IsLogged { get; set; }
        private int ChartsToLoad { get; set; } = 6;
        private int TotalCharts { get; set; } = 6;
        private bool IsLoading { get; set; } = false;
        private double Value { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLogged = await UserStateProvider.IsLoggedInAsync();
            if (IsLogged)
            {
                IsLoading = true;
            }
            DataIsReady = true;
            UserStateProvider.OnUserStateChanged += OnUserStateChanged;
            StateHasChanged();
        }

        private async void OnUserStateChanged()
        {
            IsLogged = await UserStateProvider.IsLoggedInAsync();
            if (IsLogged)
            {
                IsLoading = true;
            }
            else
            {
                IsLoading = false;
            }
            StateHasChanged();
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

        public void Dispose()
        {
            UserStateProvider.OnUserStateChanged -= OnUserStateChanged;
        }
    }
}
