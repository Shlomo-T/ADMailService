
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MLManager
{
    public static class InitializeHelper
    {
        public static List<User> LoadUsers()
        {
            var db = new DBDriver.DBConnect();
            var dt = db.Select("employeelist");
            List<User> UserList = new List<User>();
            for (int i=0; i < dt.Rows.Count;i++)
            {
                string fName = dt.Rows[i].ItemArray[1].ToString();
                string lName = dt.Rows[i].ItemArray[2].ToString();
                string mail = dt.Rows[i].ItemArray[3].ToString();
                int eid = Convert.ToInt32(dt.Rows[i].ItemArray[0].ToString());
                User user = new User(fName, lName, eid, mail);
                UserList.Add(user);
            }
            return UserList;

        }
    }
}
