using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Impl.Service;
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
