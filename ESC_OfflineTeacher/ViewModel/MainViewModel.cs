using GalaSoft.MvvmLight;


namespace ESC_OfflineTeacher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Conts       
        #endregion
        #region Fields
        private bool _navigationSource;
        #endregion
        #region Properties               
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
        #region Ctors and Methods
        public MainViewModel()
        {

        }  
        #endregion            
    }
}