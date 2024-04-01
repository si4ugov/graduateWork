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
    public partial class delWindow : Window
    {
        AdminWindow adminWindow = new AdminWindow();
        public delWindow()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Получаем введенный пользователем ID
            int userId = 0;
            if (!int.TryParse(txtUserId.Text.Trim(), out userId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID пользователя");
                return;
            }

            // Удаляем пользователя из базы данных
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("DELETE FROM users WHERE id=@id", db.getConnection());
            command.Parameters.AddWithValue("@id", userId);
            db.openConnection();
            int deletedRows = command.ExecuteNonQuery();
            db.closeConnection();

            // Проверяем, что удаление прошло успешно
            if (deletedRows == 1)
            {
                MessageBox.Show("Пользователь удален успешно");
                LoadUsers();
                this.Close(); // закрываем форму после успешного удаления
            }
            else
            {
                MessageBox.Show("Не удалось удалить пользователя. Попробуйте еще раз");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // закрываем форму без удаления
        }

        private void LoadUsers()
        {
            DB db = new DB();

            MySqlCommand command = new MySqlCommand("SELECT id, Имя, Фамилия, Отчество, Курс, Email, `Номер телефона` FROM users", db.getConnection());

            db.openConnection();
            var reader = command.ExecuteReader();
            var dataList = new List<User>();

            while (reader.Read())
            {

                var data = new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    SecondName = reader.GetString(2),
                    SurnameName = reader.GetString(3),
                    Kurs = reader.GetString(4),
                    Pass = reader.GetString(5),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                };

                dataList.Add(data);
            }
            reader.Close();
            adminWindow.dataGrid.ItemsSource = dataList;
            db.closeConnection();
        }

    }
}
