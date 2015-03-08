using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfflineTeacher_DBProject;

namespace ESC_OfflineTeacher.Model
{
    public class UserPreferences
    {
        public ENSEIGNANT Enseignant { get; set; }
        public bool LangContFr { get; set; }
        public string UserName { get; set; }
        public int IdUser { get; set; }

    }
}
