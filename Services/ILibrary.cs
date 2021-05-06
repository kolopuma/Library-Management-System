using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  Представляет то, что типичная библиотека должна быть в состоянии сделать для Patron
    public interface ILibrary
    {
        //  срок выдачи
        double LoanDuration { get; }
        //  поиск в инвентаре
        List<Book> SearchInventory(string search);
        //  просмотреть Список доступных книг
        List<Book> ViewAvailableBooks();
        //  наличие книги 
        bool HaveBook(Book b);
        //  наличие книги через isbn
        bool HaveBook(string isbn);
        //  проверка на доступ к книге ( не все в выдаче )
        bool IsBookAvailable(Book b);
        //  Регистрация книжной выдачи
        bool LoanBook(Book book, LibraryPatron patron);
        //  Заемщик может вернуть книгу
        bool BookReturn(Book book, LibraryPatron patron);
    }
}
