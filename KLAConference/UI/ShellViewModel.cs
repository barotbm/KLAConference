using KLAConference.Infrastructure;
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
            LoadTalksCommand = new RelayCommand(ExecuteLoadTalks);
            GetScheduleCommnd = new RelayCommand(ExecuteGetSchedule);
        }

        #endregion

        void ExecuteLoadTalks(object parameter)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
          //  dialog.DefaultExt = ".csv";
           // dialog.Filter = "CSV Files (*.csv)|*.csv";
            dialog.Filter = "Json files (*.json)|*.json";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dialog.FileName;
            }
        }

        void ExecuteGetSchedule(object parameter)
        {

        }

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
