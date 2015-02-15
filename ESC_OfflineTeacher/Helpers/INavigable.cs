using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC_OfflineTeacher.Helpers
{
    public interface INavigable
    {
        void Activate(object parameter);
        void Deactivate(object parameter);
        void GoBack();
    }
}
