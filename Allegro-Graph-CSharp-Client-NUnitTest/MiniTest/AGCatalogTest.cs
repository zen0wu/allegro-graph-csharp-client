using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    /// <summary>
    /// Test class AGCatalog
    /// </summary>

    [TestFixture]
    class AGCatalogTest
    {
        private AGServerInfo server;
        private AGCatalog catalog;
        private string testCatalogName;
        private string testRepositoryName;
        private string tempRepositoryName;

        [TestFixtureSetUp]
        public void Init()
        {
            string baseUrl = "http://172.16.2.21:10035";
            string username = "chainyi";
            string password = "chainyi123";
            testCatalogName = "chainyi";
            testRepositoryName = "TestCsharpclient";
            tempRepositoryName = "TempRepositoryForCSharpClientTest";
            server = new AGServerInfo(baseUrl, username, password);
            catalog = new AGCatalog(server, testCatalogName);
        }

        [Test]
        public void TestSameObject()
        {
            AGCatalog catalog1 = new AGCatalog(server, testCatalogName);
            AGCatalog catalog2 = new AGCatalog(server, testCatalogName);
            Assert.AreNotSame(catalog1, catalog2);
        }

        /// <summary>
        /// Test OpenRepository()
        /// </summary>
        [Test]
        public void TestOpenRepository()
        {
            bool result = catalog.OpenRepository(testCatalogName) is AGRepository;
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test CreateRepository()
        /// </summary>
        [Test]
        public void TestCreateRepository()
        {
            string[] preRepostories = catalog.ListRepositories();
            catalog.CreateRepository(tempRepositoryName);
            string[] currentRepostories = catalog.ListRepositories();
            Assert.AreEqual(preRepostories.Length+1, currentRepostories.Length);
            Assert.Contains(tempRepositoryName, currentRepostories);
        }

        /// <summary>
        /// Test DeleteRepository()
        /// </summary>
        [Test]
        public void TestDeleteRepository()
        {
            string[] preRepostories = catalog.ListRepositories();
            catalog.DeleteRepository(tempRepositoryName);
            string[] currentRepostories = catalog.ListRepositories();
            Assert.AreEqual(preRepostories.Length-1, currentRepostories.Length);
            Assert.True(!currentRepostories.Contains(tempRepositoryName));
        }

        /// <summary>
        /// Test ListRepositories()
        /// </summary>
        [Test]
        public void TestListRepositories()
        {
            string[] repos = catalog.ListRepositories();
            Assert.Contains(testRepositoryName, repos);
        }
    }
}
