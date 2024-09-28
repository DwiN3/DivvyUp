namespace DivvyUp_Shared.Interface
{
    public interface IMyDialogService
    {
        Task OpenDialog(string title, string content);
        Task<bool> OpenYesNoDialog(string title, string content);
    }
}
