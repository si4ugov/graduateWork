using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace withDBSql
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string secondName = textBoxSecondName.Text.Trim();
            string surnameName = textBoxSurnameName.Text.Trim();
            string phone = textBoxPhone.Text.Trim();
            string email = textBoxEmail.Text.Trim().ToLower();
            string pass = passBox.Password.Trim();
            string pass_2 = passBox_2.Password.Trim();
            string kurs = selectKurs.Text.Trim();
            string seria = textBoxSeria.Text.Trim();
            string nomerPas = textBoxNomerPas.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                textBoxName.ToolTip = "Поле обязательно для заполнения!";
                textBoxName.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxName.ToolTip = "";
                textBoxName.Background = Brushes.Transparent;
            }

            // Проверка на ввод фамилии
            if (string.IsNullOrEmpty(secondName))
            {
                textBoxSecondName.ToolTip = "Поле обязательно для заполнения!";
                textBoxSecondName.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxSecondName.ToolTip = "";
                textBoxSecondName.Background = Brushes.Transparent;
            }

            // Проверка на ввод отчества
            if (string.IsNullOrEmpty(surnameName))
            {
                textBoxSurnameName.ToolTip = "Поле обязательно для заполнения!";
                textBoxSurnameName.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxSurnameName.ToolTip = "";
                textBoxSurnameName.Background = Brushes.Transparent;
            }

            // Проверка на ввод номера телефона
            if (string.IsNullOrEmpty(phone))
            {
                textBoxPhone.ToolTip = "Поле обязательно для заполнения!";
                textBoxPhone.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxPhone.ToolTip = "";
                textBoxPhone.Background = Brushes.Transparent;
            }

            // Проверка на ввод email
            if (!email.Contains("@") || email.Length < 5 || !email.Contains("."))
            {
                textBoxEmail.ToolTip = "Email введён некорректно";
                textBoxEmail.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxEmail.ToolTip = "";
                textBoxEmail.Background = Brushes.Transparent;
            }

            // Проверка на ввод серии
            if (string.IsNullOrEmpty(seria))
            {
                textBoxSurnameName.ToolTip = "Поле обязательно для заполнения!";
                textBoxSurnameName.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxSurnameName.ToolTip = "";
                textBoxSurnameName.Background = Brushes.Transparent;
            }

            // Проверка на ввод номера паспорта
            if (string.IsNullOrEmpty(nomerPas))
            {
                textBoxSurnameName.ToolTip = "Поле обязательно для заполнения!";
                textBoxSurnameName.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                textBoxSurnameName.ToolTip = "";
                textBoxSurnameName.Background = Brushes.Transparent;
            }
            // Проверка на ввод пароля
            if (pass.Length < 5)
            {
                passBox.ToolTip = "Пароль должен должен превышать 5 символов";
                passBox.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;
            }

            // Проверка на подтверждение пароля
            if (pass_2 != pass)
            {
                passBox_2.ToolTip = "Пароли не совпадают!";
                passBox_2.Background = Brushes.DarkRed;
                return;
            }
            else
            {
                passBox_2.ToolTip = "";
                passBox_2.Background = Brushes.Transparent;
            }

            // Проверка на выбор курса
            if (kurs == null)
            {
                selectKurs.ToolTip = "Это поле обязательно";
                selectKurs.Background = Brushes.DarkRed;
                return;
            }
            if(checkEmailUsers())
                return;

            else
            {
                selectKurs.ToolTip = "";
                selectKurs.Background = Brushes.Transparent;

                DB db = new DB();

                MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`Имя`, `Фамилия`, `Отчество`, `Курс`, `Пароль`, `Email`, `Номер телефона`, `Серия`, `Номер паспорта`) VALUES (@name, @secondName, @surnameName, @kurs, @pass, @email, @phone, @seria, @nomerPas)", db.getConnection());

                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@secondName", MySqlDbType.VarChar).Value = secondName;
                command.Parameters.Add("@surnameName", MySqlDbType.VarChar).Value = surnameName;
                command.Parameters.Add("@kurs", MySqlDbType.VarChar).Value = kurs;
                command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = pass;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phone;
                command.Parameters.Add("@seria", MySqlDbType.VarChar).Value = seria;
                command.Parameters.Add("@nomerPas", MySqlDbType.VarChar).Value = nomerPas;

                db.openConnection();

                if (command.ExecuteNonQuery()  == 1)
                {
                    MessageBox.Show("Вы успешно зарегестрировались");
                    textBoxName.Text = "";
                    textBoxSecondName.Text = "";
                    textBoxSurnameName.Text = "";
                    textBoxPhone.Text = "";
                    textBoxEmail.Text = "";
                    passBox.Password = "";
                    passBox_2.Password = "";
                    selectKurs.Text = "";
                    textBoxNomerPas.Text = "";
                    textBoxSeria.Text = "";
                }
                else
                {
                    MessageBox.Show("Вы не смогли зарегестрироваться");
                }

                db.closeConnection();
            }
        }

        private void Button_Autho_Window_Click(object sender, RoutedEventArgs e)
        {
            AuthoWin authoWin = new AuthoWin();
            authoWin.Show();
            this.Hide();
        }

        public Boolean checkEmailUsers()
        {
            DB db = new DB();   

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `Email` = @email", db.getConnection());

            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = textBoxEmail.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count>0)
            {
                MessageBox.Show("Такой Email уже существует");
                return true;
            }
            else
                return false;
            
        }
    }
}
