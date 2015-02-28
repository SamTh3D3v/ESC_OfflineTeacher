using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OfflineTeacher_DBProject;


namespace ESC_OfflineTeacher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Conts
        #endregion
        #region Fields
        private readonly IFrameNavigationService _navigationService;
        private bool _navigationSource;
        private ObservableCollection<SPECIALITE> _listSpeciliteEns;
        private ENSEIGNANT _loggedInUser  ;
      
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
        public ENSEIGNANT LoggedInUser
        {
            get
            {
                return _loggedInUser;
            }

            set
            {
                if (_loggedInUser == value)
                {
                    return;
                }

                _loggedInUser = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SPECIALITE> ListSpeciliteEns
        {
            get
            {
                return _listSpeciliteEns;
            }

            set
            {
                if (_listSpeciliteEns == value)
                {
                    return;
                }

                _listSpeciliteEns = value;
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
                            using (var context=new LocalDbEntities())
                            {
                                LoggedInUser = context.ENSEIGNANTS.First(x => x.ID_ENSEIGNANT == 2);
                                ListSpeciliteEns=new ObservableCollection<SPECIALITE>(context.ENS_SPEMAT.Where(x=>x.ID_ENSEIGNANT==LoggedInUser.ID_ENSEIGNANT).Select(x=>x.SPECIALITE).Distinct().ToList());
                            }
                         
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