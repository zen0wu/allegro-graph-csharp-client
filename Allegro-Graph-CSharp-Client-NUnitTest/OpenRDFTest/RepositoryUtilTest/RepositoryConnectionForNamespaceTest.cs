using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    class RepositoryConnectionForNamespaceTest
    {
        private static string CATALOG = "chainyi";
        private static string REPOSITORY = "TestCsharpclient";
        private static string HOST = "172.16.2.21";
        private static string USERNAME = "chainyi";
        private static string PASSWORD = "chainyi123";

        Repository repo;
        RepositoryConnection repoConn;

        [TestFixtureSetUp]
        [Test]
        public void SetUp()
        {
            AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            Catalog cata = server.OpenCatalog(CATALOG);
            repo = cata.GetRepository(REPOSITORY);
            repoConn = repo.GetConnection();
        }

        [Test]
        public void TestSetNamespace()
        {

            //Console.WriteLine(repoConn.GetSpec());
            //repoConn.OpenSession(repoConn.GetSpec());
            //List<Namespace> list = repoConn.GetNamespaces();
            //int preSize = list.Count();
            //repoConn.SetNamespace("csharptest2", "http://www.csharpexample2.com/");
            //foreach (Namespace ns in list)
            //{
            //    Console.WriteLine(ns.ToString());
            //}
            //int currentSize = repoConn.GetNamespaces().Count();
            //Assert.AreEqual(currentSize, preSize + 1);
            //repoConn.RemoveNamespace("csharptest2");
            //repoConn.CloseSession();
            AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            Catalog cata = server.OpenCatalog(CATALOG);
            Repository repo = cata.GetRepository(REPOSITORY);
            RepositoryConnection repoConn = repo.GetConnection();
            //repoConn.OpenSession("<" + CATALOG + ":" + REPOSITORY + ">");
            List<Namespace> spaces = repoConn.GetNamespaces();
            Console.WriteLine(spaces.Count);
            foreach (var space in spaces)
            {
                Console.WriteLine(space.Prefix + "\t" + space.NameSpace);
            }
            Console.WriteLine(repoConn.GetNamespaces("csharptest2"));
            repoConn.CloseSession();
        }

        [Test]
        public void TestRemoveNamespace()
        {
            //repoConn.SetNamespace("ns1", "http://example.com");
            List<Namespace> list = repoConn.GetNamespaces();
            foreach (Namespace ns in list)
            {
                Console.WriteLine(ns.ToString());
            }

            //repoConn.RemoveNamespace("ns1");
            //repoConn.ClearNamespace();
        }
    }
}
