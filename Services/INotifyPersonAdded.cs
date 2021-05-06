using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  уведомить о том, что человек был добавлен в систему
    public interface INotifyPersonAdded
    {
        void NotifyPersonAdded();
    }
}
