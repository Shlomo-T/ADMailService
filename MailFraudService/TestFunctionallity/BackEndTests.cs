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
            for(int i=0;i<4;i++)
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
                    for (int j = 0; j < a[0].sentMail.Count*0.6; j++)
                    {
                        table.Rows.Add("YES", a[0].sentMail[j].sentanceAVG, a[0].sentMail[j].wordAVG, a[0].sentMail[j].tokenRaitio, a[0].sentMail[j].subjectWordCount, a[0].sentMail[j].sentanceCount);
                        
                    }
                for (int j = 0; j < a[3].sentMail.Count * 0.6; j++)
                {
                    table.Rows.Add("NO", a[3].sentMail[j].sentanceAVG, a[3].sentMail[j].wordAVG, a[3].sentMail[j].tokenRaitio, a[3].sentMail[j].subjectWordCount, a[3].sentMail[j].sentanceCount);

                }
            }

                catch { }
            

            Classifier classifier = new Classifier();
            classifier.TrainClassifier(table);
            string d = classifier.Classify(new double[] { a[1].sentMail[7].sentanceAVG, a[1].sentMail[7].wordAVG, a[1].sentMail[7].tokenRaitio, a[1].sentMail[7].subjectWordCount, a[1].sentMail[7].sentanceCount });
            int b = 0;
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
