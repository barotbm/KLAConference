using KLAConference.Entities;
using KLAConference.Infrastructure;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            var dialog = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.Filter = "Json files (*.json)|*.json";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    List<Talk> items = JsonConvert.DeserializeObject<List<Talk>>(json);
                }
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
