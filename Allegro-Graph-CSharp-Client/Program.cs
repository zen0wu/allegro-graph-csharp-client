using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Newtonsoft.Json;

using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AGServerInfo info = new AGServerInfo("http://172.16.2.21:10035", "chainyi", "chainyi123");
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
            /*
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
        }
    }
}
