/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ESC_OfflineTeacher.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;


namespace ESC_OfflineTeacher.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SetupNavigation();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<NoteExaminsViewModel>();
            SimpleIoc.Default.Register<NoteDettesViewModel>();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public NoteExaminsViewModel NoteExaminsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteExaminsViewModel>();
            }
        }   
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public NoteDettesViewModel NoteDettesProperty
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteDettesViewModel>();
            }
        }
        private static void SetupNavigation()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("LoginView", new Uri("../Views/LoginView.xaml"));
            navigationService.Configure("NotesView", new Uri("../Views/NotesDettesView.xaml"));
            navigationService.Configure("DetesView", new Uri("../Views/NotesExaminsView.xaml"));

            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }
        public static void Cleanup()
        {
        }
    }
}