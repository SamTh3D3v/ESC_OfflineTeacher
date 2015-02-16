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
        private ObservableCollection<String> _specialiteList;
        private ObservableCollection<String> _matiereList;
        private ObservableCollection<String> _semestreList;
        private ObservableCollection<String> _sectionList;
        private ObservableCollection<String> _groupeList;
        private ObservableCollection<String> _examinList;
        private String _nbEtudiants;
        private ObservableCollection<String> _rechercherParList;
        private String _searchText = "";
        private bool _sideBarIsOpen;
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
        public ObservableCollection<String> SpecialiteList
        {
            get
            {
                return _specialiteList;
            }

            set
            {
                if (_specialiteList == value)
                {
                    return;
                }

                _specialiteList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> MatiereList
        {
            get
            {
                return _matiereList;
            }

            set
            {
                if (_matiereList == value)
                {
                    return;
                }

                _matiereList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> SemestreList
        {
            get
            {
                return _semestreList;
            }

            set
            {
                if (_semestreList == value)
                {
                    return;
                }

                _semestreList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> SectionList
        {
            get
            {
                return _sectionList;
            }

            set
            {
                if (_sectionList == value)
                {
                    return;
                }

                _sectionList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> GroupeList
        {
            get
            {
                return _groupeList;
            }

            set
            {
                if (_groupeList == value)
                {
                    return;
                }

                _groupeList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> ExaminList
        {
            get
            {
                return _examinList;
            }

            set
            {
                if (_examinList == value)
                {
                    return;
                }

                _examinList = value;
                RaisePropertyChanged();
            }
        }
        public String NbEtdiants
        {
            get
            {
                return _nbEtudiants;
            }

            set
            {
                if (_nbEtudiants == value)
                {
                    return;
                }

                _nbEtudiants = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<String> RechercherParList
        {
            get
            {
                return _rechercherParList;
            }

            set
            {
                if (_rechercherParList == value)
                {
                    return;
                }

                _rechercherParList = value;
                RaisePropertyChanged();
            }
        }
        public String SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                if (_searchText == value)
                {
                    return;
                }

                _searchText = value;
                RaisePropertyChanged();
            }
        }               
        public bool SideBarIsOpen 
        {
            get
            {
                return _sideBarIsOpen;
            }

            set
            {
                if (_sideBarIsOpen == value)
                {
                    return;
                }

                _sideBarIsOpen = value;
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