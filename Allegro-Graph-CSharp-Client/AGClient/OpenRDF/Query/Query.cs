using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query
{
    /// <summary>
    ///  A query on a {@link Repository} that can be formulated in one of the
    ///  supported query languages (for example SeRQL or SPARQL). It allows one to
    ///  predefine bindings in the query to be able to reuse the same query with
    ///  different bindings.
    /// </summary>
    public abstract class AbstractQuery
    {
        public string Querylanguage { get; set; }
        public string QueryString { get; set; }
        //public string BaseURI { get; set; }
        public string Contexts { get; set; }
        public string NamedContexts { get; set; }
        public bool IncludeInferred { get; set; }
        public Dictionary<string, string> Bindings { get; set; }
        public RepositoryConnection Connection { get; set; }
        public bool CheckVariables { get; set; }

        public void SetBindings(string key, string value)
        {
            if (Bindings == null)
            {
                Bindings = new Dictionary<string, string>();
            }
            Bindings[key] = value;
        }

        public void SetBindings(Dictionary<string, string> dictionary)
        {
            if (Bindings == null)
            {
                Bindings = new Dictionary<string, string>();
            }
            foreach (string key in dictionary.Keys)
            {
                Bindings[key] = dictionary[key];
            }
        }
        public void RemoveBinding(string key)
        {
            if (Bindings != null && Bindings.ContainsKey(key))
            {
                Bindings.Remove(key);
            }
        }

        /// <summary>
        /// Assert a set of contexts (named graphs) that filter all triples.        
        /// </summary>
        /// <param name="contexts"></param>
        public void SetContext(string contexts)
        {
            this.Contexts = contexts;
        }

        /// <summary>
        ///     Determine whether evaluation results of this query should include inferred statements 
        ///     (if any inferred statements are present in the repository). 
        ///     The default setting is 'true'.
        /// </summary>
        /// <param name="includeInferred"></param>
        public void SetIncludeInferred(bool includeInferred)
        {
            this.IncludeInferred = includeInferred;
        }

        /// <summary>
        /// If true, the presence of variables in the select clause not referenced in a triple are flagged.
        /// </summary>
        /// <param name="setting"></param>
        public void SetCheckVariables(bool setting)
        {
            this.CheckVariables = setting;
        }

        /// <summary>
        ///     Evaluate a SPARQL or PROLOG query, 
        ///     which may be a 'select', 'construct', 'describe' or 'ask' query (in the SPARQL case).  
        ///     Return an appropriate response.
        ///     If analysis is True it will perform query analysis for SPARQL queries.
        ///     analysisTechnique defaults to "", which executes the query to perform dynamic analysis.
        ///     "static" analysis is the other option.
        ///     For analysisTimeout, pass a float of the number of seconds to run the query if executed.
        /// </summary>
        public string evaluate_generic_query(string infer = "false", int limit = -1, int offset = -1)
        {
            RepositoryConnection conn = this.Connection;
            string queryResult = string.Empty;
            if (this.Querylanguage == QueryLanguage.SPARQL)
            {
                queryResult = conn.GetMiniRepository().EvalSPARQLQuery(this.QueryString,
                                                                       infer,
                                                                       this.Contexts,
                                                                       this.NamedContexts,
                                                                       this.Bindings,
                                                                       this.CheckVariables,
                                                                       limit,
                                                                       offset);
            }
            else
            {
                queryResult = conn.GetMiniRepository().EvalPrologQuery(this.QueryString, infer, limit);
            }
            return queryResult;
        }
    }

    public class BooleanQuery : AbstractQuery
    {
        public bool Evaluate(string infer, int limit, int offset)
        {
            bool result = false;
            try
            {
                bool.Parse(this.evaluate_generic_query(infer, limit, offset));
            }
            catch { }
            return result;
        }
    }

    public class StringArrayQuery : AbstractQuery
    {
        public string[][] Evaluate(string infer, int limit, int offset)
        {
            string[][] result = null;
            try
            {
                result = this.Connection.GetMiniRepository().QueryResultToArray(this.evaluate_generic_query(infer, limit, offset));
            }
            catch { }
            return result;
        }
    }

    public class QueryLanguage
    {
        public static string SPARQL { get { return "SPARQL"; } }
        public static string PROLOG { get { return "PROLOG"; } }
    }
}
