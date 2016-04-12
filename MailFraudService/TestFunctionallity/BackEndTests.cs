using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data;
using MySql.Data.MySqlClient;
using MLManager;
using Entities;
using System.Collections.Generic;
using System.Data;
using ProbabilityFunctions;

namespace TestFunctionallity
{
    [TestClass]
    public class BackEndTests
    {
        [TestMethod]
        public void TestDBConnections()
        {
            //DBDriver.DBManager mng = new DBDriver.DBManager();
            //mng.DBConnect();
            //var c= mng.DBExecuteRecordsetReturnOneField("select * from enron.message", false,"mid");
            //string stm = "SELECT * FROM message";
            //mng.DBExecuteRecordset(stm,false);
            List<User> a = InitializeHelper.LoadUsers();
            for(int i=0;i<20;i++)
            {
                try
                {
                    InitializeHelper.LoadMessagePerUser(a[i]);
                }

                catch { }
            }

            DataTable table = new DataTable();
            table.Columns.Add("Mail");
            table.Columns.Add("SentAve", typeof(double));
            table.Columns.Add("WordAVG", typeof(double));
            table.Columns.Add("Token", typeof(double));
            table.Columns.Add("Subject", typeof(double));
            table.Columns.Add("SentNumber", typeof(double));


            try
            {
                for (int j = 0; j < 600; j++)
                {
                    table.Rows.Add("YES", a[0].sentMail[j].sentanceAVG, a[0].sentMail[j].wordAVG, 
                        a[0].sentMail[j].tokenRaitio, a[0].sentMail[j].subjectWordCount, a[0].sentMail[j].sentanceCount);

                }

                for (int i = 1; i < 100; i++)
                {
                    for (int j = 0; j < a[i].sentMail.Count; j++)
                    {
                        if (a[i].sentMail.Count > 0)
                        {
                            table.Rows.Add("NO", a[i].sentMail[j].sentanceAVG, a[i].sentMail[j].wordAVG,
                                a[i].sentMail[j].tokenRaitio, a[i].sentMail[j].subjectWordCount, a[i].sentMail[j].sentanceCount);
                        }
                    }
                }

            }
            

                catch { }
            

            Classifier classifier = new Classifier();
            classifier.TrainClassifier(table);
            int count = 0;
            for (int i = 601; i < 1116; i++)
            {
                string d = classifier.Classify(new double[] { a[0].sentMail[i].sentanceAVG, a[0].sentMail[i].wordAVG,
                    a[0].sentMail[i].tokenRaitio, a[0].sentMail[i].subjectWordCount, a[0].sentMail[i].sentanceCount });
                if (d.Equals("YES"))
                {
                    count++;
                }
            }
            double b = (double)count /(double)516;
            int p = 0;
        }

        [TestMethod]
        public void TestSpellChecking()
        {
            string testStr= "My life hlding all down reealy reealy";
            var ECount= TextAnalysisHelper.CheckSpell(testStr);
            Assert.AreEqual(ECount, 3);
        }

        [TestMethod]
        public void PerTestYES()
        {
            List<User> UsersList = InitializeHelper.LoadUsers();
            List<int> goodUserIndex = new List<int>();
            double totalAnswer = 0;
            int temp = 0;

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    InitializeHelper.LoadMessagePerUser(UsersList[i]);
                    if(UsersList[i].sentMail.Count>100)
                        goodUserIndex.Add(i);
                }

                catch { }
            }

            foreach (int i in goodUserIndex)
            {
                DataTable table = new DataTable();
                table.Columns.Add("Mail");
                table.Columns.Add("SentAve", typeof(double));
                table.Columns.Add("WordAVG", typeof(double));
                table.Columns.Add("Token", typeof(double));
                table.Columns.Add("Subject", typeof(double));
                table.Columns.Add("SentNumber", typeof(double));
                int j = 0;

                for (j = 0; j < UsersList[i].sentMail.Count*0.9 ; j++)
                {
                    table.Rows.Add("YES", UsersList[i].sentMail[j].sentanceAVG, UsersList[i].sentMail[j].wordAVG,
                        UsersList[i].sentMail[j].tokenRaitio, UsersList[i].sentMail[j].subjectWordCount,
                        UsersList[i].sentMail[j].sentanceCount);
                }

                foreach (int k in goodUserIndex)
                {
                    if (k != i)
                    {
                        for (int z = 0; z < UsersList[k].sentMail.Count*0.1; z++)
                        {
                            table.Rows.Add("NO", UsersList[k].sentMail[z].sentanceAVG, UsersList[k].sentMail[z].wordAVG,
                                UsersList[k].sentMail[z].tokenRaitio, UsersList[k].sentMail[z].subjectWordCount,
                                UsersList[k].sentMail[z].sentanceCount);
                        }

                    }
                }

                Classifier classifier = new Classifier();
                classifier.TrainClassifier(table);

                double countYes = 0;
                for (int k = j; k < UsersList[i].sentMail.Count; k++)
                {
                    string yesOrNO = classifier.Classify(new double[] { UsersList[i].sentMail[k].sentanceAVG, UsersList[i].sentMail[k].wordAVG,
                    UsersList[i].sentMail[k].tokenRaitio, UsersList[i].sentMail[k].subjectWordCount, UsersList[i].sentMail[k].sentanceCount });

                    table.Rows.Add(yesOrNO, UsersList[i].sentMail[k].sentanceAVG, UsersList[i].sentMail[k].wordAVG,
                    UsersList[i].sentMail[k].tokenRaitio, UsersList[i].sentMail[k].subjectWordCount, UsersList[i].sentMail[k].sentanceCount);

                    classifier.DataSet.Tables.Remove(table);
                    classifier = new Classifier();
                    classifier.TrainClassifier(table);

                    if (yesOrNO.Equals("YES"))
                    {
                        countYes++;
                    }

                }
                double ans = countYes / (UsersList[i].sentMail.Count - j);
                if (ans > 0.7)
                {
                    temp++;
                }
                totalAnswer += ans;
            }

            totalAnswer /= 26;
            int c = 0;
        }



        [TestMethod]
        public void PerTestNO()
        {
            List<User> UsersList = InitializeHelper.LoadUsers();
            List<int> goodUserIndex = new List<int>();
            double totalAnswer = 0;
            int temp = 0;

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    InitializeHelper.LoadMessagePerUser(UsersList[i]);
                    if (UsersList[i].sentMail.Count > 100)
                        goodUserIndex.Add(i);
                }

                catch { }
            }

            foreach (int i in goodUserIndex)
            {
                DataTable table = new DataTable();
                table.Columns.Add("Mail");
                table.Columns.Add("SentAve", typeof(double));
                table.Columns.Add("WordAVG", typeof(double));
                table.Columns.Add("Token", typeof(double));
                table.Columns.Add("Subject", typeof(double));
                table.Columns.Add("SentNumber", typeof(double));


                for (int j = 0; j < UsersList[i].sentMail.Count * 0.9; j++)
                {
                    table.Rows.Add("YES", UsersList[i].sentMail[j].sentanceAVG, UsersList[i].sentMail[j].wordAVG,
                        UsersList[i].sentMail[j].tokenRaitio, UsersList[i].sentMail[j].subjectWordCount,
                        UsersList[i].sentMail[j].sentanceCount);
                }

                foreach (int k in goodUserIndex)
                {
                    if (k != i)
                    {
                        for (int z = 0; z < UsersList[k].sentMail.Count * 0.1; z++)
                        {
                            table.Rows.Add("NO", UsersList[k].sentMail[z].sentanceAVG, UsersList[k].sentMail[z].wordAVG,
                                UsersList[k].sentMail[z].tokenRaitio, UsersList[k].sentMail[z].subjectWordCount,
                                UsersList[k].sentMail[z].sentanceCount);
                        }

                    }
                }

                Classifier classifier = new Classifier();
                classifier.TrainClassifier(table);
                double countYes = 0;
                double aaa = 0;

                for (int j = 0; j < UsersList.Count; j++)
                {
                    for (int k = (int)(UsersList[j].sentMail.Count * 0.9); k < UsersList[j].sentMail.Count; k++)
                    {
                        if (i != j)
                        {
                            string yesOrNO = classifier.Classify(new double[] { UsersList[j].sentMail[k].sentanceAVG, UsersList[j].sentMail[k].wordAVG,
                            UsersList[j].sentMail[k].tokenRaitio, UsersList[j].sentMail[k].subjectWordCount, UsersList[j].sentMail[k].sentanceCount });


                            table.Rows.Add(yesOrNO, UsersList[j].sentMail[k].sentanceAVG, UsersList[j].sentMail[k].wordAVG,
                            UsersList[j].sentMail[k].tokenRaitio, UsersList[j].sentMail[k].subjectWordCount, UsersList[j].sentMail[k].sentanceCount);

                            classifier.DataSet.Tables.Remove(table);
                            classifier = new Classifier();
                            classifier.TrainClassifier(table);
                            aaa++;
                            if (yesOrNO.Equals("NO"))
                            {
                                countYes++;
                            }
                        }
                    }
                }
                double ans = countYes / (UsersList[i].sentMail.Count);
                double answer1 = countYes / aaa;
                if (ans > 0.7)
                {
                    temp++;
                }
                totalAnswer += ans;
            }

            totalAnswer /= 26;
            int c = 0;
        }
    }
}
