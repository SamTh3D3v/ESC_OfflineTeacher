using System.Collections.ObjectModel;
using ESC_OfflineTeacher.Model;
using GalaSoft.MvvmLight;

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

        #endregion
        #region Ctors and Methods
        public NoteViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            GenerateFakeData();  //For Test purpuses
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