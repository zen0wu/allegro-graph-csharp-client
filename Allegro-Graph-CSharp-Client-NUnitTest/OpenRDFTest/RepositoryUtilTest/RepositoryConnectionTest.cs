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
        Statement statement;

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
            statement = new Statement("<http://example/test?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/test?abc=1&def=2>", "<http://example/test?client=Csharp>", "85");

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
        public void TestIsEmpty()
        {
            repoConn.Clear();
            Assert.IsTrue(repoConn.IsEmpty());
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

        [Test]
        public void TestGetContextIDs()
        {
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repoConn.AddTriples(quads);
            string[] contexts = repoConn.GetContextIDs();
            Assert.Contains("<http://example/test?client=Csharp>", contexts);
            repoConn.RemoveQuads(quads);
        }

        [Test]
        public void TestAddTriple()
        {
            repoConn.Clear();
            int preSize = repoConn.GetSize();
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repoConn.AddTriples(quads);
            Assert.AreEqual(repoConn.GetSize(), preSize + 1);
        }

        [Test]
        public void TestAddTriples()
        {
            repoConn.Clear();
            int preSize = repoConn.GetSize();
            string[] subjs = new string[] { "<http://example.com/article-996>", "<http://example.com/article-995>" };
            string[] preds = new string[] { "<http://www.w3.org/2000/01/rdf-schema#label>", "<http://www.w3.org/2000/01/rdf-schema#label>" };
            string[] objs = new string[] { string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", 100), string.Format("\"999{0}\"^^<http://www.w3.org/2001/XMLSchema#int>", 200) };
            string[] context = new string[] { "\"context1\"", "\"context2\"" };
            for (int i = 0; i < 2; i++)
            {
                repoConn.AddTriple(subjs[i], preds[i], objs[i], context[i]);
            }
            Assert.AreEqual(repoConn.GetSize(), preSize + 2);
        }

        [Test]
        public void TestRemoveTriples()
        {
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repoConn.AddTriples(quads);
            int preSize = repoConn.GetSize();
            repoConn.RemoveTriples(statement.Subject, statement.Predicate, statement.Object);
            Assert.Less(repoConn.GetSize(), preSize);
        }

        [Test]
        public void TestRemoveQuadsByID()
        {
            string[] ids = repoConn.GetStatementIDs();
            repoConn.RemoveQuadsByID(new string[] { ids[0], ids[1] });
            Assert.AreEqual(ids.Length - 2, repoConn.GetStatementIDs().Length);
        }

        [Test]
        public void TestRemoveQuads()
        {
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repoConn.AddTriples(quads);
            int preSize = repoConn.GetSize();
            repoConn.RemoveQuads(quads);
            Assert.Less(repoConn.GetSize(), preSize);
        }

        [Test]
        public void TestRemoveDuplicateStatements()
        {
            Statement statement = new Statement("<http://example/test?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/test?abc=1&def=9>", "<http://example/test?client=Csharp>", "85");
            repoConn.RemoveTriples(null, null, statement.Object, null);
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repoConn.AddTriples(quads);
            repoConn.AddTriples(quads);
            int preSize = repoConn.GetSize();
            repoConn.RemoveDuplicateStatements();
            Assert.AreEqual(preSize - 1, repoConn.GetSize());
        }

        [Test]
        public void TestGetBlankNodes()
        {
            string[] results = repoConn.GetBlankNodes(3);
            Assert.AreEqual(results.Length, 3);
        }

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
            string[] ids = repoConn.GetStatementIDs();
            string[][] results = repoConn.GetStatementsById(ids[0], true);
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
