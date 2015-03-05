using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
        private bool _langueInterfaceFr=true;
        private bool _langueInterfaceAr=false;
        private bool _langueContenuFr = true;
        private bool _langueContenuAr = false;
        #endregion
        #region Properties
        public bool LangueContenuFr
        {
            get
            {
                return _langueContenuFr;
            }

            set
            {
                if (_langueContenuFr == value)
                {
                    return;
                }

                _langueContenuFr = value;
                RaisePropertyChanged();
                Messenger.Default.Send<bool>(_langueContenuFr, "LangFr");
                
            }
        }
        public bool LangueContenuAr
        {
            get
            {
                return _langueContenuAr;
            }

            set
            {
                if (_langueContenuAr == value)
                {
                    return;
                }

                _langueContenuAr = value;
                RaisePropertyChanged();                                
            }
        }

        public bool LangueInterfaceFr
        {
            get
            {
                return _langueInterfaceFr;
            }

            set
            {
                if (_langueInterfaceFr == value)
                {
                    return;
                }

                _langueInterfaceFr = value;
                RaisePropertyChanged();
                if (_langueInterfaceFr)
                {
                    App.SelectCulture("Fr");
                }
            }
        }

        public bool LangueInterfaceAr
        {
            get
            {
                return _langueInterfaceAr;
            }

            set
            {
                if (_langueInterfaceAr == value)
                {
                    return;
                }

                _langueInterfaceAr = value;
                RaisePropertyChanged();
                if (_langueInterfaceAr)
                {
                    App.SelectCulture("Ar");                    
                }
            }
        }

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
                        () => _navigationService.NavigateTo("LoginView")));
            }
        }        
        #endregion
        #region Ctors and Methods
        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Messenger.Default.Register<ENSEIGNANT>(this, "Login", (ens) =>
            {
                LoggedInUser = ens;
                var context = new LocalDbEntities();
                ListSpeciliteEns = new ObservableCollection<SPECIALITE>(context.ENS_SPEMAT.Where(x => x.ID_ENSEIGNANT == LoggedInUser.ID_ENSEIGNANT).Select(x => x.SPECIALITE).Distinct().ToList());
            });
        }
        #endregion
    }
}