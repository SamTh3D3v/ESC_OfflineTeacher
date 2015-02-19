using System.Windows;
using ESC_OfflineTeacher.ViewModel;
using MahApps.Metro.Controls;

namespace ESC_OfflineTeacher
{
    
    public partial class MainWindow : MetroWindow
    {
     
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();


        }

       
    }
}