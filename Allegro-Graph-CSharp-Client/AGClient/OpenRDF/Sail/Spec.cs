using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail
{
    public class Spec
    {
        public static string Local(string name, string catalog = null)
        {
            if (catalog != null)
            {
                return string.Format("<{0}:{1}>", catalog, name);
            }
            else
            {
                return string.Format("<{0}>", name);
            }
        }

        public static string Remote(string name, string catalog = null, string host = "localhost", int port = 10035, string protocal = "http")
        {
            if (catalog != null)
            {
                catalog = "/catalogs/" + catalog;
            }
            else
            {
                catalog = "";
            }
            return string.Format("<{0}://{1}:{2}{3}/repositories/{4}>", protocal, host, port, catalog, name);

        }

        public static string Url(string url)
        {
            return string.Format("<{0}>", url);
        }

        public static string Federate(string[] stores)
        {
            for (int i = 0; i < stores.Length; i++)
            {
                stores[i] = string.Format("<{0}>",stores[i]);
            }
            return string.Join("+", stores);
        }

        public static string Reason(string store, string reasoner = "rdf++")
        {
            return string.Format("<{0}>[{1}]", store, reasoner);
        }

        public static string GraphFilter(string store, string[] graphs)
        {
            string asGraphResult = string.Empty;
            Action<object> AsGraph = delegate(object obj)
            {
                if (obj == null)
                {
                    asGraphResult = "null";
                }
                else{
                    asGraphResult = string.Format("<{0}>", obj);
                }
            };
            
            for (int i = 0; i < graphs.Length; i++)
            {
                AsGraph(graphs[i]);
                graphs[i] = asGraphResult;
            }
            return string.Format("<{0}>{1}{2}{3} ", store, "{", string.Join(" ", graphs), "}");
        }
    }
}
