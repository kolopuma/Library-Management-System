using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  Представляет библиотеку
    public class Library:ILibrary
    {
        //  Частные переменные

        //  в инвентаре хранится библиотечный инвентарь, и кто берет берет книги
        private SortedList<Book, List<LibraryPatron>> inventory;

        //  Аксессоры и мутаторы
        internal SortedList<Book, List<LibraryPatron>> Inventory { get => inventory; }

        //  Продолжительность библиотечной выдачи книг 
        public double LoanDuration => 7;

        //  Конструктор(ы)
        public Library()
        {
            inventory = new SortedList<Book, List<LibraryPatron>>();
        }

        public Library(List<Book> books)
        {
            inventory = new SortedList<Book, List<LibraryPatron>>();
            foreach (Book book in books)
                inventory.Add(book, new List<LibraryPatron>());
        }

        //  Методы

        public bool HaveBook(Book b)
        {
            return inventory.ContainsKey(b);
        }

        public bool HaveBook(string isbn)
        {
            return inventory.Keys.ToList().Exists(x => x.ISBN == isbn);
        }

        public bool LoanBook(Book book, LibraryPatron patron)
        {
            if (!College.Instance.Contains(patron))
                return false;
            if(!patron.ReachedBookLimit() && !patron.Contains(book) && IsBookAvailable(book))
            {
                LoanedBook loanBook = new LoanedBook(book, this);
                patron.Add(loanBook);
                inventory[book].Add(patron);
                return true;
            }
            return false;
        }

        //public bool RegisterBookLoan(Book book, LibraryPatron borrower)
        //{
        //    if (College.Instance.PersonInSystem(borrower as Person))
        //        if (borrower.CanBorrow() && !borrower.HasBook(book) && IsBookAvailable(book))
        //        {
        //            inventory[book].Add(borrower);
        //            return true;
        //        }
        //    return false;
        //}

        public bool BookReturn(Book book, LibraryPatron patron)
        {
            if (inventory.ContainsKey(book) && inventory[book].Contains(patron))
                return inventory[book].Remove(patron);
            return false;
        }

        public bool IsBookAvailable(Book b)
        {
            return inventory.Keys.Contains(b) && inventory[b].Count < b.TotalQuantity;
        }

        public List<Book> ViewAvailableBooks()
        {
            return inventory.Keys.Where(x => inventory[x].Count < x.TotalQuantity).ToList();
        }

        //  Возвращает список книг, которые содержат строку в той или иной форме
        public List<Book> SearchInventory(string search)
        {
            return new List<Book>(inventory.Keys.Where(x => x.Concat().Contains(search)));
        }

        //  Добавление книги в библиотечный инвентарь
        public void AddBook(Book b)
        {
            if (!inventory.ContainsKey(b))
                inventory.Add(b, new List<LibraryPatron>());
            else
                inventory.Keys[inventory.IndexOfKey(b)].AddQuantity(b.TotalQuantity);
        }

        // Номер, доступный для конкретной книги
        public int NumberAvailable(Book b)
        {
            if (!inventory.Keys.Contains(b))
                return 0;
            return b.TotalQuantity - inventory[b].Count;
        }

        //  Люди, которые в настоящее время одалживают книгу
        public List<LibraryPatron> PeopleWith(Book b)
        {
            if(inventory.ContainsKey(b))
                return inventory[b];
            return new List<LibraryPatron>(0);
        }

        //  Список клиентов, у которых есть просроченные книги)
        public List<LibraryPatron> OverduePatrons()
        {
            HashSet<LibraryPatron> overdueBorrowers = new HashSet<LibraryPatron>();
            List<Book> borrowedBooks = inventory.Keys.Where(x => inventory[x].Count > 0).ToList<Book>();
            foreach (Book book in borrowedBooks)
                foreach(LibraryPatron patron in inventory[book].FindAll(x => x.HasOverdueBook()))
                    overdueBorrowers.Add(patron);
            return overdueBorrowers.ToList<LibraryPatron>();
        }

        //  Найдите всех patrons, у которых просрочена конкретная книга
        public List<LibraryPatron> Overdue(Book b)
        {
            return inventory[b].FindAll(x => x.IsBookOverdue(b));
        }

        //  Получить книгу из инвентаря, соответствующую isbn
        public Book GetBook(string isbn)
        {
            return inventory.Keys.ToList<Book>().Find(x => x.ISBN == isbn);
        }
    }
}
