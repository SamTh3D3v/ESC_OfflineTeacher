using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using ESC_OfflineTeacher.Annotations;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ESC_OfflineTeacher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SolidColorBrush Dark = new SolidColorBrush(Colors.LightGray);
        public static SolidColorBrush Light = new SolidColorBrush(Color.FromRgb(8,111,158));
        public static string LocalDbPath = AppDomain.CurrentDomain.BaseDirectory.ToString(CultureInfo.InvariantCulture) + "SGSDB.sdf";
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            DispatcherHelper.Initialize();
        }

        async void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            var controller = await ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Opération non permise, Details :", errorMessage));            
            e.Handled = true;
        }
              
        public static void SelectCulture(string culture)
        {               
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();
            // We want our specific culture      
            string requestedCulture = string.Format("Resources/StringResources.{0}.xaml", culture);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
            if (resourceDictionary == null)
            {

                requestedCulture = "Resources/StringResources.Fr.xaml";
                resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
            }

               
            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }                                      
        }
        

       
    }
}
