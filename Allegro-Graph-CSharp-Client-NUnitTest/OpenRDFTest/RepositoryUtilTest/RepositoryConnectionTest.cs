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
    public class RepositoryConnectionTest
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
                        string.Format("context{0}", id));
            }
            else
            {
                return new Statement(string.Format("<http://example.com/article-{0}>", id),
                            "<http://www.w3.org/2000/01/rdf-schema#label>",
                            string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", id));
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
        }

        [Test]
        public void ClearTripleStore()
        {
            repoConn.Clear();
            Assert.True(repoConn.GetSize() == 0);
            Assert.True(repoConn.IsEmpty());
        }

        [Test]
        public void TestSize()
        {
            TestAdd();
            Assert.True(repoConn.GetSize() == 10);
        }

        [Test]
        public void TestAdd()
        {
            repoConn.Clear();
            for (int i = 0; i < 10; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            Assert.True(repoConn.GetSize() == 10);
        }
        [Test]
        public void TestAddWithContext()
        {
            repoConn.Clear();
            for (int i = 0; i < 10; ++i)
                repoConn.AddStatement(CreateSampleStatement(i,true));
            Assert.True(repoConn.GetSize() == 10);
        }

        [Test]
        public void TestNamespace()
        {
            repoConn.SetNamespace("ns1", "http://example.com");
            List<Namespace> list = repoConn.GetNamespaces();
            Console.WriteLine("list.size() = " + list.Count);
            repoConn.RemoveNamespace("ns1");
            repoConn.ClearNamespace();
        }

        [Test]
        public void TestTransaction()
        {
            int bn = 10;
            repoConn.Clear();
            repoConn.OpenSession(repoConn.GetSpec());
            for (int i = 0; i < bn; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            repoConn.Rollback();
            for (int i = bn; i < bn * 2; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            repoConn.Commit();
            repoConn.CloseSession();

            Assert.AreEqual(repoConn.GetSize(), bn);
        }

        [Test]
        public void TestIndices()
        {
            string type = "spogi";
            repoConn.AddIndex(type);
            string[] indices = repoConn.ListIndices();
            Assert.True(indices.Any(e => e == type));
            indices = repoConn.ListValidIndices();
            Assert.True(indices.Any(e => e == type));
            repoConn.DropIndex(type);
        }

        [Test]
        public void ExportTest()
        {
            repoConn.Export();
        }
        [Test]
        public void TestAddStatement()
        {
            Console.WriteLine("Test AddStatement");
            string[] subjs = new string[] { "<http://example.com/article-996>", "<http://example.com/article-995>" };
            string[] preds = new string[] { "<http://www.w3.org/2000/01/rdf-schema#label>", "<http://www.w3.org/2000/01/rdf-schema#label>" };
            string[] objs = new string[] { "testObj1", "testObj2" };
            string[] context = new string[] { "context1" };
            for (int i = 0; i < 2; i++)
            {
                repoConn.AddStatement(subjs[i], preds[i], objs[i], context);
            }
        }
        [Test]
        public void TestGetStatement()
        {
            string[][] results = repoConn.GetStatements(null, null, null, null);
            for (int i = 0; i < results.Length; i++)
            {
                StringBuilder sb = new StringBuilder("Triple :\t");
                for (int j = 0; j < results[i].Length; j++)
                {
                    sb.Append(results[i][j]);
                    if (j != results[i].Length - 1)
                        sb.Append("\t");
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine(results[i].Length);
            }
        }
        [Test]
        public void TestGetStatementsById()
        {
            string[][] results = repoConn.GetStatementsById("22", true);
            for (int i = 0; i < results.Length; i++)
            {
                StringBuilder sb = new StringBuilder("Triple :\t");
                for (int j = 0; j < results[i].Length; j++)
                {
                    sb.Append(results[i][j]);
                    if (j != results[i].Length - 1)
                        sb.Append("\t");
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine(results[i].Length);
            }
        }

        [Test]
        public void TestGetStatementIDs()
        {
            string[] ids = repoConn.GetStatementIDs();
            foreach (string id in ids)
            {
                Console.WriteLine(id);
            }
        }
    }
}
