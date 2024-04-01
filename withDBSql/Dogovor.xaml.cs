using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    public partial class Dogovor : Window
    {
        
        public Dogovor()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Получаем введенный пользователем ID
            int userId = 0;
            if (!int.TryParse(txtUserId.Text.Trim(), out userId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID пользователя");
                return;
            }

            string name = "", SecondName="", SurnameName="", Kurs="", Phone="", Seria="", NomerPas="", email = "";

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("SELECT Имя, Фамилия, Отчество, Курс, `Номер телефона`, Серия, `Номер паспорта`, Email FROM users WHERE id=@id", db.getConnection());
            command.Parameters.AddWithValue("@id", userId);
            db.openConnection();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                name = reader.GetString(0);
                SecondName = reader.GetString(1);
                SurnameName = reader.GetString(2);
                Kurs = reader.GetString(3);
                Phone = reader.GetString(4);
                Seria = reader.GetString(5);
                NomerPas = reader.GetString(6);
                email = reader.GetString(7);
            }
            reader.Close();

            db.closeConnection();

            string fio = $"{SecondName} {name} {SurnameName}";
            var helper = new WordHelper("Dogovor.doc");

            var items = new Dictionary<string, string>
            {
                { "<DATE>", datePicker.Text },
                { "<SERIA>", Seria},
                { "<FIO>",  fio},
                { "<NOMER>", NomerPas},
                { "<KURS>", Kurs }
            };

            string fileName = helper.Process(items);
            sendMail(fio, email, fileName);
            MessageBox.Show("Договор создан");
            this.Close(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Отправка сообщеня 
        private void sendMail(string fio, string email, string fileName)
        {

            try
            {
                SmtpClient mySmtpClient = new SmtpClient("");

                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = true;
                mySmtpClient.EnableSsl = true;

                System.Net.NetworkCredential basicAuthenticationInfo = new
                System.Net.NetworkCredential("", "");
                mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                MailAddress from = new MailAddress("", "Центра дополнительного образования");
                MailAddress to = new MailAddress($"{email}", $"{fio}");
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);
                myMail.Attachments.Add(new Attachment($"{fileName}"));

                // add ReplyTo
                MailAddress replyTo = new MailAddress("");
                myMail.ReplyToList.Add(replyTo);
                        
                // set subject and encoding
                myMail.Subject = "Договор об оказании услуг ";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = $"<b>Прикладываем к сообщению договор об оказании услуг дополнительного образования</b><br>Данное сообщение создано автоматически, ответ на него не требуется.";
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
    }
}
