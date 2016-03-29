using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data;
using MySql.Data.MySqlClient;
using MLManager;
using Entities;
using System.Collections.Generic;

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
            InitializeHelper.LoadMessagePerUser(a[0]);
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
