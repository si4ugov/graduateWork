using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace withDBSql
{
    internal class User
    {
        public string GroupName { get; set; }
        public int Count { get; set; }

        private int id;
        public int Id { get { return id; } set { id = value; } }
        private string name, secondName, surnameName, phone, email, pass, kurs, seria, nomerPas, rating;

        public string Name { get { return name; } set { name = value; } }
        public string SecondName { get { return secondName; } set { secondName = value; } }
        public string SurnameName { get { return surnameName; } set { surnameName = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Pass { get { return pass; } set { pass = value; } }
        public string Kurs { get { return kurs; } set { kurs = value; } }
        public string Seria { get { return seria; } set { seria = value; } }
        public string NomerPas { get { return nomerPas; } set { nomerPas = value; } }
        public string Rating { get { return rating; } set { rating = value; } }

        public User() { }

        public User(string name, string secondName, string surnameName, string phone, string email, string pass, string kurs, int id, string seria, string nomerPas, string rating)
        {
            
            this.name = name;
            this.secondName = secondName;
            this.surnameName = surnameName;
            this.phone = phone;
            this.email = email;
            this.pass = pass;
            this.kurs = kurs;
            this.rating = rating;
            this.id = id;
            this.seria = seria;
            this.nomerPas = nomerPas;
        }
    }
}
