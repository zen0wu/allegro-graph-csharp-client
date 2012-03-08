using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query
{
    /// <summary>
    /// Records a set of default and named graphs that can restrict the scope of a query. 
    /// </summary>
    public class AGDataset
    {
        List<string> defaultGraphs;
        List<string> namedGraphs;
        public AGDataset(string[] contexts = null)
        {
            defaultGraphs = new List<string>();
            namedGraphs = new List<string>();
            if (contexts != null && contexts.Length > 0)
            {
                namedGraphs.AddRange(contexts);
            }
        }

        public List<string> DefaultGraphs { get { return this.defaultGraphs; } }
        public List<string> NamedGraphs { get { return this.namedGraphs; } }

        public void AddDefaultGraph(string uri)
        {
            defaultGraphs.Add(uri);
        }

        public void RemoveDefaultGraph(string uri)
        {
            defaultGraphs.Remove(uri);
        }

        public void addNamedGraph(string uri)
        {
            namedGraphs.Add(uri);
        }

        public void removeNamedGraph(string uri)
        {
            namedGraphs.Remove(uri);
        }

        public void Clear()
        {
            namedGraphs.Clear();
            defaultGraphs.Clear();
        }

        public string AsQuery(bool excludeNullContext)
        {
            if (defaultGraphs.Count == 0 && namedGraphs.Count == 0)
            {
                if (excludeNullContext)
                    return "";
                else
                    return "## empty dataset ##";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string uri in defaultGraphs)
            {
                if (uri == null && excludeNullContext) continue;//null context should not appear here
                sb.Append("FROM ");
                sb.Append(uri);
                sb.Append("");
            }
            foreach (string uri in namedGraphs)
            {
                if (uri == null && excludeNullContext) continue; //null context should not appear here
                sb.Append("FROM NAMED ");
                sb.Append(uri);
                sb.Append(" ");
            }
            return sb.ToString();
        }

    }
}
