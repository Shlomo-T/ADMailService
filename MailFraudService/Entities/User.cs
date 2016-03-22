using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public int eid { get; set; }
        public string email { get; set; }
        public List<Message> sentMail { get; set; }

        public User(string first, string last, int id, string mail)
        {
            this.fName = first;
            this.lName = last;
            this.eid = id;
            this.email = mail;
            this.sentMail = new List<Message>();
        }


    }
}
