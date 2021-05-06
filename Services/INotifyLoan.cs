using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  уведомить о том, что книга была одолжена
    public interface INotifyLoan
    {
        void NotifyLoan();
    }
}
