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

namespace ESC_OfflineTeacher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SolidColorBrush Dark = new SolidColorBrush(Colors.DarkGray);
        public static SolidColorBrush Light = new SolidColorBrush(Colors.DeepSkyBlue);
        static App()
        {
            DispatcherHelper.Initialize();
            // Setup Quick Converter.            
            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));
            QuickConverter.EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));

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
