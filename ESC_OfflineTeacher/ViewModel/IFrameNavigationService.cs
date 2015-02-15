using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace ESC_OfflineTeacher.ViewModel
{
    public interface IFrameNavigationService:INavigationService
    {
        object Parameter { get; }  
    }
}
