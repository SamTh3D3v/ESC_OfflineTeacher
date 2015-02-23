using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ESC_OfflineTeacher.Model;
using ESC_OfflineTeacher.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
        #endregion
        #region Properties
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
                        if (!backgroundWorker1.IsBusy)
                        {

                            Debug.WriteLine(
              "Starting Data Synchronization Process...\r\n Please wait till the process compeletes.\r\n");
                            Application.Current.MainWindow.Cursor = Cursors.Wait;

                            SynchronizationHelper syncHelper = new SynchronizationHelper();
                            // Start synchronization
                            backgroundWorker1.RunWorkerAsync(syncHelper);
                        }
                    }));
            }
        }
        #endregion
        #region Ctors and Methods
        public NoteViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            this.backgroundWorker1 =new BackgroundWorker();
            this.backgroundWorker1.WorkerReportsProgress = true;
            // Register the various BackgroundWorker events
            this.backgroundWorker1.DoWork+= new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged+= new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted+= new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

            GenerateFakeData();  //For Test purpuses
        }
        // Method to start syncthonization in background
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker;
            worker = (BackgroundWorker)sender;
            SynchronizationHelper syncHelper = (SynchronizationHelper)e.Argument;
            syncHelper.SynchronizeAsync(worker, e);

        }

        //Method to report synchronization progress
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SyncResults results = (SyncResults)e.UserState;
            if (results != null)
            {
                DisplayStats(results);
            }
        }

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
        private void DisplayStats(SyncResults results)
        {

            TimeSpan diff =
                results.Stats.SyncEndTime.Subtract(results.Stats.SyncStartTime);

            Debug.WriteLine(
                string.Format(
"{4}:  - Total Time To Synchronize = {0}:{1}:{2}:{3}\r\nTotal Records Uploaded: {5}  Total Records Downloaded: {6}\r\n",
               diff.Hours, diff.Minutes, diff.Seconds,
               diff.Milliseconds, results.Message,
               results.Stats.UploadChangesTotal,
               results.Stats.DownloadChangesTotal));
        }

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
        #endregion


    }
}