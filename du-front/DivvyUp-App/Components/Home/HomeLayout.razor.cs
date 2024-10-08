using DivvyUp_App.GuiService;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.Home
{
    partial class HomeLayout
    {
        [Inject]
        private UserAppService UserAppService { get; set; }
    }
}
