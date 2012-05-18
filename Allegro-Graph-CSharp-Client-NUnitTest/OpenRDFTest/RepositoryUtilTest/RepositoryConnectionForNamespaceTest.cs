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
        //Repository repo;
        //RepositoryConnection repoConn;
        [Test]
        public void TestGetNamespace()
        {
            Namespace testNamespace = new Namespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            List<Namespace> results = repoConn.GetNamespaces();
            bool flag = false;
            foreach (Namespace ns in results)
            {
                if (ns.Prefix == testNamespace.Prefix && ns.NameSpace == testNamespace.NameSpace)
                {
                    flag = true;
                    break;
                }
            }
            Assert.IsTrue(flag);
        }

        [Test]
        public void TestSetRemoveNamespace()
        {
            repoConn.OpenSession(repoConn.GetSpec());
            int preSize = repoConn.GetNamespaces().Count();

            repoConn.SetNamespace("csharptest2", "http://www.csharpexample2.com/");
            Assert.AreEqual(repoConn.GetNamespaces().Count(), preSize + 1);

            repoConn.RemoveNamespace("csharptest2");
            Assert.AreEqual(repoConn.GetNamespaces().Count(), preSize );

            repoConn.CloseSession();
            //AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            //Catalog cata = server.OpenCatalog(CATALOG);
            //Repository repo = cata.GetRepository(REPOSITORY);
            //RepositoryConnection repoConn = repo.GetConnection();
            ////repoConn.OpenSession("<" + CATALOG + ":" + REPOSITORY + ">");
            //List<Namespace> spaces = repoConn.GetNamespaces();
            //Console.WriteLine(spaces.Count);
            //foreach (var space in spaces)
            //{
            //    Console.WriteLine(space.Prefix + "\t" + space.NameSpace);
            //}
            //Console.WriteLine(repoConn.GetNamespaces("csharptest2"));
            //repoConn.CloseSession();
        }

        
        [Test]
        public void TestClearNamespace()
        {
            repoConn.ClearNamespace();
            int defaultSize = repoConn.GetNamespaces().Count();
            repoConn.SetNamespace("ns1", "http://example.com/1");
            repoConn.SetNamespace("ns2", "http://example.com/2");
            repoConn.ClearNamespace();
            Assert.AreEqual(defaultSize, repoConn.GetNamespaces().Count());
        }
    }
}
