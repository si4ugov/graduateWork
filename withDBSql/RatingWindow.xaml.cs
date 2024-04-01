using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MailMessage = System.Net.Mail.MailMessage;
using Window = System.Windows.Window;
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace withDBSql
{
    public partial class RatingWindow : Window
    {
        public RatingWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string kurs = comboBox_Courses.Text.Trim();
            var selectedRow = dataGrid_Groups.SelectedItem as DataRowView;
            DB db = new DB();

            MySqlCommand command = new MySqlCommand("SELECT id, Имя, Фамилия, Отчество, Курс, Оценка FROM users WHERE `Курс` = @kurs", db.getConnection());
            command.Parameters.Add("@kurs", MySqlDbType.VarChar).Value = kurs;

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
                    Rating = reader.GetString(5)
                };
                dataList.Add(data);
            }
            reader.Close();

            dataGrid_Groups.ItemsSource = dataList;
            db.closeConnection();

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;

            foreach (var item in dataGrid_Groups.Items)
            {
                var user = (User)item;
                var rating = user.Rating;
                var id = user.Id;

                DB db = new DB();
                MySqlCommand command = new MySqlCommand("UPDATE users SET Оценка = @Rating WHERE id = @Id", db.getConnection());
                command.Parameters.Add("@Rating", MySqlDbType.VarChar).Value = rating;
                command.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;

                if (rating != "Неудовлетворительно" && rating != null)
                {
                    Create_Sert(id);
                }

                try
                {
                    db.openConnection();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    db.closeConnection();
                }
            }

            if (success)
            {
                MessageBox.Show("Оценка сохранена!");
            }
            else
            {
                MessageBox.Show("Не удалось сохранить оценку.");
            }
        }

        private void Create_Sert(int userId)
        {
          
            DB db = new DB();
            MySqlCommand command = new MySqlCommand("SELECT Имя, Фамилия, Отчество, Курс, Email FROM users WHERE id=@id", db.getConnection());
            db.openConnection();
            command.Parameters.Add("@Id", MySqlDbType.Int32).Value = userId;
            MySqlDataReader sqlReader = command.ExecuteReader();
            sqlReader.Read();

            var helper = new SertHelper("OfficialSertDoc.docx");

            var items = new Dictionary<string, string>
            {
                { "<NAME>", sqlReader["Имя"].ToString()},
                { "<SECNAME>", sqlReader["Фамилия"].ToString()},
                { "<SURENAME>", sqlReader["Отчество"].ToString()},
                { "<KURS>", sqlReader["Курс"].ToString()},
                { "<DATE>", DateTime.Now.ToString("dd/MM/yyyy")}
            };

            string name = sqlReader["Имя"].ToString();
            string email = sqlReader["Email"].ToString();
            string fileName = helper.Process(items);

            sendMail(name, fileName, email);
            db.closeConnection();
        }

        private void sendMail(string name, string fileName, string email)
        {

            try
            {

                SmtpClient mySmtpClient = new SmtpClient("smtp.mail.ru");

                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = true;
                mySmtpClient.EnableSsl = true;

                System.Net.NetworkCredential basicAuthenticationInfo = new
                System.Net.NetworkCredential("", "");
                mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                MailAddress from = new MailAddress("", "Центра дополнительного образования");
                MailAddress to = new MailAddress($"{email}", $"{name}");
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                myMail.Attachments.Add(new Attachment($"{fileName}"));

                // add ReplyTo
                MailAddress replyTo = new MailAddress("");
                myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                myMail.Subject = "Поздравляем с завершением курса";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = $"<b>Прикладываем к сообщению сертификат о повышении квалификации</b><br>Данное сообщение создано автоматически, ответ на него не требуется.";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;

                // text or html
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

            private void Cancel_Click(object sender, RoutedEventArgs e)
            {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Hide();
            }

        //Выгрузка в Excel
        private void Creat_Excel(object sender, RoutedEventArgs e)
        {
            string kurs = comboBox_Courses.Text.Trim();

            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT id, Имя, Фамилия, Отчество, Курс, Оценка FROM users WHERE `Курс` = @kurs", db.getConnection());
            command.Parameters.Add("@kurs", MySqlDbType.VarChar).Value = kurs;

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
            workbook.SaveAs("D:\\ycheba\\Диплом\\withDBSql\\withDBSql\\bin\\Debug\\RatingExcel" + DateTime.Now.ToString("yyyyMMdd HHmmss "));
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
