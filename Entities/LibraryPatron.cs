using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public abstract class LibraryPatron : Person, ICollection<LoanedBook>
    {
        //  Частные переменные
        private List<LoanedBook> borrowedBooks;

        //  Конструктор(ы)
        public LibraryPatron(string ppsn, string name, string address, string phone, string email) : base(ppsn, name, address, phone, email)
        {
            borrowedBooks = new List<LoanedBook>();
        }

        //public List<BorrowedBook> BorrowedBooks { get => borrowedBooks; private set => borrowedBooks = value; }

        public int Count => ((ICollection<LoanedBook>)borrowedBooks).Count;

        public bool IsReadOnly => ((ICollection<LoanedBook>)borrowedBooks).IsReadOnly;

        //  Студент может одолжить книгу в пределах своей ограниченной суммы, если у него нет просроченной задолженности
        public abstract bool ReachedBookLimit();

        //  Проверяет, есть ли у patron книга
        public bool HasBook(Book b)
        {
            return borrowedBooks.Contains(b);
        }

        //  Попытка позаимствовать книгу из библиотеки
        public bool RequestLoan(Book book, ILibrary library)
        {
            return library.LoanBook(book, this);
        }

        //  возвращает книгу в библиотеку, если она у них есть
        public bool ReturnBook(Book b)
        {
            if (borrowedBooks.Exists(x => x.ISBN == b.ISBN))
            {
                int index = borrowedBooks.FindIndex(x => x.ISBN == b.ISBN);
                if (index >= 0 && borrowedBooks[index].ReturnBook(this))
                    return borrowedBooks.Remove(borrowedBooks[index]);
            }
            return false;
        }

        //  Время просрочки книги
        public void EmailLateNoticeAlert(Book b)
        {
            throw new NotImplementedException();
        }

        //  Просмотр сведений о конкретной заимствованной книге
        //public LoanedBook ViewBorrowedBookDetails(Book b)
        //{
        //    try
        //    {
        //        if (!borrowedBooks.Contains(b))
        //            throw new BookNotFoundException(string.Format("BorrowedBook: {0} not found.", b.ISBN));
        //        else if (b == null)
        //            throw new ArgumentNullException("Argument null in Student.ViewBorrowedBook(Book)");
        //    }
        //    catch (BookNotFoundException bnfe)
        //    {
        //        Console.WriteLine(bnfe.Message);
        //    }
        //    catch (ArgumentNullException ane)
        //    {
        //        Console.WriteLine(ane.Message);
        //    }
        //    return borrowedBooks[borrowedBooks.IndexOf(b as LoanedBook)];
        //}

        //  проверка, не просрочена ли конкретная книга
        public bool IsBookOverdue(Book b)
        {
            if (borrowedBooks.Contains(b))
                return borrowedBooks.Find(x => x.Equals(b)).IsOverdue();
            return false;
        }

        //  проверка, нет ли у патрона просроченных книг
        public bool HasOverdueBook()
        {
            return borrowedBooks.Exists(x => x.IsOverdue());
        }

        //  переопределенные методы объектов
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void Add(LoanedBook item)
        {
            if(!ReachedBookLimit() && !Contains(item))
                ((ICollection<LoanedBook>)borrowedBooks).Add(item);
        }

        public void Clear()
        {
            ((ICollection<LoanedBook>)borrowedBooks).Clear();
        }

        public bool Contains(LoanedBook item)
        {
            return ((ICollection<LoanedBook>)borrowedBooks).Contains(item);
        }

        public void CopyTo(LoanedBook[] array, int arrayIndex)
        {
            ((ICollection<LoanedBook>)borrowedBooks).CopyTo(array, arrayIndex);
        }

        public bool Remove(LoanedBook item)
        {
            return ((ICollection<LoanedBook>)borrowedBooks).Remove(item);
        }

        public IEnumerator<LoanedBook> GetEnumerator()
        {
            return ((ICollection<LoanedBook>)borrowedBooks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<LoanedBook>)borrowedBooks).GetEnumerator();
        }
    }
}
