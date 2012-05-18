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
        private AllegroGraphServer ags;
        private Catalog catalog;
        [Test]
        [TestFixtureSetUp]
        public void init()
        {
            ags = new AllegroGraphServer("172.16.2.21", 10035, "chainyi", "chainyi123");
            catalog = ags.OpenCatalog("chainyi");
            Console.WriteLine(ags.Url);
            Console.WriteLine(ags.Version);
            Console.WriteLine(catalog.GetName());
        }

        [Test]
        public void ListCatalogsTest()
        {

            string[] catalogs = ags.ListCatalogs();
            foreach (string cat in catalogs)
            {
                Console.WriteLine(cat);
            }
        }
        ///<summary>
        ///测试catalog类
        ///</summary>
        [Test]
        public void CreateRepositoryTest()
        {
            Console.WriteLine(catalog.Url);
            Console.WriteLine("create 'temp' repository");
            catalog.CreateRepository("temp");
        }
        [Test]
        public void DeleteRepositoryTest()
        {
            Console.WriteLine(catalog.Url);
            Console.WriteLine("delete 'temp' repository");
            catalog.DeleteRepository("temp");
        }
        [Test]
        public void ListRepositoriesTest()
        {
            Console.WriteLine(catalog.Url);
            string[] repos = catalog.ListRepositories();
            foreach (string repo in repos)
            {
                Console.WriteLine(repo);
            }
        }

        [Test]
        public void GetRepository_OpenTest()
        {
            Repository repository = catalog.GetRepository("temp", AccessVerb.OPEN);
        }

        [Test]
        public void GetRepository_CreateTest()
        {
            Repository repository = catalog.GetRepository("temp", AccessVerb.CREATE);
        }

        [Test]
        public void SetInitFileTest()
        {
            string content = "(<-- (after-after ?a ?b) (q- ?a !<http:after> ?x) (q- ?x !<http:after> ?b))";
            ags.SetInitFile(content, false);
        }
        [Test]
        public void GetInitFileTest()
        {
            Console.WriteLine(ags.GetInitFile());
        }
        [Test]
    }
}
