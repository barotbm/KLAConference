using System.ComponentModel;
using System.Windows;

namespace KLAConference.UI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        /* Ideally most of the code here should go to ViewModel
         * 
         */

        #region Constructor
        public Shell()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #endregion        
    }
}
