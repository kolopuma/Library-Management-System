using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    //  Домашняя страница приложения
    public partial class EntryForm : Form, INotifyOverdueUpdate, INotifyPersonAdded, INotifyBookAdded
    {
        //   поток таймера для обновления окна "Просроченные клиенты"
        private static System.Threading.Timer updater;

        public EntryForm()
        {
            InitializeComponent();
            //  Источник данных списка людей в списке людей колледжа
            personSearchBox.DataSource = College.Instance.ToList();
            //  Источник данных списка книг в списке книг библиотеки
            bookSearchBox.DataSource = College.Instance.Library.Inventory.Keys.ToList<Book>();
            patronsOverdueBox.DataSource = new List<LibraryPatron>();

            //  Запускайть поток каждые 1 секунду
            updater = new System.Threading.Timer(UpdateCallback, this, 1000, 1000);
        }

        //  Вызываеть таймером каждую секунду, чтобы обновить поле "выданные книги" .
        private static void UpdateCallback(object obj)
        {
            //  Уведомить моего слушателя, домашнюю страницу (форму ввода), что она должна быть обновлена
            if (obj is EntryForm)
                (obj as EntryForm).NotifyOverdueUpdate();
        }

        private void addLecturerClicked(object sender, EventArgs e)
        {
            new AddLecturerForm(this).ShowDialog();
        }

        private void personSearchBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LibraryPatron p = personSearchBox.SelectedItem as LibraryPatron;
            if (p != null)
                new PersonForm(p).ShowDialog();
        }

        private void inputPersonSearch_TextChanged(object sender, EventArgs e)
        {
            if (inputPersonSearch.Text.Length == 0)
                personSearchBox.DataSource = College.Instance.ToList();
            else
                personSearchBox.DataSource = College.Instance.GetPeople(inputPersonSearch.Text.ToLower());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AddStudentForm(this).ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AddBookForm(this).ShowDialog();
        }

        private void inputSearchBook_TextChanged(object sender, EventArgs e)
        {
            if (inputSearchBook.Text.Length == 0)
                bookSearchBox.DataSource = College.Instance.Library.Inventory.Keys.ToList<Book>();
            else {
                string searchText = inputSearchBook.Text.ToLower();
                bookSearchBox.DataSource = College.Instance.Library.Inventory.Keys.ToList<Book>().FindAll(
                    x => x.Title.ToLower().Contains(searchText) || x.Author.ToLower().Contains(searchText));
            }
        }

        private void bookSearchBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Book b = bookSearchBox.SelectedItem as Book;
            if (b != null)
                new BookForm(b).ShowDialog();
        }

        private void EntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            updater.Dispose();
        }

        //  Person, , открыть PersonForm  Person
        private void personDetailsButton_Click(object sender, EventArgs e)
        {
            LibraryPatron p = personSearchBox.SelectedItem as LibraryPatron;
            if (p != null)
                new PersonForm(p).ShowDialog();
        }

        //  Book выбранная с помощью кнопки, открыть BookForm для выбранной Book
        private void bookDetailsButton_Click(object sender, EventArgs e)
        {
            Book b = bookSearchBox.SelectedItem as Book;
            if (b != null)
                new BookForm(b).ShowDialog();
        }

        //  Если  выбран radiobutton, обновите список
        private void lecturerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePersonList();
        }

        //  Все radiobutton выбраны, обновите список
        private void allRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePersonList();
        }

        //  Если выбран radiobutton Student, обновите список
        private void studentsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePersonList();
        }

        //  Обновите список лиц и обязательно учтите текстовое поле поиска, а также выбор radiobutton
        private void UpdatePersonList()
        {
            personSearchBox.DataSource = null;
            if (allRadioButton.Checked)
            {
                if (inputPersonSearch.Text.Length == 0)
                    personSearchBox.DataSource = College.Instance.ToList();
                else
                    personSearchBox.DataSource = College.Instance.GetPeople(inputPersonSearch.Text.ToLower());
            }
            else if (studentsRadioButton.Checked)
            {
                if (inputPersonSearch.Text.Length == 0)
                    personSearchBox.DataSource = College.Instance.GetStudents();
                else
                    personSearchBox.DataSource = College.Instance.GetStudents(inputPersonSearch.Text.ToLower());
            }
            else
            {
                if (inputPersonSearch.Text.Length == 0)
                    personSearchBox.DataSource = College.Instance.GetLecturers();
                else
                    personSearchBox.DataSource = College.Instance.GetLecturers(inputPersonSearch.Text.ToLower());
            }
        }

        //  Обновить список книг и обязательно учтите текстовое поле поиска
        private void UpdateBooksData()
        {
            if (inputSearchBook.Text.Length == 0)
            {
                bookSearchBox.DataSource = College.Instance.Library.Inventory.Keys.ToList<Book>();
            }
            else
            {
                string searchText = inputSearchBook.Text.ToLower();
                bookSearchBox.DataSource = College.Instance.Library.Inventory.Keys.ToList<Book>().FindAll(
                    x => x.Title.ToLower().Contains(searchText) || x.Author.ToLower().Contains(searchText));
            }
        }



        //  Когда пользователь дважды нажимает на человека в Overdue box, откройте форму этого человека
        private void patronsOverdueBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LibraryPatron p = patronsOverdueBox.SelectedItem as LibraryPatron;
            if (p != null)
                new PersonForm(p).ShowDialog();
        }

        //  Уведомления

        //  Обновить "Overdue Patrons" box
        public void NotifyOverdueUpdate()
        {
            List<LibraryPatron> patrons = College.Instance.Library.OverduePatrons();
            if (!Enumerable.SequenceEqual(patrons, patronsOverdueBox.DataSource as List<LibraryPatron>))
                patronsOverdueBox.DataSource = patrons;
        }

        //  Человек должен был быть добавлен в систему, обновить список лиц
        public void NotifyPersonAdded()
        {
            UpdatePersonList();
        }

        //  Книга должна была быть добавлена в систему, обновите список книг
        public void NotifyBookAdded()
        {
            UpdateBooksData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you Sure You want to Exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
