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
    public partial class RepositoryConnectionTest
    {
        private static string CATALOG = "chainyi";
        private static string REPOSITORY = "TestCsharpclient";
        private static string HOST = "172.16.2.21";
        private static string USERNAME = "chainyi";
        private static string PASSWORD = "chainyi123";

        Repository repo;
        RepositoryConnection repoConn;

        private Statement CreateSampleStatement(int id, bool withContext = false)
        {
            if (withContext)
            {
                return new Statement(string.Format("<http://example.com/article-{0}>", id),
                        "<http://www.w3.org/2000/01/rdf-schema#label>",
                        string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", id),
                        string.Format("\"context{0}\"", id));
            }
            else
            {
                return new Statement(string.Format("<http://example.com/article-{0}>", id),
                            "<http://www.w3.org/2000/01/rdf-schema#label>",
                             //string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", id));
                            string.Format("\"obj{0}\"", id));
            }
        }

        [TestFixtureSetUp]
        [Test]
        public void SetUp()
        {
            AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            Catalog cata = server.OpenCatalog(CATALOG);
            repo = cata.GetRepository(REPOSITORY);
            repoConn = repo.GetConnection();
            //Console.WriteLine(repo.Url);
        }

        [Test]
        public void TestClearTripleStore()
        {
            repoConn.Clear();
            Assert.AreEqual(repoConn.GetSize(), 0);
            Assert.True(repoConn.IsEmpty());
        }

        [Test]
        public void TestGetSize()
        {
            repoConn.Clear();
            TestAdd();
            Assert.AreEqual(repoConn.GetSize(), 10);
        }

        [Test]
        public void TestAdd()
        {
            repoConn.Clear();
            for (int i = 0; i < 10; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            Assert.AreEqual(repoConn.GetSize(), 10);
        }
        [Test]
        public void TestAddWithContext()
        {
            repoConn.Clear();
            for (int i = 10; i < 20; ++i)
                repoConn.AddStatement(CreateSampleStatement(i, true));
            Assert.AreEqual(repoConn.GetSize(), 10);
        }        

       

        //[Test]
        //public void ExportTest()
        //{
        //    repoConn.Export();
        //}
        [Test]
        public void TestAddStatement()
        {
            repoConn.Clear();
            int preSize = repoConn.GetSize();
            string[] subjs = new string[] { "<http://example.com/article-996>", "<http://example.com/article-995>" };
            string[] preds = new string[] { "<http://www.w3.org/2000/01/rdf-schema#label>", "<http://www.w3.org/2000/01/rdf-schema#label>" };
            string[] objs = new string[] { string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", 100), string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", 200) };
            string[] context = new string[] { "\"context1\"","\"context2\"" };
            for (int i = 0; i < 2; i++)
            {
                repoConn.AddStatement(subjs[i], preds[i], objs[i],context[i]);
            }
            Assert.AreEqual(repoConn.GetSize(), preSize + 2);
        }
        [Test]
        public void TestGetStatement()
        {
            string[][] results = repoConn.GetStatements(null, null, null, null);
            Assert.Greater(results[0].Length, 0);
        }

        [Test]
        public void TestGetStatementsById()
        {
            string[][] results = repoConn.GetStatementsById("22", true);
            Assert.Greater(results[0].Length, 0);
        }

        [Test]
        public void TestGetStatementIDs()
        {
            string[] ids = repoConn.GetStatementIDs();
            Assert.Greater(ids.Length, 0);
        }

        [Test]
        public void TestGetStatementIDsWithParams()
        {
            string[] ids = repoConn.GetStatementIDs(new string[] { "<http://example.com/article-3>" }, null, null, null);
            Assert.Greater(ids.Length, 0);
        }
    }
}
