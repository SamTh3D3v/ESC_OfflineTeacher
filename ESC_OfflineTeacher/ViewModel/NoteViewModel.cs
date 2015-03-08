using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ESC_OfflineTeacher.Model;
using ESC_OfflineTeacher.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServerCe;
using Microsoft.Win32;
using OfflineTeacher_DBProject;


namespace ESC_OfflineTeacher.ViewModel
{

    public class NoteViewModel : ViewModelBase
    {   
        #region Fields
        private readonly IFrameNavigationService _navigationService;
        private ObservableCollection<EtudiantNote> _listNotesExamins;
        private ObservableCollection<EtudiantNoteDette> _listNotesDettes;
        private bool _progressRingIsActive = false;
        private ObservableCollection<EtudiantNote> _listNotesExaminsForSearch;
        private ObservableCollection<EtudiantNoteDette> _listNotesDettesForSearch;
        private MessageDialogResult _controller;             
        private LocalDbEntities _context;
        private BackgroundWorker _syncBackgroundWorker;
        private ObservableCollection<SPECIALITE> _specialiteList;
        private ObservableCollection<MATIERE> _matiereList;
        private ObservableCollection<MODES_ETUDES> _semestreList;
        private ObservableCollection<SECTION> _sectionList;
        private ObservableCollection<GROUPE> _groupeList;
        private ObservableCollection<EXAMEN> _examinList;
        private String _nbEtudiants = "";
        private string _currentYear = "";
        private ObservableCollection<SearchItem> _rechercherParList;
        private String _searchText = "";
        private String _searchTextDette = "";
        private bool _sideBarIsOpen;
        private ENSEIGNANT _loggedInTeacher;
        private SPECIALITE _selectedSpecialite;
        private MATIERE _selectedMatiere;
        private MODES_ETUDES _selectedSemester;
        private SECTION _selectedSection;
        private GROUPE _selectedGroupe;
        private EXAMEN _selectedExamen;
        private bool _afficherTousExamin = false;
        private Visibility _noteDetteColVisibility;
        private Visibility _rattrapageDetteColVisibility;
        private IEnumerable<GROUPE> _allGroupesList;
        private IEnumerable<SECTION> _allSectionsList;
        private ObservableCollection<ExaminDette> _listExaminDette;
        private int _pbValue;
        private Visibility _pbVisibility = Visibility.Collapsed;
        private ExaminDette _selectedExaminDette;
        private bool _langContentFr=true;
        private SolidColorBrush _globaleThemeBrush;
        private bool _langInterfaceFr = true;
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
        public ObservableCollection<EtudiantNoteDette> ListNotesDettes
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
                SelectedSpecialite = _specialiteList.First();
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
                SelectedMatiere = _matiereList.FirstOrDefault();
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
                SelectedSemester = _semestreList.FirstOrDefault();
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
                SelectedSection = _sectionList.FirstOrDefault();
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
                SelectedGroupe = _groupeList.FirstOrDefault();
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
                SelectedExamin = _examinList.FirstOrDefault();
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
        public ObservableCollection<SearchItem> RechercherParList
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


                var seachText = _searchText.ToLower();
                if (seachText != "")
                {
                    var matricule = RechercherParList[0].IsSelected;
                    var nom = RechercherParList[1].IsSelected;
                    var prenom = RechercherParList[2].IsSelected;
                    if (LangContentFr)
                    {
                        ListNotesExamins = new ObservableCollection<EtudiantNote>(_listNotesExaminsForSearch.Where(x =>
                          x.Matricule.ToLower().Contains(seachText) && matricule
                       || x.NomLatin.ToLower().Contains(seachText) && nom
                       || x.PrenomLatin.ToLower().Contains(seachText) && prenom));
                    }
                    else
                    {
                        ListNotesExamins = new ObservableCollection<EtudiantNote>(_listNotesExaminsForSearch.Where(x =>
                          x.Matricule.ToLower().Contains(seachText) && matricule
                       || x.Nom.ToLower().Contains(seachText) && nom
                       || x.Prenom.ToLower().Contains(seachText) && prenom));
                    }
                   
                }
                else
                {
                    RefreshNoteStudentList();
                }
            }
        }
        public String SearchTextDette
        {
            get
            {
                return _searchTextDette;
            }

            set
            {
                if (_searchTextDette == value)
                {
                    return;
                }

                _searchTextDette = value;
                RaisePropertyChanged();


                var seachTextDette = _searchTextDette.ToLower();
                if (seachTextDette != "")
                {
                    var matricule = RechercherParList[0].IsSelected;
                    var nom = RechercherParList[1].IsSelected;
                    var prenom = RechercherParList[2].IsSelected;
                    if (LangContentFr)
                    {
                        ListNotesDettes = new ObservableCollection<EtudiantNoteDette>(_listNotesDettesForSearch.Where(x =>
                            x.Matricule.ToLower().Contains(seachTextDette) && matricule
                         || x.NomLatin.ToLower().Contains(seachTextDette) && nom
                         || x.PrenomLatin.ToLower().Contains(seachTextDette) && prenom)); 
                    }
                    else
                    {
                        ListNotesDettes = new ObservableCollection<EtudiantNoteDette>(_listNotesDettesForSearch.Where(x =>
                           x.Matricule.ToLower().Contains(seachTextDette) && matricule
                        || x.Nom.ToLower().Contains(seachTextDette) && nom
                        || x.Prenom.ToLower().Contains(seachTextDette) && prenom));
                    }
                    
                }
                else
                {
                    RefreshNoteDetteStudentList();
                }
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
                if (_selectedSpecialite == value )
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
                    SelectedMatiere = MatiereList.FirstOrDefault();
                    var modeEtudes =
                    _context.EXAMENS_ANNEES_MODES_ETUDES.Where(
                        x => x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE &&
                             x.ANNEE_UNIVERSITAIRE == cy).ToList();
                    SemestreList = new ObservableCollection<MODES_ETUDES>(modeEtudes.Select(x => x.MODES_ETUDES).Distinct().ToList());
                    RefreshNoteStudentList();
                    RefreshNoteDetteStudentList();
                    
                }
                else
                {
                    MatiereList = new ObservableCollection<MATIERE>();
                    SectionList = new ObservableCollection<SECTION>();
                    GroupeList = new ObservableCollection<GROUPE>();
                    ListNotesExamins = new ObservableCollection<EtudiantNote>();
                    ListNotesDettes = new ObservableCollection<EtudiantNoteDette>();
                }
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
                if (_selectedMatiere!=null)
                {
                    var cy = int.Parse(CurrentYear);
                    var groupeList =
                   _context.USERS_SPECIALITES.Where(
                       x =>
                           x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == _loggedInTeacher.ID_USER &&
                           x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE && x.ID_MATIERE == SelectedMatiere.ID_MATIERE).Select(x => x.GROUPE).Distinct().ToList();
                    SectionList = new ObservableCollection<SECTION>(groupeList.Select(x => x.SECTION).Distinct());
                }
                RefreshNoteStudentList();
                RefreshNoteDetteStudentList();
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
                if (_selectedSemester!=null && _selectedSpecialite!=null)
                {
                    var cy = int.Parse(CurrentYear);
                    var modeEtudes =
                        _context.EXAMENS_ANNEES_MODES_ETUDES.Where(x => x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE &&
                                 x.ANNEE_UNIVERSITAIRE == cy && x.ID_MODE_ETUDE == _selectedSemester.ID_MODE_ETUDE).ToList();
                    ExaminList = new ObservableCollection<EXAMEN>(modeEtudes.Select(x => x.EXAMEN).ToList());
                    RefreshNoteStudentList();
                    RefreshNoteDetteStudentList(); 
                }
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
                                x.ID_SPECIALITE == _selectedSpecialite.ID_SPECIALITE && x.ID_MATIERE==_selectedMatiere.ID_MATIERE).Select(x => x.GROUPE).Where(g=>g.ID_SECTION==_selectedSection.ID_SECTION).Distinct());
                    RefreshNoteStudentList();
                    RefreshNoteDetteStudentList();
                }
                else
                {
                    GroupeList = new ObservableCollection<GROUPE>();
                    ListNotesExamins = new ObservableCollection<EtudiantNote>();
                    ListNotesDettes = new ObservableCollection<EtudiantNoteDette>();
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
                RefreshNoteDetteStudentList();
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
                RefreshNoteDetteStudentList();
            }
        }
        public ExaminDette SelectedExaminDette
        {
            get
            {
                return _selectedExaminDette;
            }

            set
            {
                _selectedExaminDette = value;
                RaisePropertyChanged();
                if (_selectedExaminDette==null)
                return;
                if (_selectedExaminDette.DESIGNATION_LATIN == ListExaminDette[0].DESIGNATION_LATIN && !AfficherTousExamin)
                {
                    NoteDetteColVisibility = Visibility.Visible;
                    RattrapageDetteColVisibility = Visibility.Collapsed;
                }
                else if (_selectedExaminDette.DESIGNATION_LATIN == ListExaminDette[1].DESIGNATION_LATIN && !AfficherTousExamin)
                {
                    NoteDetteColVisibility = Visibility.Collapsed;
                    RattrapageDetteColVisibility = Visibility.Visible;
                }

            }
        }
        public ObservableCollection<ExaminDette> ListExaminDette
        {
            get
            {
                return _listExaminDette;
            }

            set
            {
                if (_listExaminDette == value)
                {
                    return;
                }

                _listExaminDette = value;
                RaisePropertyChanged();
                SelectedExaminDette = _listExaminDette.FirstOrDefault();
            }
        }
        public int PbValue
        {
            get
            {
                return _pbValue;
            }

            set
            {
                if (_pbValue == value)
                {
                    return;
                }

                _pbValue = value;
                RaisePropertyChanged();
            }
        }
        public Visibility PbVisibility
        {
            get
            {
                return _pbVisibility;
            }

            set
            {
                if (_pbVisibility == value)
                {
                    return;
                }

                _pbVisibility = value;
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
        public bool AfficherTousExamin
        {
            get
            {
                return _afficherTousExamin;
            }

            set
            {
                if (_afficherTousExamin == value)
                {
                    return;
                }

                _afficherTousExamin = value;
                RaisePropertyChanged();
                if (_afficherTousExamin)
                {
                    NoteDetteColVisibility = Visibility.Visible;
                    RattrapageDetteColVisibility = Visibility.Visible;
                }
                else
                {
                    SelectedExaminDette = SelectedExaminDette;
                }

            }
        }
        public Visibility NoteDetteColVisibility
        {
            get
            {
                return _noteDetteColVisibility;
            }

            set
            {
                if (_noteDetteColVisibility == value)
                {
                    return;
                }

                _noteDetteColVisibility = value;
                RaisePropertyChanged();
            }
        }
        public Visibility RattrapageDetteColVisibility
        {
            get
            {
                return _rattrapageDetteColVisibility;
            }

            set
            {
                if (_rattrapageDetteColVisibility == value)
                {
                    return;
                }

                _rattrapageDetteColVisibility = value;
                RaisePropertyChanged();
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
                    ?? (_syncCommand = new RelayCommand(async () =>
                    {                        
                        //if ((string)Settings.Default["HashValue"] == "")
                        //{
                        //    Settings.Default["HashValue"] = ComputeHash(_localDbPath);
                        //    Settings.Default.Save();
                        //}
                        //else if (!ComputeHash(_localDbPath).Equals(Settings.Default["HashValue"]))
                        //{
                        //    //the database has been modified outside of the application 
                        //    _controller = await ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("impossible de synchronizer", "la base de donné a été modifié en dehors de l'application... "));
                        //    return;
                        //}
                        Save();
                        if (!_syncBackgroundWorker.IsBusy)
                            _syncBackgroundWorker.RunWorkerAsync();

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
                        if (LoggedInTeacher!=null)
                        {
                            CurrentYear = _context.ANNEES.Max(x => x.ANNEE_UNIVERSITAIRE).ToString(CultureInfo.InvariantCulture);
                            var cy = int.Parse(CurrentYear);
                            var userSpecialite = _context.USERS_SPECIALITES.Where(x => x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == LoggedInTeacher.ID_USER).ToList();
                            SpecialiteList = new ObservableCollection<SPECIALITE>(userSpecialite.Select(x => x.SPECIALITE).Distinct().ToList()); 
                        }                      
                    }));
            }
        }
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(async () => Save()));
            }
        }

        private async void Save()
        {
            //if ((string)Settings.Default["HashValue"] == "")
            //{
            //    Settings.Default["HashValue"] = ComputeHash(_localDbPath);
            //    Settings.Default.Save();
            //}
            //else if(!ComputeHash(_localDbPath).Equals(Settings.Default["HashValue"]))
            //{
            //    //the database has been modified outside of the application 
            //    _controller = await ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("impossible d'enregistrer", "la base de donné a été modifié en dehors de l'application... "));
            //    return;
            //}
            var cy = int.Parse(CurrentYear);
            foreach (var etudiantNote in ListNotesExamins)
            {
                var oldNote =
                    _context.NOTES_EXAMEN.First(
                        x => x.ID_ETUDIANT == etudiantNote.IdEtudiant && x.ANNEE_UNIVERSITAIRE == cy &&
                             x.ID_MATIERE == SelectedMatiere.ID_MATIERE &&
                             x.ID_EXAMEN == _selectedExamen.ID_EXAMEN).NOTE;
                if (oldNote != etudiantNote.Note)
                {
                    LOG_SaisieNotes(etudiantNote.IdEtudiant, SelectedMatiere.ID_MATIERE, oldNote, etudiantNote.Note, false);
                    _context.NOTES_EXAMEN.First(
                        x => x.ID_ETUDIANT == etudiantNote.IdEtudiant && x.ANNEE_UNIVERSITAIRE == cy &&
                             x.ID_MATIERE == SelectedMatiere.ID_MATIERE &&
                             x.ID_EXAMEN == _selectedExamen.ID_EXAMEN).NOTE = etudiantNote.Note;
                }
            }
            foreach (var etudiantNoteDette in ListNotesDettes)
            {
                var old = _context.NOTE_DETTE.First(x => x.ID_ETUDIANT == etudiantNoteDette.IdEtudiant && x.ANNEE_PASSAGE_DETTE == cy &&
                                                       x.ID_MATIERE == SelectedMatiere.ID_MATIERE);
                var oldNote = old.NOTE;
                var oldNoteRattrapage = old.NOTE_RATTRAPAGE;
                if (oldNote != etudiantNoteDette.Note || oldNoteRattrapage != etudiantNoteDette.NoteRattrapage)
                {
                    LOG_SaisieNotes(etudiantNoteDette.IdEtudiant, SelectedMatiere.ID_MATIERE, oldNote, etudiantNoteDette.Note, true, oldNoteRattrapage, etudiantNoteDette.NoteRattrapage);
                    _context.NOTE_DETTE.First(x => x.ID_ETUDIANT == etudiantNoteDette.IdEtudiant && x.ANNEE_PASSAGE_DETTE == cy &&
                                                       x.ID_MATIERE == SelectedMatiere.ID_MATIERE).NOTE = etudiantNoteDette.Note;
                    _context.NOTE_DETTE.First(x => x.ID_ETUDIANT == etudiantNoteDette.IdEtudiant && x.ANNEE_PASSAGE_DETTE == cy &&
                                                      x.ID_MATIERE == SelectedMatiere.ID_MATIERE).NOTE_RATTRAPAGE = etudiantNoteDette.NoteRattrapage;
                }
            }
            _context.SaveChanges();
            var hashValue = ComputeHash(App.LocalDbPath);
            Settings.Default["HashValue"] = hashValue;
            Settings.Default.Save();
        }
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                    ?? (_cancelCommand = new RelayCommand(
                    () =>
                    {
                        RefreshNoteStudentList();
                        RefreshNoteDetteStudentList();
                    }));
            }
        }
        private RelayCommand _importDataBaseCommand;
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
                                //var hashReader=new StreamReader(fileName+".hash");
                                //var hash = ComputeHash(fileName);
                                //if (!hash.Equals(hashReader.ReadLine().Trim()))
                                //{
                                //    _controller = await ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("impossible d'importer le fichier", "le fichier que vous voulez importer est interrompu ... "));
                                //    return;
                                //}
                                Settings.Default["HashValue"] = ComputeHash(fileName, App.LocalDbPath);
                                Settings.Default.Save();
                                _context.Dispose();
                                _context=new LocalDbEntities();
                                RefreshNoteStudentList();
                                RefreshNoteDetteStudentList();

                            }
                            catch (Exception e)
                            {
                                var _controller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Erreur d'importation ", "le fichier que vous voulez importer est interrompu ... "));                                
                            }
                        }
                        
                    }));
            }
        }
        private RelayCommand _exportDataBaseCommand;
        public RelayCommand ExportDataBaseCommand
        {
            get
            {
                return _exportDataBaseCommand
                    ?? (_exportDataBaseCommand = new RelayCommand(async () =>
                    {
                        //export the database and the hash file
                        var dlg = new SaveFileDialog
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
                                var hash=ComputeHash(App.LocalDbPath,fileName);
                                var strWritter = new StreamWriter(fileName + ".hash");
                                await strWritter.WriteLineAsync(hash);                                
                                strWritter.Close();

                            }
                            catch (Exception e)
                            {
                                var _controller = ((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Erreur d'éxportation ", "probleme d'éxportation du base de données ... "));                                
                            }
                        }                     
                    }));
            }
        }
        #endregion
        #region Ctors and Methods
        public NoteViewModel(IFrameNavigationService navigationService)
        {                       
            _navigationService = navigationService;            
            _context = new LocalDbEntities();            
            Initialisation();
            Messenger.Default.Register<bool>(this,"LangFr", (fr) =>
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
            
        }
        private void Initialisation()
        {
            
            InitializeSyncBackgroundWorker();          
            ListNotesExamins = new ObservableCollection<EtudiantNote>();
            ListNotesDettes = new ObservableCollection<EtudiantNoteDette>();
            ListExaminDette = new ObservableCollection<ExaminDette>()
            {
                new ExaminDette()
                {
                    DESIGNATION = "النقطة" ,
                    DESIGNATION_LATIN = "Note"
                },
                new ExaminDette()
                {
                    DESIGNATION = "الاستدراك",
                    DESIGNATION_LATIN = "Rattrapage"
                }
 
            };
            RechercherParList = new ObservableCollection<SearchItem>()
            {
                new SearchItem()
                {
                    Name = "Matricule",
                    IsSelected = true
                },
                 new SearchItem()
                {
                    Name = "Nom",
                    IsSelected = true
                },
                 new SearchItem()
                {
                    Name = "Prenom",
                    IsSelected = true
                }               
            };
            if (_navigationService.Parameter != null)
            {
                LoggedInTeacher = ((UserPreferences)_navigationService.Parameter).Enseignant;
                //if it is the first sync, the above object is null .. -> force resync then update it 
                if (LoggedInTeacher == null)
                {
                    _syncBackgroundWorker.RunWorkerAsync();                   
                }
                LangContentFr = ((UserPreferences)_navigationService.Parameter).LangContFr;
            }
            GlobalThemeBrush = App.Dark;
            AfficherTousExamin = true;
        }
        private void InitializeSyncBackgroundWorker()
        {
            _syncBackgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            _syncBackgroundWorker.DoWork += new DoWorkEventHandler(_syncBackgroundWorker_DoWork);
            _syncBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_syncBackgroundWorker_RunWorkerCompleted);
           // _syncBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_syncBackgroundWorker_ProgressChanged);
        }       
        private async void _syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            PbVisibility = Visibility.Collapsed;
            ProgressRingIsActive = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.MainWindow.Cursor = Cursors.Arrow;
            }));                                  
            Settings.Default["HashValue"] = ComputeHash(App.LocalDbPath);
            Settings.Default.Save();
            _context.Dispose();
            _context=new LocalDbEntities();           
            LoggedInTeacher =_context.ENSEIGNANTS.First(
                                y => y.ID_USER == ((UserPreferences)_navigationService.Parameter).IdUser);
            Messenger.Default.Send<ENSEIGNANT>(LoggedInTeacher, "Login");
            CurrentYear = _context.ANNEES.Max(x => x.ANNEE_UNIVERSITAIRE).ToString(CultureInfo.InvariantCulture);
            var cy = int.Parse(CurrentYear);
            var userSpecialite = _context.USERS_SPECIALITES.Where(x => x.ANNEE_UNIVERSITAIRE == cy && x.ID_USER == LoggedInTeacher.ID_USER).ToList();
            SpecialiteList = new ObservableCollection<SPECIALITE>(userSpecialite.Select(x => x.SPECIALITE).Distinct().ToList()); 
            RefreshNoteStudentList();
            RefreshNoteDetteStudentList();

        }

        private void _syncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            // Application.Current.MainWindow.Cursor = Cursors.Wait;
            PbVisibility = Visibility.Visible;
            ProgressRingIsActive = true;
            PbValue = 0;
            var agent = new ESCLocalDbSyncAgent
            {
                LocalProvider = new ESCLocalDbClientSyncProvider(),
                RemoteProvider = new ESCLocalDbServerSyncProvider()
            };
            agent.SessionProgress += new EventHandler<Microsoft.Synchronization.SessionProgressEventArgs>(agent_SessionProgress);            
            ((ESCLocalDbClientSyncProvider)agent.LocalProvider).ApplyChangeFailed += new EventHandler<Microsoft.Synchronization.Data.ApplyChangeFailedEventArgs>(Local_ApplyChangeFailed);          
            ((ESCLocalDbServerSyncProvider)agent.RemoteProvider).ApplyChangeFailed += new EventHandler<Microsoft.Synchronization.Data.ApplyChangeFailedEventArgs>(Remote_ApplyChangeFailed);            
            e.Result = agent.Synchronize();
        }

        async void agent_SessionProgress(object sender, Microsoft.Synchronization.SessionProgressEventArgs e)
        {
            PbValue = e.PercentCompleted;             
        }

        async void Local_ApplyChangeFailed(object sender, Microsoft.Synchronization.Data.ApplyChangeFailedEventArgs e)
        {
            var _controller = await((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Problem de synchronization", "problème lors de application des changement a la base local ... "));
        }

        async void Remote_ApplyChangeFailed(object sender, Microsoft.Synchronization.Data.ApplyChangeFailedEventArgs e)
        {
            var _controller = await((Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Problem de synchronization", "problème lors de application des changement a la base distante ... "));
        }

        private void RefreshNoteStudentList()
        {
            if (SelectedMatiere != null && SelectedExamin != null && SelectedGroupe != null)
            {
                var cy = int.Parse(CurrentYear);
                ListNotesExamins =
                    new ObservableCollection<EtudiantNote>(_context.NOTES_EXAMEN.Where(
                        x => x.ANNEE_UNIVERSITAIRE == cy &&
                             x.ID_MATIERE == SelectedMatiere.ID_MATIERE &&
                             x.ID_EXAMEN == SelectedExamin.ID_EXAMEN).Join(_context.ETUDES, note => note.ID_ETUDIANT, etude => etude.ID_ETUDIANT, (note, etude) => new
                             {
                                 etudiant = note.ETUDIANT,
                                 note = note.NOTE,
                                 groupe = etude.ID_GROUPE,
                                 Section = etude.ID_SECTION,
                                 specialite = etude.ID_SPECIALITE
                             }).Where(x => x.groupe == SelectedGroupe.ID_GROUPE).Select(x => new EtudiantNote()
                             {
                                 IdEtudiant = x.etudiant.ID_ETUDIANT,
                                 Matricule = x.etudiant.MATRICULE,
                                 Nom = x.etudiant.NOM,
                                 Prenom = x.etudiant.PRENOM,
                                 NomLatin = x.etudiant.NOM_LATIN,
                                 PrenomLatin = x.etudiant.PRENOM_LATIN,
                                 Note = x.note
                             }));
                _listNotesExaminsForSearch = new ObservableCollection<EtudiantNote>(ListNotesExamins);
            }
        }
        private void RefreshNoteDetteStudentList()
        {
            if (SelectedMatiere != null && SelectedGroupe != null)
            {
                var cy = int.Parse(CurrentYear);
                ListNotesDettes =
                    new ObservableCollection<EtudiantNoteDette>(_context.NOTE_DETTE.Where(
                        x => x.ANNEE_PASSAGE_DETTE == cy &&
                             x.ID_MATIERE == SelectedMatiere.ID_MATIERE).Join(_context.ETUDES, note => note.ID_ETUDIANT, etude => etude.ID_ETUDIANT, (note, etude) => new
                             {
                                 etudiant = note.ETUDIANT,
                                 note = note.NOTE,
                                 noteRattrapage = note.NOTE_RATTRAPAGE,
                                 groupe = etude.ID_GROUPE,
                                 Section = etude.ID_SECTION,
                                 specialite = etude.ID_SPECIALITE
                             }).Where(x => x.groupe == SelectedGroupe.ID_GROUPE).Select(x => new EtudiantNoteDette()
                             {
                                 IdEtudiant = x.etudiant.ID_ETUDIANT,
                                 Matricule = x.etudiant.MATRICULE,
                                 Nom = x.etudiant.NOM,
                                 Prenom = x.etudiant.PRENOM,
                                 NomLatin = x.etudiant.NOM_LATIN,
                                 PrenomLatin = x.etudiant.PRENOM_LATIN,
                                 Note = x.note,
                                 NoteRattrapage = x.noteRattrapage
                             }));
                _listNotesDettesForSearch = new ObservableCollection<EtudiantNoteDette>(ListNotesDettes);
            }
        }

        public string ComputeHash(string fileName,string exportTo=null)
        {
            using (var md5 = MD5.Create())
            {

                string to = exportTo ?? "\\res.sdf";                
                File.Copy(fileName, to, true);
                string res="";
                using (var stream = File.OpenRead(to))                
                {
                     res= BitConverter.ToString(md5.ComputeHash(stream));
                }
                if (exportTo==null)
                {
                    File.Delete(to);
                }
                return res;
            }
        }
        
        public void LOG_SaisieNotes(int? idEtudiant, int? idMatiere, double? oldNote, double? newNote, bool dette, double? oldNoteRattrapage = null, double? newNoteRattrapage = null)
        {
            const string details = "[MACHINE={0}];[ACTION=Modification d'une note d'un étudiant];[{1}]";
            _context.LOGs.AddObject(new LOG()
            {
                ID_LOG = Guid.NewGuid(),
                ID_ETUDIANT = idEtudiant,
                ID_USER = (int)LoggedInTeacher.ID_USER,
                JOUR = DateTime.Now,
                OPERATION = "Modifier",
                ANNEE_UNIVERSITAIRE = int.Parse(CurrentYear),
                ID_MATIERE = idMatiere,
                MODULE = "SaisieNotes",
                DETAILS = (!dette) ? String.Format(details, System.Environment.MachineName, "[" + SelectedExamin.DESIGNATION + "=" + oldNote + "-->" + SelectedExamin.DESIGNATION + "=" + newNote + "]") :
                                   String.Format(details, System.Environment.MachineName, "[ancienne note=" + oldNote + "-->" + "nouvelle note=" + newNote + "];[ancienne note rattrapage=" + oldNoteRattrapage + "-->" + "nouvelle note=" + newNoteRattrapage + "];"),

            });
        }
        #endregion
    }
}