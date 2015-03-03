using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace ESC_OfflineTeacher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
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
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }
    }
}
