using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using System.Data;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    [TestFixture]
    class AGRepositoryTest
    {
        private AGServerInfo server;
        private AGCatalog catalog;
        private AGRepository repository;
        string BaseUrl;
        string Username;
        string Password;
        string TestRepositoryName;
        Namespace TestNamespace;
        string TestIndexName;
        Statement statement;

        [TestFixtureSetUp]
        public void init()
        {
            BaseUrl = "http://172.16.2.21:10035";
            Username = "chainyi";
            Password = "chainyi123";
            server = new AGServerInfo(BaseUrl, Username, Password);
            catalog = new AGCatalog(server, "chainyi");
            TestRepositoryName = "TestCsharpclient";
            repository = new AGRepository(catalog, TestRepositoryName);
            TestNamespace = new Namespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            TestIndexName = "gospi";
            statement = new Statement("<http://example/test?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/test?abc=1&def=2>", "<http://example/test?client=Csharp>", "85");
        }

        [Test]
        public void TestSameObject()
        {
            AGRepository repository1 = new AGRepository(catalog, TestRepositoryName);
            AGRepository repository2 = new AGRepository(catalog, TestRepositoryName);
            Assert.AreNotSame(repository1, repository2);
        }

        /// <summary>
        /// Test GetSize()
        /// </summary>
        [Test]
        public void TestGetSize()
        {
            int size = repository.GetSize();
            Assert.Greater(size, 0);
        }

        /// <summary>
        /// Test AddStatements()
        /// </summary>
        [Test]
        public void TestAddStatements()
        {
            //TestCase-1

            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            int preSize = repository.GetSize();
            repository.AddStatements(quads);
            Assert.AreEqual(preSize + 1, repository.GetSize());

            //TestCase-2 
            statement.Subject = "<http://example/test?abc=3&def=4>";
            statement.Object = "<http://example/test?abc=3&def=4>";
            quads = new string[1][] {
                new string[3] {statement.Subject,statement.Predicate,statement.Object} };
            preSize = repository.GetSize();
            repository.AddStatements(quads);
            Assert.AreEqual(preSize + 1, repository.GetSize());
        }

        /// <summary>
        /// Test DeleteMatchingStatements()
        /// </summary>
        [Test]
        public void TestDeleteMatchingStatements()
        {
            //TestCase-1
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repository.AddStatements(quads);
            int result = repository.DeleteMatchingStatements(statement.Subject, statement.Predicate, statement.Object, statement.Context);
            Assert.GreaterOrEqual(result, 1);

            //TestCase-2
            string subj = statement.Subject;
            string pred = null;
            string obj = null;
            string context = null;
            repository.AddStatements(quads);
            result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result, 1);

            //TestCase-3
            subj = null;
            pred = statement.Predicate;
            obj = null;
            context = null;
            repository.AddStatements(quads);
            result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result, 1);

            //TestCase-4
            subj = null;
            pred = null;
            obj = statement.Object;
            context = null;
            repository.AddStatements(quads);
            result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result, 1);
        }

        /// <summary>
        /// Test DeleteStatements()
        /// </summary>
        [Test]
        public void TestDeleteStatements()
        {
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repository.AddStatements(quads);
            int preSize = repository.GetSize();
            repository.DeleteStatements(quads);
            Assert.Less(repository.GetSize(), preSize);
        }

        ///<summary>
        ///Test EvalSPARQLQuery()
        ///</summary>
        [Test]
        public void TestEvalSPARQLQuery()
        {
            //TestCase-1
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repository.AddStatements(quads);
            string query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            DataTable dt = repository.QueryResultToDataTable(repository.EvalSPARQLQuery(query));
            bool flag = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["subj"].ToString() == statement.Subject && dr["pred"].ToString() == statement.Predicate && dr["obj"].ToString() == statement.Object)
                {
                    flag = true;
                    break;
                }
            }
            Assert.IsTrue(flag);

            //TestCase-2
            query = "SELECT ?obj WHERE{ ?subj  <http://www.w3.org/1999/02/22-rdf-syntax-ns#value>  ?obj}";
            dt = repository.QueryResultToDataTable(repository.EvalSPARQLQuery(query));
            Assert.GreaterOrEqual(dt.Rows.Count, 1);

            //TestCase-3
            query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            int returnNum = 2;
            dt = repository.QueryResultToDataTable(repository.EvalSPARQLQuery(query, "false", null, null, null, false, returnNum, -1));
            Assert.AreEqual(dt.Rows.Count, returnNum);
        }

        ///<summary>
        /// Test GetStatements()
        ///</summary>
        [Test]
        public void TestGetStatements()
        {
            //TestCase-1
            string[][] quads = new string[1][] {
                new string[4] { statement.Subject,statement.Predicate,statement.Object,statement.Context} };
            repository.AddStatements(quads);
            string[] subj = new string[] { statement.Subject };
            string[] pred = new string[] { statement.Predicate };
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result.GetLength(0), 1);

            //TestCase-2
            subj = null;
            pred = new string[] { statement.Predicate };
            obj = null;
            context = null;
            result = repository.GetStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result.GetLength(0), 1);

            //TestCase-3
            subj = null;
            pred = null;
            obj = null;
            context = null;
            result = repository.GetStatements(subj, pred, obj, context);
            Assert.GreaterOrEqual(result.GetLength(0), 1);

            //TestCase-4
            subj = null;
            pred = null;
            obj = null;
            context = null;
            result = repository.GetStatements(subj, pred, obj, context, "false", 2, -1);
            Assert.AreEqual(result.GetLength(0), 2);
        }

        [Test]
        public void TestGetStatementsById()
        {
            string[][] results = repository.GetStatementsById("85", true);
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < results[0].Length; j++)
            {
                sb.Append(results[0][j]);
            }
            Assert.AreEqual(sb.ToString(), statement.ToString());
        }

        [Test]
        public void TestGetBlankNodes()
        {
            string[] results = repository.GetBlankNodes(3);
            Assert.AreEqual(results.Length, 3);
        }

        [Test]
        public void TestListNamespaces()
        {
            List<Namespace> results = repository.ListNamespaces();
            bool flag = false;
            foreach (Namespace ns in results)
            {
                if (ns.Prefix == TestNamespace.Prefix && ns.NameSpace == TestNamespace.NameSpace)
                {
                    flag = true;
                    break;
                }
            }
            Assert.IsTrue(flag);
        }

        [Test]
        public void TestListIndices()
        {
            string[] indexs = repository.ListIndices();
            Assert.Contains(TestIndexName, indexs);
        }

        
    }
}
