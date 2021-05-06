using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class LoginForm : Form
    {
        Thread th;
        /* Переменные, которые будут хранить на протяжение работы программы логин и пароль. */
        public string login = string.Empty;
        public string password = string.Empty;
        private Users user = new Users(); // Экземпляр класса пользователей.
        public LoginForm()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            try
            {
                FileStream fs = new FileStream("Users.dat", FileMode.Open);

                BinaryFormatter formatter = new BinaryFormatter();

                user = (Users)formatter.Deserialize(fs);

                fs.Close();
            }
            catch { return; }
        }
        private void EnterToForm()
        {
            for (int i = 0; i < user.Logins.Count; i++) // Ищем пользователя и проверяем правильность пароля.
            {
                if (user.Logins[i] == txtUserName.Text && user.Passwords[i] == txtPassword.Text)
                {
                    login = user.Logins[i];
                    password = user.Passwords[i];

                    MessageBox.Show("Вы вошли в систему!");
                    this.Close();


                }
                else if (user.Logins[i] == txtUserName.Text && txtPassword.Text != user.Passwords[i])
                {
                    login = user.Logins[i];

                    MessageBox.Show("Неверный пароль!");
                }
                
            }


            if (login == "") { MessageBox.Show("Пользователь " + txtUserName.Text + " не найден!"); }
            
            
        }
        private void AddUser() // Регистрируем нового пользователя.
        {
            if (txtUserName.Text == "" || txtPassword.Text == "") { MessageBox.Show("Не введен логин или пароль!"); return; }
            if (user.Logins.Contains(txtUserName.Text))
            {
                MessageBox.Show("Пользовать уже зарегестрирован!");
            }
            else
            {
                user.Logins.Add(txtUserName.Text);
                user.Passwords.Add(txtPassword.Text);

                FileStream fs = new FileStream("Users.dat", FileMode.OpenOrCreate);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, user); // Сериализуем класс.

                fs.Close();

                login = txtUserName.Text;
                MessageBox.Show("Пользователь удачно зарегестрирован!");
            }
            
            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnterToForm();
            if (this.IsDisposed)
            {
                th = new Thread(open);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }

        }
        public void open(object obj)
        {
            Application.Run(new EntryForm());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddUser();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox_Show_Hide_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Show_Hide.Checked)
            {
                txtPassword.UseSystemPasswordChar = true;
                var checkBox = (CheckBox)sender;
                checkBox.Text = "View";
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
                var checkBox = (CheckBox)sender;
                checkBox.Text = "Hide";
            }
            
        }
    }
}
