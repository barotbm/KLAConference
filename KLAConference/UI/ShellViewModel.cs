﻿using KLAConference.Algorithm;
using KLAConference.Entities;
using KLAConference.Infrastructure;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KLAConference.UI
{
    public class ShellViewModel : IShellViewModel, INotifyPropertyChanged
    {
        #region Fields

        private string _status = "";
        private ConferenceEngine _conferenceEngine;
        private List<Talk> _talks;
        private bool _isResultAvailable = false;

        #endregion

        #region Properties

        public bool IsResultAvailable
        {
            get
            {
                return _isResultAvailable;
            }
            set
            {
                _isResultAvailable = value;
                OnPropertyRaised("IsResultAvailable");
            }
        }

        public ObservableCollection<Talk> ScheduledTalks { get; private set; } = new ObservableCollection<Talk>();

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
            LoadTalksCommand = new AsyncCommand(ExecuteLoadTalks);
            GetScheduleCommnd = new AsyncCommand(ExecuteGetSchedule);

            LoadInfrastructure();
        }

        #endregion

        #region Commands       

        private async Task LoadInfrastructure()
        {
            Status = Constants.Loading;
            // Load the infrastructure in the background
            // Note: Ideally the engine  should be loaded using Unity to avoid tight coupling
            await Task.Run(() =>
            {

                string filename = ConfigurationManager.AppSettings[Constants.ConfigPath];
                Config config;
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
                _conferenceEngine = new ConferenceEngine(config);
            });
            Status = Constants.Ready;
        }

        async Task ExecuteLoadTalks()
        {
            // Create OpenFileDialog 
            var dialog = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.Filter = Constants.JsonFilter;
            dialog.InitialDirectory = Path.Combine(System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase), Constants.TestInput);

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                Status = Constants.LoadingTalks;
                IsResultAvailable = false;

                await Task.Run(() =>
                {
                    string filename = dialog.FileName;
                    using (StreamReader r = new StreamReader(filename))
                    {
                        string json = r.ReadToEnd();
                        _talks = JsonConvert.DeserializeObject<List<Talk>>(json);

                        foreach (var talk in _talks)
                        {
                            // Assumption: Talk's name should not have any other numbers
                            Match m = Regex.Match(talk.Name, Constants.NumberRegex);
                            if (string.IsNullOrEmpty(m.Value))
                            {
                                talk.Duration = 5;
                            }
                            else
                            {
                                talk.Duration = Convert.ToInt32(m.Value);
                            }
                        }
                    }
                });

                Status = Constants.LoadedTalks;
            }
        }

        async Task ExecuteGetSchedule()
        {
            Status = Constants.GetFinalSchedule;
            ScheduledConference result = null;
            await Task.Run(() =>
            {
               result = _conferenceEngine.Run(_talks);
            });

            if (result == null)
            {
                Status =Constants.InternalFailure;
                return;
            }
            
            if(result.Type == ResultType.Success)
            {
                ScheduledTalks = new ObservableCollection<Talk>(result.OrderedTalks);
                IsResultAvailable = true;
                OnPropertyRaised("ScheduledTalks");
            }
            else
            {

            }
            

            Status = result.Type.ToString();
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
