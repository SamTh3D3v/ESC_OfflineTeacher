using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;


namespace ESC_OfflineTeacher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Conts
        #endregion
        #region Fields
        private readonly IFrameNavigationService _navigationService;
        private bool _navigationSource;
      
        #endregion
        #region Properties

        public IFrameNavigationService FrameNavigationService
        {
            get { return _navigationService; }
        }

        public bool NavigationSource
        {
            get
            {
                return _navigationSource;
            }

            set
            {
                if (_navigationSource == value)
                {
                    return;
                }

                _navigationSource = value;
                RaisePropertyChanged();
            }
        }
       
        #endregion
        #region Commands
        private RelayCommand _mainWindowLoadedCommand;
        public RelayCommand MainWindowLoadedCommand
        {
            get
            {
                return _mainWindowLoadedCommand
                    ?? (_mainWindowLoadedCommand = new RelayCommand(
                        () =>
                        {
                            _navigationService.NavigateTo("LoginView");                            
                        }));
            }
        }        
        #endregion
        #region Ctors and Methods
        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}