using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  Представляет собой книгу, которая была заимствована
    public class LoanedBook : Book, IComparable<Book>, IEquatable<Book>
    {
        //  Частные переменные

        //  дата выдачи
        private DateTime dateIssued;
        //  дата, когда книга должна быть возвращена
        private DateTime dueDate;
        //  библиотека, из которой он был позаимствован
        private ILibrary library;
        //  независимо от того, был ли уже продлен кредит на книгу ( только один раз )
        private bool extended;

        //  Аксессоры и мутаторы
        public DateTime IssueDate { get => dateIssued; private set => dateIssued = value; }
        public DateTime DueDate { get => dueDate; private set => dueDate = value; }
        internal ILibrary Library { get => library; private set => library = value; }
        public DateTime DateIssued { get => dateIssued; private set => dateIssued = value; }

        //  Конструктор(ы)
        public LoanedBook(Book book, ILibrary library) : base(book.ISBN, book.Title, book.Author, book.Publisher, book.Category, book.TotalQuantity)
        {
            //  установление даты выдачи на данное время
            DateIssued = DateTime.Now;

            //  Если приложение скомпилировано с набором переменных DebugMode
            if (College.DebugMode)
            {
                //  Срок выполнения становится случайным временем между настоящим и минимальными / максимальными секундами
                Random r = new Random();
                DueDate = DateTime.Now.AddMilliseconds(
                    r.Next((int)College.DebugTimeBetweenMillis.Min, (int)College.DebugTimeBetweenMillis.Max));
            }
            else
                DueDate = DateTime.Now.AddDays(library.LoanDuration);   //  библиотека решает срок кредита

            Library = library;
            extended = false;
        }

        //  Запросить продление кредита
        public bool RequestExtension()
        {
            //  разрешено только один раз
            if (!extended)
            {
                if (College.DebugMode)
                {
                    Random r = new Random();
                    DueDate = DateTime.Now.AddMilliseconds(
                        r.Next((int)College.DebugTimeBetweenMillis.Min, (int)College.DebugTimeBetweenMillis.Max));
                }
                else
                    dueDate.AddDays(library.LoanDuration);
                extended = true;
                return true;
            }
            return false;
        }

        //вернуть позаимствованную книгу
        public bool ReturnBook(LibraryPatron borrower)
        {
            return library.BookReturn(this, borrower);
        }

        //  проверьте, не просрочена ли книга
        public bool IsOverdue()
        {
            return DateTime.Now.CompareTo(DueDate) > 0;
        }

        //  представьте дату и время выпуска в виде строки
        public string ShowIssuedDateTime()
        {
            return DateIssued.ToString();
        }

        //  представьте дату и время выполнения в виде строки
        public string ShowDueDateTime()
        {
            return DueDate.ToString();
        }

        //  переопределение методов объекта

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("'{0, -21}{1}", string.Concat(Title, "'"), 
                (IsOverdue() ?  "OVERDUE: "+ShowDueDateTime() :  "Due: " + ShowDueDateTime()));
        }
    }
}
