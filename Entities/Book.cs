using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    //  Класс книги, представляющий объект книги, включая запасенное количество
    public class Book:IComparable<Book>, IEquatable<Book>
    {
        //  Частные переменные
        private string isbn, title, author, publisher, category;
        private int totalQuantity;

        //  Аксессоры и мутаторы
        public string ISBN { get => isbn; private set => isbn = value; }
        public string Title { get => title; private set => title = value; }
        public string Author { get => author; private set => author = value; }
        public string Publisher { get => publisher; private set => publisher = value; }
        public string Category { get => category; private set => category = value; }
        public int TotalQuantity { get => totalQuantity; private set => totalQuantity = value; }

        //  Конструктор(ы)
        public Book(string isbn, string title, string author, string publisher, string category, int quantity)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            Publisher = publisher;
            Category = category;
            TotalQuantity = quantity;
        }

        //  Еще одна книги добавлена в инвентарь
        public void AddQuantity(int quantity)
        {
            if(quantity > 0)
                TotalQuantity += quantity;
        }

        //  количество книг изъятых из инвентаря
        public void RemoveQuantity(int quantity)
        {
            TotalQuantity -= quantity;
            if (TotalQuantity < 0)
                TotalQuantity = 0;
        }

        // Для потенциального поиска
        public string Concat()
        {
            return string.Concat(isbn,title,author,publisher,category);
        }

        public int CompareTo(Book other)
        {
            return isbn.CompareTo(other.isbn);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Book))
                return false;
            return Equals(obj as Book);
        }

        public bool Equals(Book other)
        {
            return isbn == other.isbn;
        }

        public override int GetHashCode()
        {
            return isbn.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("'{0,-20} Author:{1}", string.Concat(title,"'"), author);
        }
    }
}
