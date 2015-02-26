using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ESC_OfflineTeacher.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OfflineTeacher_DBProject;


namespace ESC_OfflineTeacher.ViewModel
{

    public class NoteViewModel : ViewModelBase
    {
        #region Consts

        #endregion
        #region Fields
        private readonly IFrameNavigationService _navigationService;
        private ObservableCollection<EtudiantNote> _listNotesExamins;
        private ObservableCollection<EtudiantNote> _listNotesDettes;
        // Create a BackgroundWorker object to synchronize without blocking
        // the UI thread
        private BackgroundWorker backgroundWorker1;
        private LocalDbEntities _context;

        private ObservableCollection<SPECIALITE> _specialiteList;
        private ObservableCollection<MATIERE> _matiereList;
        private ObservableCollection<MODES_ETUDES> _semestreList;
        private ObservableCollection<SECTION> _sectionList;
        private ObservableCollection<GROUPE> _groupeList;
        private ObservableCollection<EXAMEN> _examinList;
        private String _nbEtudiants = "";
        private string _currentYear = "";
        private ObservableCollection<String> _rechercherParList;
        private String _searchText = "";
        private bool _sideBarIsOpen;
        private ENSEIGNANT _loggedInTeacher;
        private SPECIALITE _selectedSpecialite;
        private MATIERE _selectedMatiere;
        private MODES_ETUDES _selectedSemester;
        private SECTION _selectedSection;
        private GROUPE _selectedGroupe;
        private EXAMEN _selectedExamen;


        private IEnumerable<GROUPE> _allGroupesList;
        private IEnumerable<SECTION> _allSectionsList;
        #endregion
        #region Properties
        public string CurrentYear
        {
            get
            {
                return _currentYear;
            }

            set
            {
                if (_currentYear == value)
                {
                    return;
                }

                _currentYear = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<EtudiantNote> ListNotesExamins
        {
            get
            {
                return _listNotesExamins;
            }

            set
            {
                if (_listNotesExamins == value)
                {
                    return;
                }

                _listNotesExamins = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<EtudiantNote> ListNotesDettes
        {
            get
            {
                return _listNotesDettes;
            }

            set
            {
                if (_listNotesDettes == value)
                {
                    return;
                }

                _listNotesDettes = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SPECIALITE> SpecialiteList
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
        public ObservableCollection<MATIERE> MatiereList
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
        public ObservableCollection<MODES_ETUDES> SemestreList
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
        public ObservableCollection<SECTION> SectionList
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
        public ObservableCollection<GROUPE> GroupeList
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
        public ObservableCollection<EXAMEN> ExaminList
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
        public ENSEIGNANT LoggedInTeacher
        {
            get
            {
                return _loggedInTeacher;
            }

            set
            {
                if (_loggedInTeacher == value)
                {
                    return;
                }

                _loggedInTeacher = value;
                RaisePropertyChanged();
            }
        }
        public SPECIALITE SelectedSpecialite
        {
            get
            {
                return _selectedSpecialite;
            }

            set
            {
                if (_selectedSpecialite == value)
                {
                    return;
                }

                _selectedSpecialite = value;
                RaisePropertyChanged();

                var cy = int.Parse(CurrentYear);
                if (_selectedSpecialite != null)
                {
                    var matiereInUserSpecialite = _context.USERS_SPECIALITES.Where(x => x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == LoggedInTeacher.ID_USER && x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE).Select(x => x.MATIERE).Distinct();
                    MatiereList = new ObservableCollection<MATIERE>(matiereInUserSpecialite.ToList());
                    var groupeList =
                   _context.USERS_SPECIALITES.Where(
                       x =>
                           x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == _loggedInTeacher.ID_USER &&
                           x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE).Select(x => x.GROUPE).Distinct().ToList();

                    SectionList = new ObservableCollection<SECTION>(groupeList.Select(x => x.SECTION).Distinct());
                }
                else
                {
                    MatiereList = new ObservableCollection<MATIERE>();
                    SectionList = new ObservableCollection<SECTION>();
                    GroupeList = new ObservableCollection<GROUPE>();
                    ListNotesExamins = new ObservableCollection<EtudiantNote>();
                }

                //set the semesters and the needed exam for this year 
                var modeEtudes =
                    _context.EXAMENS_ANNEES_MODES_ETUDES.Where(
                        x => x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE &&
                             x.ANNEE_UNIVERSITAIRE == cy).ToList();
                SemestreList = new ObservableCollection<MODES_ETUDES>(modeEtudes.Select(x => x.MODES_ETUDES).Distinct().ToList());
                RefreshNoteStudentList();

            }
        }
        public MATIERE SelectedMatiere
        {
            get
            {
                return _selectedMatiere;
            }

            set
            {
                if (_selectedMatiere == value)
                {
                    return;
                }

                _selectedMatiere = value;
                RaisePropertyChanged();
                RefreshNoteStudentList();
            }
        }
        public MODES_ETUDES SelectedSemester
        {
            get
            {
                return _selectedSemester;
            }

            set
            {
                if (_selectedSemester == value)
                {
                    return;
                }

                _selectedSemester = value;
                RaisePropertyChanged();
                var cy = int.Parse(CurrentYear);
                var modeEtudes =
                    _context.EXAMENS_ANNEES_MODES_ETUDES.Where(x => x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE &&
                             x.ANNEE_UNIVERSITAIRE == cy && x.ID_MODE_ETUDE == _selectedSemester.ID_MODE_ETUDE).ToList();
                ExaminList = new ObservableCollection<EXAMEN>(modeEtudes.Select(x => x.EXAMEN).ToList());
                RefreshNoteStudentList();
            }
        }
        public SECTION SelectedSection
        {
            get
            {
                return _selectedSection;
            }

            set
            {
                if (_selectedSection == value)
                {
                    return;
                }

                _selectedSection = value;
                RaisePropertyChanged();
                if (_selectedSection != null)
                {
                    var cy = int.Parse(CurrentYear);
                    GroupeList =
                        new ObservableCollection<GROUPE>(_context.USERS_SPECIALITES.Where(
                            x =>
                                x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == _loggedInTeacher.ID_USER &&
                                x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE).Select(x => x.GROUPE).Distinct());
                    RefreshNoteStudentList();
                }
                else
                {
                    GroupeList = new ObservableCollection<GROUPE>();
                    ListNotesExamins = new ObservableCollection<EtudiantNote>();
                }


            }
        }
        public GROUPE SelectedGroupe
        {
            get
            {
                return _selectedGroupe;
            }

            set
            {
                if (_selectedGroupe == value)
                {
                    return;
                }

                _selectedGroupe = value;
                RaisePropertyChanged();
                RefreshNoteStudentList();
            }
        }
        public EXAMEN SelectedExamin
        {
            get
            {
                return _selectedExamen;
            }

            set
            {
                if (_selectedExamen == value)
                {
                    return;
                }

                _selectedExamen = value;
                RaisePropertyChanged();
                RefreshNoteStudentList();
            }
        }
        #endregion
        #region Commands
        private RelayCommand _syncCommand;
        public RelayCommand SyncCommand
        {
            get
            {
                return _syncCommand
                    ?? (_syncCommand = new RelayCommand(
                    () =>
                    {
                        // Check if sync is already in progress
                        //          if (!backgroundWorker1.IsBusy)
                        //          {

                        //              Debug.WriteLine(
                        //"Starting Data Synchronization Process...\r\n Please wait till the process compeletes.\r\n");
                        //              Application.Current.MainWindow.Cursor = Cursors.Wait;

                        //              SynchronizationHelper syncHelper = new SynchronizationHelper();
                        //              // Start synchronization
                        //              backgroundWorker1.RunWorkerAsync(syncHelper);
                        //          }
                        //using (LocalSGSDBEntities context = new LocalSGSDBEntities())
                        //{
                        //    CurrentYear = context.ANNEES.Max(x => x.ANNEE_UNIVERSITAIRE).ToString(CultureInfo.InvariantCulture);

                        //}

                    }));
            }
        }
        private RelayCommand _noteViewLoadedCommand;
        public RelayCommand NoteViewLoadedCommand
        {
            get
            {
                return _noteViewLoadedCommand
                    ?? (_noteViewLoadedCommand = new RelayCommand(
                    () =>
                    {
                        //the loggin is using a wcf service where the returned data is of USER type
                        LoggedInTeacher = _context.ENSEIGNANTS.First(x => x.ID_ENSEIGNANT == 2);
                        CurrentYear = _context.ANNEES.Max(x => x.ANNEE_UNIVERSITAIRE).ToString(CultureInfo.InvariantCulture);

                        var cy = int.Parse(CurrentYear);
                        var userSpecialite = _context.USERS_SPECIALITES.Where(x => x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == LoggedInTeacher.ID_USER).ToList();
                        SpecialiteList = new ObservableCollection<SPECIALITE>(userSpecialite.Select(x => x.SPECIALITE).Distinct().ToList());

                    }));
            }
        }
        #endregion
        #region Ctors and Methods
        public NoteViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _context = new LocalDbEntities();
            //this.backgroundWorker1 =new BackgroundWorker();
            //this.backgroundWorker1.WorkerReportsProgress = true;
            //// Register the various BackgroundWorker events
            //this.backgroundWorker1.DoWork+= new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            //this.backgroundWorker1.ProgressChanged+= new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            //this.backgroundWorker1.RunWorkerCompleted+= new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

            //ESCLocalDbSyncAgent
            //    LocalSGSDBEntities dEntities=new LocalSGSDBEntities();

            ListNotesExamins = new ObservableCollection<EtudiantNote>();
            //GenerateFakeData();  //For Test purpuses

        }
        // Method to start syncthonization in background
        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker;
        //    worker = (BackgroundWorker)sender;
        //    SynchronizationHelper syncHelper = (SynchronizationHelper)e.Argument;
        //    syncHelper.SynchronizeAsync(worker, e);

        //}

        ////Method to report synchronization progress
        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    SyncResults results = (SyncResults)e.UserState;
        //    if (results != null)
        //    {
        //        DisplayStats(results);
        //    }
        //}

        //Method runs when synchtonization compeletes
        private void backgroundWorker1_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
            if (e.Error != null)
            {
                Debug.WriteLine(
     "An Error has occurred. Please try synchronization later.\r\nThe error:" + e.Error.Message);
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                Debug.WriteLine("Synchronization Finished Successfully\r\n");
                MessageBox.Show("Finished Synchronization");
            }
        }


        // Method to format the SyncResults for display
        //        private void DisplayStats(SyncResults results)
        //        {

        //            TimeSpan diff =
        //                results.Stats.SyncEndTime.Subtract(results.Stats.SyncStartTime);

        //            Debug.WriteLine(
        //                string.Format(
        //"{4}:  - Total Time To Synchronize = {0}:{1}:{2}:{3}\r\nTotal Records Uploaded: {5}  Total Records Downloaded: {6}\r\n",
        //               diff.Hours, diff.Minutes, diff.Seconds,
        //               diff.Milliseconds, results.Message,
        //               results.Stats.UploadChangesTotal,
        //               results.Stats.DownloadChangesTotal));
        //        }

        private void GenerateFakeData()
        {
            #region fake Data
            ListNotesExamins = new ObservableCollection<EtudiantNote>()
            {
                new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                }, new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                },
                 new EtudiantNote()
                {
                    Matricule = "070014",
                    Nom="Meriem",
                    Prenom ="Alichaoui"                     
                }
            };
            ListNotesDettes = new ObservableCollection<EtudiantNote>(ListNotesExamins);
            #endregion
        }

        private void RefreshNoteStudentList()
        {
            if (SelectedMatiere != null && SelectedExamin != null)
            {
                var cy = int.Parse(CurrentYear);
                ListNotesExamins =
                    new ObservableCollection<EtudiantNote>(_context.NOTES_EXAMEN.Where(
                        x => x.ANNEE_UNIVERSITAIRE == cy &&
                             x.ID_MATIERE == SelectedMatiere.ID_MATIERE &&
                             x.ID_EXAMEN == SelectedExamin.ID_EXAMEN).Select(x => new EtudiantNote()
                             {
                                 Nom = x.ETUDIANT.NOM,
                                 Prenom = x.ETUDIANT.PRENOM,
                                 NoteExamin = x.NOTE
                             }));

                //ListNotesExamins = new ObservableCollection<EtudiantNote>(_context.NOTES_EXAMEN.Where(x => x.ANNEE_UNIVERSITAIRE == cy &&
                //                                 x.ID_MATIERE == SelectedMatiere.ID_MATIERE &&
                //                                 x.ID_EXAMEN == SelectedExamin.ID_EXAMEN)
                //    .Join(_context.ETUDIANTS, noteExamin => noteExamin.ID_ETUDIANT,
                //        etudiant => etudiant.ID_ETUDIANT, (noteExamin, etudiant) => new EtudiantNote()
                //        {
                //            Matricule = etudiant.MATRICULE,
                //            Nom = etudiant.NOM,
                //            Prenom = etudiant.PRENOM,
                //            NoteExamin = noteExamin.NOTE
                //        }).ToList());
            }
        }
        #endregion


    }

}