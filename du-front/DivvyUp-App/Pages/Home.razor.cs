using DivvyUp_Impl_Maui.Service;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Pages
{
    public partial class Home
    {
        [Inject]
        private UserAppService User { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}
