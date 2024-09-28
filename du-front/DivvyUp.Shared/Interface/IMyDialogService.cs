using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp.Shared.Interface
{
    public interface IMyDialogService
    {
        Task OpenDialog(string title, string content);
        Task<bool> OpenYesNoDialog(string title, string content);
    }
}
