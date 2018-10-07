using System.ComponentModel;
using System.Windows;

namespace KLAConference.UI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
       #region Constructor
        public Shell()
        {
            InitializeComponent();

            // Note: Should inject the viewmodel using Unity and Interface
            this.DataContext = new ShellViewModel();
        }
        #endregion        
    }
}
