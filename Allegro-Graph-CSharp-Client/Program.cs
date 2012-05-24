using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Newtonsoft.Json;

using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client
{
    class Program
    {
        private static string CATALOG = "chainyi";
        private static string REPOSITORY = "TestCsharpclient";
        private static string HOST = "172.16.2.21";
        private static string USERNAME = "chainyi";
        private static string PASSWORD = "chainyi123";

        private static Statement CreateSampleStatement(int id)
        {
            return new Statement(string.Format("<http://example.com/article-{0}>", id),
                    "<http://www.w3.org/2000/01/rdf-schema#label>",
                    string.Format("\"999\"^^<http://www.w3.org/2001/XMLSchema#int>", id));
        }

        static void Main(string[] args)
        {
            /*AGServerInfo info = new AGServerInfo("http://172.16.2.21:10035", "chainyi", "chainyi123");
            AGClient.Mini.AGClient client = new AGClient.Mini.AGClient(info);
            Console.WriteLine("VERSION = " + client.GetVersion());
            foreach (string catalog in client.ListCatalogs())
            {
                Console.WriteLine(catalog);
            }
            Console.WriteLine();

            AGCatalog cat = new AGCatalog(info, "chainyi");
            AGRepository repo = cat.OpenRepository("CSharpClient");

            int size = repo.GetSize(null);
            Console.WriteLine(size);
            
            repo.AddStatements(new string[1][] { new string[3] { "<http://example.org/node>", "<http://example.org/pred>", "\"object-literal\"" }});
            Console.WriteLine(repo.GetSize(null));
            
            repo.DeleteMatchingStatements("<http://example.org/node>", null, null, null);
            Console.WriteLine(repo.GetSize(null));

            repo.AddStatements(new string[1][] { new string[3] { "<http://example.org/node>", "<http://example.org/pred>", "<http://example.org/object>" }});
            Console.WriteLine(repo.GetSize(null));
            repo.DeleteStatements(new string[1][] { new string[3] { "<http://example.org/node>", "<http://example.org/pred>", "<http://example.org/object>" } });
            Console.WriteLine(repo.GetSize(null));
            */
            
            /*
            string[][] quads = new string[1000][];
            int l = 0;
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 10; ++j)
                    for (int k = 0; k < 10; ++k)
                    {
                        string subj = "<http://example.org/node" + i + ">";
                        string pred = "<http://example.org/pred" + j + ">";
                        string obj = "<http://example.org/obj" + k + ">";
                        quads[l++] = new string[3] { subj, pred, obj };
                    }
            repo.AddStatements(quads);
            Console.WriteLine(repo.GetSize(null));

            repo.DeleteStatements(quads);
            Console.WriteLine(repo.GetSize(null));
            
            string[][] resarr = repo.GetStatements(new string[] { "<http://example.org/node0>", "<http://example.org/node1>" }, null, null, null);
            for (int i = 0; i < resarr.Length; ++i)
            {
                for (int j = 0; j < resarr[i].Length; ++j)
                    Console.Write(resarr[i][j] + " ");
                Console.WriteLine();
            }

            string query = "select ?s ?p ?o where { ?s ?p ?o . FILTER (?s = <http://example.org/node0>) }";
            Console.WriteLine("query = " + query);
            DataTable dt = repo.EvalSPARQLQuery(query);
            //Console.WriteLine(queryres);
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + "\t");
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    Console.Write(dr[dc] + " ");
                Console.WriteLine();
            }*/

            //AllegroGraphServer server = new AllegroGraphServer("172.16.2.21", 10035, "chainyi", "chainyi123");
            //Catalog ca = server.OpenCatalog("chainyi");
            //Repository re = ca.GetRepository("CSClient2");
            //RepositoryConnection conn = re.GetConnection();
            //Console.WriteLine("size=" + conn.GetSize());

            //string type = "spogi";
            //conn.AddIndex(type);
            //string[] indices = conn.ListIndices();
            //indices = conn.ListValidIndices();
            //conn.DropIndex(type);

            AllegroGraphServer server = new AllegroGraphServer(HOST, 10035, USERNAME, PASSWORD);
            Catalog cata = server.OpenCatalog(CATALOG);
            Repository repo = cata.GetRepository(REPOSITORY);
            RepositoryConnection repoConn = repo.GetConnection();
            //repoConn.OpenSession("<" + CATALOG + ":" + REPOSITORY + ">");
            //List<Namespace> spaces = repoConn.GetNamespaces();
            //Console.WriteLine(spaces.Count);
            //foreach (var space in spaces)
            //{
            //    Console.WriteLine(space.Prefix + "\t" + space.NameSpace);
            //}
            //Console.WriteLine(repoConn.GetNamespaces("csharptest2"));
            //repoConn.CloseSession();
            //string geoType = repoConn.SetSphericalGeoType(2);
            //Console.Write(geoType);
            repoConn.GetMiniRepository().DeleteStatementsById(new string[] {"272" });

        }
    }
}
