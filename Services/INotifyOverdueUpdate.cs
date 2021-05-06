using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  уведомить  о том, что список просроченных книг был обновлен
    public interface INotifyOverdueUpdate
    {
        void NotifyOverdueUpdate();
    }
}
