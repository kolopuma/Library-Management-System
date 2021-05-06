using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  уведомить о том, что в систему добавлена книга
    public interface INotifyBookAdded
    {
        void NotifyBookAdded();
    }
}
