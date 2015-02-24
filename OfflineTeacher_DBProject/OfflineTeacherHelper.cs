using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineTeacher_DBProject
{
    public class OfflineTeacherHelper
    {
        public String GetYear()
        {
            String res = "";
            using (LocalSGSDBEntities context = new LocalSGSDBEntities())
            {
                res = context.ANNEES.Max(x => x.ANNEE_UNIVERSITAIRE).ToString(CultureInfo.InvariantCulture);
            }
            return res;
        }
    }
}
