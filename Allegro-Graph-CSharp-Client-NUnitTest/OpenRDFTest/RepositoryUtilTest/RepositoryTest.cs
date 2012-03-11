using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    public class RepositoryTest
    {
        private static string CATALOG = "chainyi";
        private static string REPOSITORY = "CSharpClient";
        private static string HOST = "172.16.2.21";
        private static string USERNAME = "chainyi";
        private static string PASSWORD = "chainyi123";

        Repository repo;

        [TestFixtureSetUp]
        [Test]
        public void SetUp()
        {
            AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            Catalog cata = server.OpenCatalog(CATALOG);
            repo = cata.GetRepository(REPOSITORY);
        }

        [Test]
        public void TestGetDatabaseName()
        {
            Assert.True(repo.GetDatabaseName() == REPOSITORY);
        }

        [Test]
        public void TestGetSpec()
        {
            Assert.True(repo.GetSpec() == "<chainyi:CSharpClient>", repo.GetSpec());
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            repo.ShutDown();
        }
    }
}
