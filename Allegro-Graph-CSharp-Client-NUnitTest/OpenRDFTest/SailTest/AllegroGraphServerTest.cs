using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.SailTest
{

    [TestFixture]
    class AllegroGraphServerTest
    {
        private string buildDate;
        private string version;
        private string testCatalogName;
        private string testRepoName;
        private string userName;
        private string password;
        private AllegroGraphServer ags;
        private Catalog catalog;

        [TestFixtureSetUp]
        public void init()
        {
            userName = "chainyi";
            password = "chainyi123";
            testCatalogName = "chainyi";
            testRepoName = "TestCsharpclient";
            ags = new AllegroGraphServer("172.16.2.21", 10035, userName, password);
            catalog = ags.OpenCatalog(testCatalogName);
            buildDate = "February 13, 2012 16:42:07 GMT-0800";
            version = "4.5";
        }

        [Test]
        public void TestProperty()
        {
            Assert.AreEqual(ags.Url, "http://172.16.2.21:10035");
            Assert.AreEqual(ags.Version, version);
            Assert.AreEqual(catalog.GetName(), testCatalogName);
            Assert.AreEqual(ags.Date, buildDate);
        }

        [Test]
        public void TestListCatalogs()
        {
            string[] catalogs = ags.ListCatalogs();
            Assert.Contains(testCatalogName, catalogs);
        }

        [Test]
        public void TestCreateDeleteCatalog()
        {
            string catalogTest = "OnlyTest";
            int preLength = ags.ListCatalogs().Length;
            ags.CreateCatalog(catalogTest);
            Assert.AreEqual(preLength + 1, ags.ListCatalogs().Length);

            ags.DeleteCatalog(catalogTest);
            Assert.AreEqual(preLength, ags.ListCatalogs().Length);
        }

        [Test]
        public void TestOpenCatalog()
        {
            bool result = ags.OpenCatalog(testCatalogName) is Catalog;
            Assert.IsTrue(result);
        }

        //test catalog class
        [Test]
        public void TestGetSesameProtocolVersion()
        {
            Assert.AreEqual(catalog.GetSesameProtocolVersion(), "4");
        }

        [Test]
        public void TestGetName()
        {
            Assert.AreEqual(catalog.GetName(), testCatalogName);
        }

        [Test]
        public void TestListRepositories()
        {
            string[] repos = catalog.ListRepositories();
            Assert.Contains(testRepoName, repos);
        }

        [Test]
        public void TestCreateDeleteRepository()
        {
            string tempRepositoryName = "RepositoryForTest";
            string[] preRepostories = catalog.ListRepositories();
            catalog.CreateRepository(tempRepositoryName);
            string[] currentRepostories = catalog.ListRepositories();
            Assert.AreEqual(preRepostories.Length + 1, currentRepostories.Length);
            Assert.Contains(tempRepositoryName, currentRepostories);

            catalog.DeleteRepository(tempRepositoryName);
            currentRepostories = catalog.ListRepositories();
            Assert.AreEqual(preRepostories.Length, currentRepostories.Length);
            Assert.False(currentRepostories.Contains(tempRepositoryName));
        }
        
        [Test]
        public void TestGetRepository()
        {
            Repository repository = catalog.GetRepository(testRepoName, AccessVerb.OPEN);
            Assert.AreEqual(repository.GetDatabaseName(), testRepoName);
        }

        [Test]
        public void TestInitFile()
        {
            string content = "(<-- (after-after ?a ?b) (q- ?a !<http:after> ?x) (q- ?x !<http:after> ?b))";
            ags.SetInitFile(content, false);
            Assert.True(ags.GetInitFile().Contains(content));
        }
    }
}
