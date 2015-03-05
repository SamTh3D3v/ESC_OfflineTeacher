using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC_OfflineTeacher.Model
{
    public class EtudiantNote
    {
        public int IdEtudiant { get; set; }
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomLatin { get; set; }
        public string PrenomLatin { get; set; }
        public double? Note { get; set; }
        //public double? NoteRattrapage { get; set; }
        //public double? NoteParticipation { get; set; }        
        //public double? NoteDette { get; set; }        
    }

    public class EtudiantNoteDette : EtudiantNote
    {
        public double? NoteRattrapage { get; set; }
    }
}
