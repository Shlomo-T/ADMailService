using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TestFunctionallity
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //DBDriver.DBManager mng = new DBDriver.DBManager();
            //mng.DBConnect();
            //var c= mng.DBExecuteRecordsetReturnOneField("select * from enron.message", false,"mid");
            //string stm = "SELECT * FROM message";
            //mng.DBExecuteRecordset(stm,false);
            var db = new DBDriver.DBConnect();
            var c=db.Count("employeelist");
            var dt = db.Select("employeelist");
            Assert.AreEqual(c, 151);
        }
    }
}
