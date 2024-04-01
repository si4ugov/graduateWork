using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace withDBSql
{
    internal class Admin
    {
        private string phone = "123456789", pass = "12345";

        public Admin() { }

        public string Phone { get { return phone; } }
        public string Pass { get { return pass; } }
    }
}
