using System.Windows.Input;

namespace KLAConference.UI
{
    interface IShellViewModel
    {
        string Status { get; set; }
        ICommand LoadTalksCommand { get; set; }
        ICommand GetScheduleCommnd { get; set; }
    }
}
