using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace withDBSql
{
    /// <summary>
    /// Логика взаимодействия для AuthoWin.xaml
    /// </summary>
    public partial class AuthoWin : Window
    {
        public AuthoWin()
        {
            InitializeComponent();
        }

        private void Button_Main_Wind_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        public void Button_Auto_Click(object sender, RoutedEventArgs e)
        {
            string loginA = textBoxPhone.Text.Trim();
            string passA = passBox.Password.Trim();
            string name = "";

            if (passA.Length < 5)
            {
                passBox.ToolTip = "Пароль должен должен превышать 5 символов";
                passBox.Background = Brushes.DarkRed;
            }
            else
            {
                textBoxPhone.ToolTip = "";
                textBoxPhone.Background = Brushes.Transparent;

                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                DB dB = new DB();

                MySqlCommand command = new MySqlCommand("SELECT * FROM `admins` WHERE `login` = @lA AND `pass` = @pA", dB.getConnection());

                command.Parameters.Add("@lA", MySqlDbType.VarChar).Value = loginA;
                command.Parameters.Add("@pA", MySqlDbType.VarChar).Value = passA;

                try
                {
                    dB.openConnection();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        name = reader.GetString("name");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса к базе данных: " + ex.Message);
                }
                finally
                {
                    dB.closeConnection();
                }

                
                if (name == "Admin")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    this.Hide();
                }
                else if (name == "cod1" || name == "cod2" || name == "cod3")
                {
                    teacherWind teacherWind = new teacherWind();
                    teacherWind.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Вы ввели некорректные данные");
                }
            }
        }
    }
}
