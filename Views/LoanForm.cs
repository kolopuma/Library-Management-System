using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class LoanForm : Form
    {
        LibraryPatron borrower;
        Book book;
        INotifyLoan listener;
        
        public LoanForm(Book b, INotifyLoan parent)
        {
            InitializeComponent();
            personSearchBox.DataSource = College.Instance.ToList();
            book = b;
            listener = parent;
        }

        //  Всякий раз, когда текст в поле поиска изменяется, 
        private void inputPersonSearch_TextChanged(object sender, EventArgs e)
        {
            if (inputPersonSearch.Text.Length == 0)
                personSearchBox.DataSource = College.Instance.ToList();
            else
                personSearchBox.DataSource = College.Instance.GetPeople(inputPersonSearch.Text.ToLower());
        }

        //  когда пользователь дважды нажимает на человека, этот метод вызывается
        private void personSearchBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  exceptions (security measures)
            try
            {
                borrower = personSearchBox.SelectedItem as LibraryPatron;
                if (borrower == null)
                    throw new ArgumentNullException("LoanForm[39]: Selected person not of type 'LibraryPatron'");
            }catch(ArgumentNullException ane)
            {
                //  log exception
                new NotificationForm("Patron not found.").ShowDialog();
            }

            //  Если заемщик достиг своего лимита, сообщите об этом и не разрешайте выдачу 
            if (borrower.ReachedBookLimit())
                new NotificationForm(borrower.Name + " has met their quota of " + (int)(borrower as Student).Status + " books, and therefore cannot borrow anymore books.").ShowDialog();
            else if (borrower.HasBook(book))    //  если у них уже есть книга, не допускается другая
                new NotificationForm(borrower.Name + " already has a copy of this book. 1 copy permitted only.").ShowDialog();
            else
            {
                //  Подтвердите выдачу книги взаймы нужному человеку
                var confirmResult = MessageBox.Show(
                string.Format("Are you sure you want to loan: '{0}', by {1}, to {2}", book.Title, book.Author, borrower.Name),
                                    "Confirmation",
                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    //  Пользователь еще не подтвержден, поэтому система пытается выдать избранному покровителю книгу
                    if (College.Instance.Library.LoanBook(book, borrower))
                        listener.NotifyLoan();  //  update listener 

                    this.Close();
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
