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
        private static string REPOSITORY = "CSharpClient";
        private static string HOST = "172.16.2.21";
        private static string USERNAME = "chainyi";
        private static string PASSWORD = "chainyi123";

        Repository repo;
        RepositoryConnection repoConn;

        private Statement CreateSampleStatement(int id)
        {
            return new Statement(string.Format("<http://example.com/article-{0}>", id),
                    "<http://www.w3.org/2000/01/rdf-schema#label>",
                    string.Format("\"999\"^^<http://www.w3.org/2001/XMLSchema#int>", id));
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
            repoConn.Clear();
            for (int i = 0; i < 10; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            Assert.True(repoConn.GetSize() == 10);
        }

        [Test]
        public void TestNamespace()
        {
            string prefix = "<http://example.com/";
            repoConn.SetNamespace(prefix, "ns1");
            List<Namespace> list = repoConn.GetNamespaces();
            Assert.True(list.Count == 1);
            Assert.AreEqual(list[0].Prefix, prefix);
            repoConn.RemoveNamespace(prefix);
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
            for (int i = bn; i < bn*2; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            repoConn.Commit();
            repoConn.CloseSession();

            Assert.AreEqual(repoConn.GetSize(), bn);
        }

        [Test]
        public void TestIndices()
        {
            string type = "spo";
            repoConn.AddIndex(type);
            string[] indices = repoConn.ListIndices();
            Assert.True(indices.Length == 1);
            Assert.True(indices[0] == type);
            indices = repoConn.ListValidIndices();
            Assert.True(indices.Length == 1);
            Assert.True(indices[0] == type);
            repoConn.DropIndex(type);
        }

        [Test]
        public void ExportTest()
        {
            repoConn.Export();
        }
    }
}
