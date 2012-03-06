using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;
using System.Data;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    [TestFixture]
    class AGRepositoryTest
    {
        private AGServerInfo server;
        private AGCatalog catalog;
        private AGRepository repository;

        [Test]
        [TestFixtureSetUp]
        public void init()
        {
            //对于非root catalog，访问时要增加catalog的路径,是否增加个成员表示repository所在的catalog
            string baseUrl = "http://172.16.2.21:10035";
            string username = "chainyi";
            string password = "chainyi123";
            server = new AGServerInfo(baseUrl, username, password);
            catalog = new AGCatalog(server, "chainyi");
            string repositoryName = "CSharpClient";
            repository = new AGRepository(catalog, repositoryName);
            Console.WriteLine(repository.Url);
        }

        ///<summary>
        /// 测试构造函数以及同样的参数是否返回同样的对象
        ///<summary>
        [Test]
        public void SameObjectTest()
        {
            string repositoryName = "CSharpClient";
            AGRepository repository1 = new AGRepository(catalog, repositoryName);
            AGRepository repository2 = new AGRepository(catalog, repositoryName);
            Assert.AreNotSame(repository1, repository2);
        }

        /// <summary>
        /// 测试 GetSize()
        /// </summary>
        [Test]
        public void GetSizeTest()
        {
            int size = repository.GetSize(null);
            Console.Write(size);
            Assert.IsNotNull(size);
        }

        /// <summary>
        /// 测试 AddStatements()
        /// </summary>
        [Test]
        public void AddStatementsTest()
        {
            string[][] quads = new string[1][] {
                new string[4] { "<http://example/q?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/q?abc=1&def=2>" ,"<http://example/q?abc=1&def=2>" } };
            repository.AddStatements(quads);
        }

        /// <summary>
        /// 测试 AddStatements() 
        /// </summary>
        [Test]
        [ExpectedException(typeof(AGRequestException))]
        public void AddStatementsTest2()
        {
            string[][] quads = new string[1][] {
                new string[3] { "<http://example/q?abc=3&def=4>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/q?abc=3&def=4>" } };
            repository.AddStatements(quads);
        }

        /// <summary>
        /// 测试 DeleteMatchingStatements()
        /// </summary>
        [Test]
        public void DeleteMatchingStatementsTest()
        {
            string subj = "<http://example/q?abc=3&def=4>";
            string pred = "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>";
            string obj = "<http://example/q?abc=3&def=4>";
            string context = null;
            int result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Console.WriteLine(result);
        }
        /// <summary>
        /// 测试 DeleteMatchingStatements()
        /// </summary>
        [Test]
        public void DeleteMatchingStatementsTest2()
        {
            string subj = "<http://example/q?abc=1&def=2>";
            string pred = null;
            string obj = null;
            string context = null;
            int result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 测试 DeleteMatchingStatements()
        /// </summary>
        [Test]
        public void DeleteMatchingStatementsTest3()
        {
            string subj = null;
            string pred = "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>";
            string obj = null;
            string context = null;
            int result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 测试 DeleteMatchingStatements()
        /// </summary>
        [Test]
        public void DeleteMatchingStatementsTest4()
        {
            string subj = null;
            string pred = null;
            string obj = "<http://example/q?abc=1&def=2>";
            string context = null;
            int result = repository.DeleteMatchingStatements(subj, pred, obj, context);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 测试 DeleteStatements()
        /// </summary>
        [Test]
        public void DeleteStatementsTest()
        {
            string[][] quads = new string[1][] {
                new string[4] { "<http://example/q?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/q?abc=1&def=2>" ,"<http://example/q?abc=1&def=2>" } };
            //repository.AddStatements(quads);
            Console.WriteLine(repository.GetSize());
            repository.DeleteStatements(quads);
            Console.WriteLine(repository.GetSize());
        }

        ///<summary>
        ///EvalSPARQLQuery()测试
        ///</summary>
        [Test]
        public void EvalSPARQLQueryTest()
        {
            string query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            DataTable dt = repository.EvalSPARQLQuery(query);
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + "\t");
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    Console.Write(dr[dc] + " ");
                Console.WriteLine();
            }
        }

        ///<summary>
        ///EvalSPARQLQuery()测试
        ///</summary>
        [Test]
        public void EvalSPARQLQueryTest2()
        {
            string query = "select ?subj ?pred ?obj {?subj ?pre ?obj}";
            DataTable dt = repository.EvalSPARQLQuery(query);
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + "\t");
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    Console.Write(dr[dc] + " ");
                Console.WriteLine();
            }
        }

        ///<summary>
        ///EvalSPARQLQuery()测试
        ///</summary>
        [Test]
        public void EvalSPARQLQueryTest3()
        {
            string query = "SELECT ?obj WHERE{ ?subj  <http://example.org/pred>  ?obj  . FILTER regex(?obj, \"ga\", \"i\") }";
            //Console.WriteLine(query);
            DataTable dt = repository.EvalSPARQLQuery(query);
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + "\t");
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    Console.Write(dr[dc] + " ");
                Console.WriteLine();
            }
        }

        ///<summary>
        ///EvalSPARQLQuery()测试
        ///</summary>
        [Test]
        public void EvalSPARQLQueryTest4()
        {
            string query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            int returnNum = 2;
            DataTable dt = repository.EvalSPARQLQuery(query, "false", null, null, null, false, returnNum, -1);
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + "\t");
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    Console.Write(dr[dc] + " ");
                Console.WriteLine();
            }
        }

        ///<summary>
        ///GetStatements()测试
        ///</summary>
        [Test]
        public void GetStatementsTest()
        {
            string[] subj = new string[] { "<http://example.org/node>" };
            string[] pred = new string[] { "<http://example.org/pred>" };
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context);
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                    Console.Write(result[i][j]);
                Console.WriteLine();
            }           
        }

        ///<summary>
        ///GetStatements()测试
        ///</summary>
        [Test]
        public void GetStatementsTest2()
        {
            string[] subj = null;
            string[] pred = new string[] { "<http://example.org/pred>" };
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context);
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                    Console.Write(result[i][j]);
                Console.WriteLine();
            }
        }

        ///<summary>
        ///GetStatements()测试
        ///</summary>
        [Test]
        public void GetStatementsTest3()
        {
            string[] subj = null;
            string[] pred = null;
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context);
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                    Console.Write(result[i][j]);
                Console.WriteLine();
            }
        }

        ///<summary>
        ///GetStatements()测试
        ///</summary>
        [Test]
        public void GetStatementsTest4()
        {
            string[] subj = null;
            string[] pred = null;
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context,"false",2,-1);
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                    Console.Write(result[i][j]);
                Console.WriteLine();
            }
        }

        ///<summary>
        ///GetStatements()测试
        ///</summary>
        [Test]
        public void GetStatementsTest5()
        {
            string[] subj = new string[]{"<none sense>"};
            string[] pred = null;
            string[] obj = null;
            string[] context = null;
            string[][] result = repository.GetStatements(subj, pred, obj, context, "false", 2, -1);
            if (result.Length == 0)
            {
                Console.WriteLine("No result return");
            }
            else
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    for (int j = 0; j < result[i].Length; j++)
                        Console.Write(result[i][j]);
                    Console.WriteLine();
                }
            }
        }

        [Test]
        public void GetBlankNodesTest()
        {
            string[] results = repository.GetBlankNodes(3);
            if (results != null)
            {
                foreach (string result in results)
                {
                    Console.WriteLine(result);
                }
            }
        }

        [Test]
        public void ListNamespaces()
        {
            List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace> results = repository.ListNamespaces();
            foreach (Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace nspace in results)
            {
                Console.WriteLine(nspace.Prefix+"#"+nspace.NameSpace);
            }
        }


    }
}
