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
    }
}
