using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace withDBSql
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("SELECT id, Имя, Фамилия, Отчество, Курс, Email, `Номер телефона` FROM users", db.getConnection());

            LoadUsers();
        }

        //Кнопка для перехода на форму удаления
        private void Button_Del_Click(object sender, RoutedEventArgs e)
        {
            delWindow deltWindow = new delWindow();
            deltWindow.Show();
        }
        // Кнопка обновления таблицы
        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }
        // Для кнопки обновления таблицы
        private void LoadUsers()
        {
            var selectedRow = dataGrid.SelectedItem as DataRowView;
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
            
            dataGrid.ItemsSource = dataList;
            db.closeConnection();
        }
        private void Button_Dog_Create_Click(object sender, RoutedEventArgs e)
        {
            Dogovor dogovor = new Dogovor();
            dogovor.Show();
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void Button_Ratings_Click(object sender, RoutedEventArgs e)
        {
            RatingWindow ratingWindow = new RatingWindow();
            ratingWindow.Show();
            this.Hide();
        }

        private void Button_Create_Excel(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT id, Имя, Фамилия, Отчество, Курс, Оценка, Пароль, Email, `Номер телефона`, Серия, `Номер паспорта`FROM users", db.getConnection());

            // Создание адаптера данных и заполнение DataTable
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Создание объекта Excel
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Add();
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

            // Запись заголовков столбцов в Excel
            int columnIndex = 1;
            foreach (DataColumn column in dataTable.Columns)
            {
                worksheet.Cells[1, columnIndex] = column.ColumnName;
                columnIndex++;
            }

            // Запись данных в Excel
            int rowIndex = 2;
            foreach (DataRow row in dataTable.Rows)
            {
                columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    worksheet.Cells[rowIndex, columnIndex] = row[column.ColumnName].ToString();
                    columnIndex++;
                }
                rowIndex++;
            }

            // Сохранение Excel-файла и закрытие объектов
            workbook.SaveAs("D:\\ycheba\\Диплом\\withDBSql\\withDBSql\\bin\\Debug\\AllDbExcel" + DateTime.Now.ToString("yyyyMMdd HHmmss "));
            workbook.Close();
            MessageBox.Show("Файл создан!");
            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excel);
            db.closeConnection();
        }
    }
}
