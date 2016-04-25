using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MLManager;

namespace TestFunctionallity
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestByEmail()
        {
            BayesClassifier.Classifier m_Classifier = new BayesClassifier.Classifier();

            List<User> UsersList = InitializeHelper.LoadUsers();
            List<int> goodUserIndex = new List<int>();
            double totalAnswer = 0;
            int temp = 0;

            for (int i = 50; i < 100; i++)
            {
                try
                {
                    InitializeHelper.LoadMessagePerUser(UsersList[i]);
                    if (UsersList[i].sentMail.Count > 100)
                        goodUserIndex.Add(i);
                }

                catch { }
            }

           /* var user = UsersList.First();
            for(int j=0;j<user.sentMail.Count*0.9;j++)
            {
                //TextAnalysisHelper.
                string testOther = user.sentMail[j].Body;

                // convert string to stream
                byte[] byteArr = Encoding.UTF8.GetBytes(testOther);
                MemoryStream streamer = new MemoryStream(byteArr);


                // convert stream to string
                StreamReader r = new StreamReader(streamer);
                m_Classifier.TeachCategory(user.email, r);

            }*/

            for (int i = 50; i < 100; i++)
            {
                if (goodUserIndex.Contains(i))
                {
                    var badUser = UsersList[i];
                    for (int j = 0; j < badUser.sentMail.Count*0.9; j++)
                    {
                        string testOther = badUser.sentMail[j].Body;

                        // convert string to stream
                        byte[] byteArr = Encoding.UTF8.GetBytes(testOther);
                        MemoryStream streamer = new MemoryStream(byteArr);


                        // convert stream to string
                        StreamReader r = new StreamReader(streamer);
                        m_Classifier.TeachCategory(badUser.email, r);

                    }
                }
            }
            var userTest = UsersList[66];
            string test = userTest.sentMail[444].Body;

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(test);
            MemoryStream stream = new MemoryStream(byteArray);


            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            Dictionary<string, double> score = m_Classifier.Classify(reader);
            var maxScore = score.Values.Max();
            string name;
            foreach (var key in score.Keys)
            {
                if (score[key] == maxScore)
                {
                    name = key;
                }
            }
            string text = reader.ReadToEnd();

        }
    }
}
