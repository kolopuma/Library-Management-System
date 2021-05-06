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
    //  Просмот сведений о книге и разрешите одолжить или вернуть книгу
    public partial class BookForm : Form, INotifyLoan, INotifyReturn
    {
        private Book book;

        public BookForm(Book b)
        {
            InitializeComponent();
            book = b;
            outputAuthor.Text = book.Author;
            outputCategory.Text = book.Category;
            outputISBN.Text = book.ISBN;
            outputPublisher.Text = book.Publisher;
            outputQuantity.Text = ""+ book.TotalQuantity;
            outputTitle.Text = book.Title;
            int numAvailable = UpdateNumAvailable();
            if (numAvailable == 0)
                loanButton.Enabled = false;
            else if (numAvailable == book.TotalQuantity)
                returnButton.Enabled = false;
        }

        //  При оформлении выдачи обновить пользовательский интерфейс
        public void NotifyLoan()
        {
            int numAvailable = UpdateNumAvailable();
            if (numAvailable == 0)
                loanButton.Enabled = false;
            else
                returnButton.Enabled = true;
        }

        //  При возврате обновить пользовательский интерфейс
        public void NotifyReturn()
        {
            int numAvailable = UpdateNumAvailable();
            if (numAvailable == book.TotalQuantity)
                returnButton.Enabled = false;
            else
                loanButton.Enabled = true;
        }

        //  Обновляет значение поля outputAvailable  (после выдачи или возврата)
        private int UpdateNumAvailable()
        {
            int numAvailable = College.Instance.Library.NumberAvailable(book);
            outputAvailable.Text = "" + numAvailable;
            return numAvailable;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //  Откройть форму выдачи для книги
        private void loanButton_Click(object sender, EventArgs e)
        {
            new LoanForm(book, this).ShowDialog();
        }

        // Откройте форму возврата для книги
        private void returnButton_Click(object sender, EventArgs e)
        {
            new ReturnForm(book, this).ShowDialog();
        }
    }
}
