using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ESC_OfflineTeacher.Model;
using ESC_OfflineTeacher.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using OfflineTeacher_DBProject;

namespace ESC_OfflineTeacher.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {     
        #region Fields        
        private readonly IFrameNavigationService _navigationService;
        private String _userName = "";
        private PasswordBox _passwordBox;
        private bool _langContentFr = true;
        private SolidColorBrush _globaleThemeBrush;
        private bool _progressRingIsActive = false;
        private BackgroundWorker _loginBackgroundWorker;
        private bool _langInterfaceFr = true;
        private RelayCommand<Object> _loginCommand;
        private RelayCommand _importDataBaseCommand;
        #endregion
        #region Properties
        public SolidColorBrush GlobalThemeBrush
        {
            get
            {
                return _globaleThemeBrush;
            }

            set
            {


                _globaleThemeBrush = value;
                RaisePropertyChanged();
            }
        }
        public bool LangContentFr
        {
            get
            {
                return _langContentFr;
            }

            set
            {
                if (_langContentFr == value)
                {
                    return;
                }

                _langContentFr = value;
                RaisePropertyChanged();
            }
        }
        public bool LangInterfaceFr
        {
            get
            {
                return _langInterfaceFr;
            }

            set
            {
                if (_langInterfaceFr == value)
                {
                    return;
                }

                _langInterfaceFr = value;
                RaisePropertyChanged();
            }
        }
        public String UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if (_userName == value)
                {
                    return;
                }

                _userName = value;
                RaisePropertyChanged();
            }
        }
        public bool ProgressRingIsActive
        {
            get
            {
                return _progressRingIsActive;
            }

            set
            {
                if (_progressRingIsActive == value)
                {
                    return;
                }

                _progressRingIsActive = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region Commands        
        public RelayCommand<Object> LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommand<Object>(
                    (p) =>
                    {                                                
                        if (!File.Exists(App.LocalDbPath))
                        {
                            var _contoller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Problème d'accés a la base de données local", "La base de données local n'exite pas ou elle est corrompu, importer une nouvelle base de données"));
                            return;
                        }
                        _passwordBox = p as PasswordBox;
                        if (_passwordBox != null && !_loginBackgroundWorker.IsBusy)
                             _loginBackgroundWorker.RunWorkerAsync();
                            
                    }));
            }
        }        
        public RelayCommand ImportDataBaseCommand
        {
            get
            {
                return _importDataBaseCommand
                    ?? (_importDataBaseCommand = new RelayCommand(async () =>
                    {
                        var dlg = new OpenFileDialog
                        {
                            DefaultExt = ".sdf",
                            Filter = "SqlCE database (.sdf)|*.sdf",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        };
                        if (dlg.ShowDialog() == true)
                        {
                            try
                            {
                                string fileName = dlg.FileName;                                                            
                                Settings.Default.HashValue= ComputeHash(fileName, App.LocalDbPath);
                                Settings.Default.Save();
                            }
                            catch (Exception e)
                            {
                                var _controller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("impossible d'importer le fichier", "le fichier que vous voulez importer est interrompu ... "));                                
                            }
                        }
                    }));
            }
        }
        #endregion
        #region Ctors and Methods
        public LoginViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;            
            InitializeSyncBackgroundWorker();
            Messenger.Default.Register<bool>(this, "LangFr", (fr) =>
            {
                LangContentFr = fr;
            });
            Messenger.Default.Register<bool>(this, "LangFrInterface", (fr) =>
            {
                LangInterfaceFr = fr;
            });
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "IsDark":
                        GlobalThemeBrush = App.Dark;
                        break;
                    case "IsLight":
                        GlobalThemeBrush = App.Light;
                        break;
                }
            });
            GlobalThemeBrush = App.Dark;                      

        }

        private void InitializeSyncBackgroundWorker()
        {
            _loginBackgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            _loginBackgroundWorker.DoWork += new DoWorkEventHandler(_loginBackgroundWorker_DoWork);
            _loginBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_LoginBackgroundWorker_RunWorkerCompleted);
            //_loginBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_loginBackgroundWorker_ProgressChanged);
        }
        private void _LoginBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressRingIsActive = false;
        }

        private void _loginBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgressRingIsActive = true;
            LogIn(UserName, _passwordBox.Password);            
        }

        private async void LogIn(string userName, string password)
        {            
            ENSEIGNANT loggedInEns = null;
            var client = new SGS.AuthenticationServiceClient();
            var clientHelper = new SGS.Helper.OfflineServiceClient();
            //first check whether the teacher has already logged In at least Once 
            var db = new LocalDbEntities();                       
            int idUser;            
            if (db.UserPreferences.Any(u => u.UserName == userName))
            {                
                try
                {
                    idUser = db.UserPreferences.First(u => u.UserName == userName).IdUser;
                    loggedInEns = db.ENSEIGNANTS.FirstOrDefault(x => x.ID_USER == idUser);
                    if (loggedInEns==null)
                    {
                       loggedInEns=new ENSEIGNANT()
                       {
                           NOM = UserName,
                           ID_USER = idUser,
                           NOM_LATIN = UserName
                       }; 
                    }
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _navigationService.NavigateTo("Notes", new UserPreferences
                        {
                            Enseignant = loggedInEns,
                            LangContFr = LangContentFr,
                            UserName = userName,
                            IdUser = idUser,

                        });
                        Messenger.Default.Send<ENSEIGNANT>(loggedInEns, "Login");
                    }));
                }
                catch (Exception ex)
                {
                    var _controller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Une erreur interne", "un porblème d'accés a la base de données locale ... "));                    
                }

            }
            else
            {
                //use the service to authentificate and update the userPref Local Table when successeeded
                try
                {                    
                    if (!client.Login(userName, password, null, true))
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                        {                            
                            await
                                ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync(
                                    "Echec de connection", "Mot de passe ou nom d'utilisateur erroné ... "));
                        }));
                        return;
                    }
                    idUser = (int)clientHelper.GetUserId(userName);

                  
                    
                    //the first use of the application --> Force the sync 
                    try
                    {
                        db.UserPreferences.AddObject(new UserPreference()
                                    {
                                        UserName = userName,
                                        IdUser = idUser
                                    });
                        db.SaveChanges();
                        db.Dispose();
                    }
                    catch (Exception ex)
                    {
                        var _controller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Une erreur interne", "un porblème d'accés a la base de données locale ... "));                        
                    }                  
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => _navigationService.NavigateTo("Notes", new UserPreferences
                    {
                        Enseignant = null,
                        LangContFr = LangContentFr,
                        UserName = userName,
                        IdUser = idUser,
                    })));
                }
                catch (EndpointNotFoundException ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        var ctontroller = await ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync(
                            "Echec de connection", "Le Serveur est injoignable ... "));
                    }));
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        var ctontroller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync(
                            "Echec de connection", "Un probleme avec le service d'authentification ... "));
                    }));
                }

            }
        }
        public string ComputeHash(string fileName, string exportTo = null)
        {
            using (var md5 = MD5.Create())
            {

                string to = exportTo ?? "\\res.sdf";
                File.Copy(fileName, to, true);
                string res = "";
                using (var stream = File.OpenRead(to))
                {
                    res = BitConverter.ToString(md5.ComputeHash(stream));
                }
                if (exportTo == null)
                {
                    File.Delete(to);
                }
                return res;
            }
        }
       
        #endregion
    }
}