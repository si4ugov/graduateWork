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
  
    public partial class GroupWind : Window
    {
        public GroupWind()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Hide();
        }

        private void FormGroups_Click(object sender, RoutedEventArgs e)
        {
            string kurs = comboBox_Courses.Text.Trim();

            // 1. Получить список пользователей с выбранным курсом
            List<User> users = new List<User>();

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `Курс` = @kurs", db.getConnection());
            command.Parameters.Add("@kurs", MySqlDbType.VarChar).Value = kurs;

            try
            {
                db.openConnection();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32("Id");
                    user.Name = reader.GetString("Имя");
                    user.SecondName = reader.GetString("Фамилия");
                    user.SurnameName = reader.GetString("Отчество");
                    user.Kurs = reader.GetString("Курс");
                    user.Pass = reader.GetString("Пароль");
                    user.Email = reader.GetString("Email");
                    user.Phone = reader.GetString("Номер телефона");

                    users.Add(user);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса к базе данных: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            UserGroup group = new UserGroup(users, kurs);

            dataGrid_Groups.ItemsSource = group.Users;
        }
    }
}
