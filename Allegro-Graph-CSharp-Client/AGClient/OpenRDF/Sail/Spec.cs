using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail
{
    public class Spec
    {
        /// <summary>
        /// Indicates the triple store named "store" in the catalog.
        /// </summary>
        /// <param name="store">store name</param>
        /// <param name="catalog">catalog name</param>
        /// <returns></returns>
        public static string Local(string store, string catalog = null)
        {
            if (catalog != null)
            {
                return string.Format("<{0}:{1}>", catalog, store);
            }
            else
            {
                return string.Format("<{0}>", store);
            }
        }

        /// <summary>
        /// A remote store, by URL. If the URL points to the server itself, the store will be opened locally. 
        /// </summary>
        /// <param name="name">store name</param>
        /// <param name="catalog">catalog name</param>
        /// <param name="host">host name</param>
        /// <param name="port">port number</param>
        /// <param name="protocal">protocal type</param>
        /// <returns>A remote store URL. </returns>
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

        /// <summary>
        /// Indicates the triple store named "url" in the root catalog.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Url(string url)
        {
            return string.Format("<{0}>", url);
        }

        /// <summary>
        /// The federation of stores 
        /// </summary>
        /// <param name="stores">stores name</param>
        /// <returns></returns>
        public static string Federate(string[] stores)
        {
            for (int i = 0; i < stores.Length; i++)
            {
                stores[i] = string.Format("<{0}>",stores[i]);
            }
            return  string.Join("+", stores) ;
        }

        /// <summary>
        /// The store "store", with rdfs++ reasoning applied.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="reasoner"></param>
        /// <returns></returns>
        public static string Reason(string store, string reasoner = "rdf++")
        {
            return string.Format("<{0}>[{1}]", store, reasoner);
        }

        /// <summary>
        /// Store "store", filtered to only contain the triples in the default graph (null)or graph named in graphs.
        /// Any number of graphs can be given between the braces. 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="graphs"></param>
        /// <returns></returns>
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
