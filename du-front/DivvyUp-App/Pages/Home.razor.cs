using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Pages
{
    public partial class Home
    {
        [Inject]
        private IUserAppService UserAppService { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}
