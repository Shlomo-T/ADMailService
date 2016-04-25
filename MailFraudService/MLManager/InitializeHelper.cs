
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

        public static void LoadMessagePerUser(User user)
        {
            var db = new DBDriver.DBConnect();
            string sender = "'" + user.email + "'";
            var dt = db.Select("message where sender=" + sender);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string body = dt.Rows[i].ItemArray[5].ToString();
                string subject = dt.Rows[i].ItemArray[4].ToString();
                string date = dt.Rows[i].ItemArray[2].ToString();

                List<string> bodyAfterSplit = TextAnalysisHelper.SplitByDot(body);

                List<List<string>> wordsPerSent = TextAnalysisHelper.WordsPerSentence(bodyAfterSplit);

                double sentAVG = TextAnalysisHelper.GetSentenceAVG(wordsPerSent);

                Dictionary<string, int> wordsDict = TextAnalysisHelper.CountWords(wordsPerSent);

                double wordAVG = TextAnalysisHelper.GetWordAVG(wordsDict);
                double token = TextAnalysisHelper.GetTokenRatio(wordsDict);
                double subjectWordCount = subject.Split(' ').Length;
                int mistakeCount = 0;

                Message msg = new Message(sentAVG, wordAVG, token, DateTime.Parse(date), subjectWordCount, bodyAfterSplit.Count,mistakeCount,body);
                user.sentMail.Add(msg);

            }

        }
    }
}
