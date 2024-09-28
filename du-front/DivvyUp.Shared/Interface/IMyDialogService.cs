using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Shared.Interface
{
    public interface IMyDialogService
    {
        Task OpenDialog(string title, string content);
        Task<bool> OpenYesNoDialog(string title, string content);
    }
}
