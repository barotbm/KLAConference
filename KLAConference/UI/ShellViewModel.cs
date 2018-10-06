using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KLAConference.UI
{
    public class ShellViewModel : IShellViewModel, INotifyPropertyChanged
    {
        #region Fields

        private string _status = "";

        #endregion

        #region Properties

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyRaised("Status");
            }
        }

        public ICommand LoadTalksCommand { get; set; }

        public ICommand GetScheduleCommnd { get; set; }

        #endregion

        #region Constructor

        public ShellViewModel()
        {

        }

        #endregion

        #region INotifyPropertyChanged       

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion
    }
}
