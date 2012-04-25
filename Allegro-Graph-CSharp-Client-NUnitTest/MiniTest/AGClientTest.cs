using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    /// <summary>
    /// Test AGClient
    /// </summary>
    [TestFixture]
    class AGClientTest
    {
        private AGServerInfo server;
        private AGClient agClient;
        private string buildDate; 
        private string version;
        private string testCatalogName;
        private string userName;
        private string password;

        [TestFixtureSetUp]
        public void Init()
        {
            userName = "chainyi";
            password = "chainyi123";
            server = new AGServerInfo("http://172.16.2.21:10035", userName, password);
            agClient = new AGClient(server);
            buildDate = "February 13, 2012 16:42:07 GMT-0800";
            version = "4.5";
            testCatalogName = "chainyi";
        }
       
        [Test]
        public void TestSameObject()
        {
            AGClient agClient1 = new AGClient(server);
            AGClient agClient2 = new AGClient(server);
            Assert.AreNotSame(agClient1, agClient2);
        }

        /// <summary>
        /// Test GetVersion()
        /// </summary>
        [Test]
        public void TestGetVersion()
        {
            string version = agClient.GetVersion();
            Assert.AreEqual(version,this.version);
        }

        /// <summary>
        /// Test ListCatalogs()
        /// </summary>
        [Test]
        public void TestListCatalogs()
        {
            string[] catalogs = agClient.ListCatalogs();
            Assert.IsNotEmpty(catalogs);
            Assert.Contains(testCatalogName, catalogs);
        }

        /// <summary>
        /// Test OpenCatalog()
        /// </summary>
        [Test]
        public void TestOpenCatalog()
        {
            string catalogName = "chainyi";
            bool result = agClient.OpenCatalog(catalogName) is AGCatalog;
            Assert.IsTrue(result);
        }

        [Test]
        public void TestOpenSession()
        {
            AGRepository result = agClient.OpenSession("<chainyi:CSharpClient>");
            Assert.AreEqual(userName, result.Username);
            Assert.AreEqual(password, result.Password);
            Assert.NotNull(result.Url);
        }

        [Test]
        public void TestGetBuiltDate()
        {
            Assert.AreEqual(agClient.GetBuiltDate(), this.buildDate);
        }

        [Test]
        //[Ignore("need administrator previlege")]
        public void TestCreateCatalog()
        {
            int preLength = agClient.ListCatalogs().Length;
            agClient.CreateCatalog("OnlyTest");
            Assert.AreEqual(preLength+1, agClient.ListCatalogs().Length);
        }

        [Test]
        //[Ignore("need administrator previlege")]
        public void TestDeleteCatalog()
        {
            int preLength = agClient.ListCatalogs().Length;
            agClient.DeleteCatalog("OnlyTest");
            Assert.AreEqual(preLength-1, agClient.ListCatalogs().Length);
        }  

        [Test]
        [Ignore("need administrator previlege")]
        public void TestReConfigure()
        {
            agClient.ReConfigure();
        }

        [Test]
        [Ignore("need administrator previlege")]
        public void TestReopenLog()
        {
            agClient.ReopenLog();
        }

        [Test]
        [Ignore("need administrator previlege")]
        public void TestGetInitFile()
        {
            agClient.GetInitFile();
        }
    }
}
