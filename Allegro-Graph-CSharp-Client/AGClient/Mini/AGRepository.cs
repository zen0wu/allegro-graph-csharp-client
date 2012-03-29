using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRepository : IAGUrl
    {
        private string RepoUrl;
        private AGCatalog Catalog;
        private string Name;

        /// <summary>
        /// Construct a repository from the catalog it belongs to
        /// </summary>
        /// <param name="Catalog"></param>
        /// <param name="Name"></param>
        public AGRepository(AGCatalog Catalog, string Name)
        {
            RepoUrl = Catalog.Url + "/repositories/" + Name;
            this.Name = Name;
            this.Catalog = Catalog;
        }

        /// <summary>
        /// Constructor for OpenSession
        /// </summary>
        /// <param name="repoUrl">Session URL</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        public AGRepository(string repoUrl, string userName, string password)
        {
            this.RepoUrl = repoUrl;
            this.Catalog = new AGCatalog(new AGServerInfo(repoUrl, userName, password), null);
        }

        public string Url
        {
            get { return RepoUrl; }
            set { RepoUrl = value; }
        }
        public string Username { get { return Catalog.Username; } }
        public string Password { get { return Catalog.Password; } }
        public string DatabaseName { get { return this.Name; } }

        /// <summary>
        /// Get the size of the repository
        /// </summary>
        /// <param name="context">Can specify a named graph as context</param>
        /// <returns>The size of repository</returns>
        public int GetSize(string context = null)
        {
            Dictionary<string, object> parameters = null;
            if (context != null)
            {
                parameters = new Dictionary<string, object>();
                parameters.Add("context", context);
            }
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/size", parameters);
        }

        /// <summary>
        /// Returns the catalog it belongs to
        /// </summary>
        /// <returns>The catalog it belongs to</returns>
        public AGCatalog GetCatalog()
        {
            return this.Catalog;
        }

        /// <summary>
        /// List the contexts in this repository
        /// </summary>
        /// <returns>The names of contexts</returns>
        public string[] ListContexts()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/contexts");
        }

        /// <summary>
        /// Get all blank nodes in this repository
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <returns>The URIs of blank nodes</returns>
        public string[] GetBlankNodes(int amount = 1)
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "POST", string.Format("/blankNodes?amount={0}", amount));
        }

        /// <summary>
        /// Add a new statement
        /// </summary>
        /// <param name="quads">A list of quadtuple, which consists of subject, predicate, object, and context</param>
        public void AddStatements(string[][] quads)
        {
            AGRequestService.DoReq(this, "POST", "/statements", quads);
        }

        /// <summary>
        /// Delete the matching statements
        /// </summary>
        /// <param name="subj">Subject</param>
        /// <param name="pred">Predicate</param>
        /// <param name="obj">Object</param>
        /// <param name="context">Context</param>
        /// <returns>The number of tuples deleted</returns>
        public int DeleteMatchingStatements(string subj, string pred, string obj, string context)
        {
            Dictionary<string, object> body = new Dictionary<string, object>();
            if (subj != null)
                body.Add("subj", subj);
            if (pred != null)
                body.Add("pred", pred);
            if (obj != null)
                body.Add("obj", obj);
            if (context != null)
                body.Add("context", context);
            return AGRequestService.DoReqAndGet<int>(this, "DELETE", "/statements", body);
        }

        /// <summary>
        /// Remove the given statements
        /// </summary>
        /// <param name="quads">Statements to be deleted</param>
        public void DeleteStatements(string[][] quads)
        {
            AGRequestService.DoReq(this, "POST", "/statements/delete", quads);
        }

        /// <summary>
        /// Execute SPARQL Query
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="infer">Infer option, can be "false","rdfs++","restriction"</param>
        /// <param name="context">Context</param>
        /// <param name="namedContext">Named Context</param>
        /// <param name="bindings">Local bindings for variables</param>
        /// <param name="checkVariables">Whether to check the non-existing variable</param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="offset">Skip some of the results at the start</param>
        /// <returns>A raw string representing the result, encoded in JSON format</returns>
        public string EvalSPARQLQuery(string query, string infer = "false", string context = null, string namedContext = null,
           Dictionary<string, string> bindings = null, bool checkVariables = false, int limit = -1, int offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("query", query);
            parameters.Add("queryLn", "sparql");
            parameters.Add("infer", infer);
            if (context != null) parameters.Add("context", context);
            if (namedContext != null) parameters.Add("namedContext", namedContext);
            if (bindings != null)
            {
                foreach (string vari in bindings.Keys)
                    parameters.Add("$" + vari, HttpUtility.UrlEncode(bindings[vari]));
            }
            parameters.Add("checkVariables", checkVariables.ToString());
            if (limit >= 0) parameters.Add("limit", limit.ToString());
            if (offset >= 0) parameters.Add("offset", offset.ToString());
            return AGRequestService.DoReqAndGet(this, "GET", "", parameters);
        }

        /// <summary>
        /// Execute prolog query
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="infer">Infer type <see cref="EvalSPARQLQuery"/></param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="count">If true, counting number of the result will be returned</param>
        /// <param name="accept">Accept header in HTTP request</param>
        /// <returns>A raw result, either the query result, or the count of it</returns>
        public string EvalPrologQuery(string query, string infer = "false", int limit = -1, bool count = false, string accept = null)
        {
            if (accept == null)
            {
                if (count) accept = "text/integer";
                else accept = "application/json";
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("query", query);
            parameters.Add("queryLn", "prolog");
            parameters.Add("infer", infer);
            parameters.Add("accept", accept);
            if (limit >= 0) parameters.Add("limit", limit.ToString());

            return AGRequestService.DoReqAndGet(this, "POST", null, parameters, true);
        }

        /// <summary>
        /// Translate result to Data.DataTable
        /// </summary>
        /// <param name="result">The raw result of query</param>
        /// <returns>A datatable</returns>
        public DataTable QueryResultToDataTable(string result)
        {
            JObject rawResult = JObject.Parse(result);
            JArray headers = rawResult["names"] as JArray;
            JArray contents = rawResult["values"] as JArray;

            DataTable resultTable = new DataTable();
            foreach (JToken columnName in headers)
                resultTable.Columns.Add(new DataColumn(columnName.Value<string>()));

            foreach (JArray rowObj in contents)
            {
                DataRow aRow = resultTable.NewRow();
                int index = 0;
                foreach (JToken cell in rowObj)
                    aRow[index++] = cell.Value<string>();
                resultTable.Rows.Add(aRow);
            }
            return resultTable;
        }

        /// <summary>
        /// Translate result to string array,the first row is names,the rests are values
        /// </summary>
        /// <param name="result">The raw result</param>
        /// <returns>The translated array</returns>
        public string[][] QueryResultToArray(string result)
        {
            JObject rawResult = JObject.Parse(result);
            JArray headers = rawResult["names"] as JArray;
            JArray contents = rawResult["values"] as JArray;

            string[][] resultArray = new string[headers.Count][];
            int index = 0;
            resultArray[0] = new string[headers.Count];
            foreach (JToken columnName in headers)
            {
                resultArray[0][index++] = columnName.Value<string>();
            }
            int row = 1;
            foreach (JArray rowObj in contents)
            {
                resultArray[row++] = new string[rowObj.Count];
                index = 0;
                foreach (JToken cell in rowObj)
                    resultArray[row][index++] = cell.Value<string>();
            }
            return resultArray;
        }

        /// <summary>
        /// Get the specified statements
        /// </summary>
        /// <param name="subj">Subject constraints, can be given multiple times</param>
        /// <param name="pred">Predicate constraints, can be given multiple times</param>
        /// <param name="obj">Object constraints, can be given multiple times</param>
        /// <param name="context">Context constraints, can be given multiple times</param>
        /// <param name="infer">Infer type, default set to "False"</param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="offset">Skip some of the results at the start</param>
        /// <returns>Found statements</returns>
        public string[][] GetStatements(string[] subj, string[] pred, string[] obj, string[] context, string infer = "false",
            int limit = -1, int offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Action<string, string[]> addArrayParam = delegate(string Key, string[] param)
            {
                if (param != null && param.Length > 0)
                {
                    if (param.Length == 1)
                        parameters.Add(Key, param[0]);
                    else
                        parameters.Add(Key, param);
                }
            };
            addArrayParam("subj", subj);
            addArrayParam("pred", pred);
            addArrayParam("obj", obj);
            addArrayParam("context", context);
            if (limit >= 0) parameters.Add("limit", limit.ToString());
            if (offset >= 0) parameters.Add("offset", offset.ToString());
            return AGRequestService.DoReqAndGet<string[][]>(this, "GET", "/statements", parameters);
        }

        /// <summary>
        /// Return all statements whose triple ID matches an ID in the set 'ids'.
        /// </summary>
        /// <param name="ids">Id constraints</param>
        /// <param name="returnIDs">Whether to return ids</param>
        /// <returns>A raw result, eithor the statements or their ids</returns>
        public string[][] GetStatementsById(string ids, bool returnIDs = true)
        {
            string accept = string.Empty;
            if (returnIDs)
            {
                accept = "application/x-quints+json";
            }
            else
            {
                accept = "application/json";
            }
            return AGRequestService.DoReqAndGet<string[][]>(this, "GET", "/statements/id/id=" + ids, accept, null, true);
        }

        /// <summary>
        /// Delete statements by their ids
        /// </summary>
        /// <param name="ids">The ids of the statements</param>
        public void DeleteStatementsById(string[] ids)
        {
            AGRequestService.DoReq(this, "POST", "/statements/delete?ids=true", JsonConvert.SerializeObject(ids));
        }

        /// <summary>
        /// Deletes all duplicate statements that are currently present in the store
        /// </summary>
        /// <param name="indexMode">The indexmode can be either spog (the default) or spo to indicate</param>
        public void DeleteDuplicateStatements(string indexMode = "spog")
        {
            AGRequestService.DoReq(this, "DELETE", "/statements/duplicates?mode=" + indexMode);
        }

        /// <summary>
        /// List the namespaces of the current repository
        /// </summary>
        /// <returns>A list of namespace</returns>
        public List<Namespace> ListNamespaces()
        {
            return AGRequestService.DoReqAndGet<List<Namespace>>(this, "GET", "/namespaces");
        }            
       
        /// <summary>
        /// Returns the namespace URI defined for the given prefix. 
        /// </summary>
        /// <param name="prefix">The prefix of the namespace</param>
        /// <returns>The namespace's name</returns>
        public string GetNamespaces(string prefix)
        {
            return AGRequestService.DoReqAndGet<string>(this, "GET", "/namespaces/" + prefix);
        }

        /// <summary>
        /// Add a namespace
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="nsUrl">Namespace's URL</param>
        public void AddNamespace(string prefix, string nsUrl)
        {
            AGRequestService.DoReq(this, "POST", "/namespaces/" + prefix, "text/plain",nsUrl, true);
        }

        /// <summary>
        /// Delete all the namespaces with the prefix
        /// </summary>
        /// <param name="prefix">Given prefix</param>
        public void DeleteNamespace(string prefix)
        {
            AGRequestService.DoReq(this, "DELETE", "/namespaces/" + prefix);
        }

        /// <summary>
        /// Deletes all namespaces in this repository for the current user.
        /// </summary>
        /// <param name="reset">
        /// If a reset argument of true is passed, the user's namespaces are reset to the default set of namespaces. 
        ///</param>
        public void ClearNamespaces(bool reset = true)
        {
            AGRequestService.DoReq(this, "DELETE", string.Format("/namespaces/reset={0}", reset));
        }

        /// <summary>
        /// Load a given file into AG Server
        /// </summary>
        /// <param name="filePath">Path</param>
        /// <param name="format">File format, can be "nttriples","rdf/xml"</param>
        /// <param name="baseUrl">Base URL</param>
        /// <param name="context">Context, default set to null</param>
        /// <param name="serverSide">Whether this request is on server side</param>
        public void LoadFile(string filePath, string format, string baseUrl = null, string context = null, bool serverSide = false)
        {
            string contentType = string.Empty;
            if (format == "ntriples")
            {
                contentType = "text/plain";
            }
            else if (format == "rdf/xml")
            {
                contentType = "application/rdf+xml";
            }
            string fileContent = string.Empty;
            if (!serverSide)
            {
                StreamReader sr = new StreamReader(filePath);
                fileContent = sr.ReadToEnd();
                sr.Close();
                filePath = null;
            }
            string vars = string.Format("file={0}&context={1}&baseUrl={2}", HttpUtility.UrlEncode(filePath), HttpUtility.UrlEncode(context), HttpUtility.UrlEncode(baseUrl));
            string relativeUrl = "/statements?" + vars;
            AGRequestService.DoReq(this, "POST", relativeUrl, contentType, fileContent);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        ///Type mapping
        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Fetches a result set of currently specified mappings. 
        /// </summary>
        /// <returns>List<DataType></returns>
        public List<DataType> ListTypeMapping()
        {
            return AGRequestService.DoReqAndGet<List<DataType>>(this, "GET", "/mapping");
        }

       /// <summary>
       /// Clear type mappings for this repository. 
       /// </summary>
       /// <param name="isAll">
        ///      if true Clear all type mappings for this repository including the automatic ones.
        ///      else Clear all non-automatic type mappings for this repository. 
       /// </param>
        public void ClearTypeMapping(bool isAll=false)
        {
            AGRequestService.DoReq(this, "DELETE", "/mapping");
        }

        /// <summary>
        /// Yields a list of literal types for which datatype mappings have been defined in this store.
        /// </summary>
        /// <returns>The set of type</returns>
        public string[] ListMappedTypes()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/mapping/type");
        }

        /// <summary>
        /// Takes two arguments, type (the RDF literal type) and encoding, 
        /// and defines a datatype mapping from the first to the second
        /// </summary>
        /// <param name="type">the RDF literal type</param>
        /// <param name="encoding">Encoding</param>
        public void AddMappedType(string type, string encoding)
        {
            AGRequestService.DoReq(this, "PUT", string.Format("/mapping/type?type={0}&encoding={1}",type,encoding));
        }

        /// <summary>
        /// Deletes a datatype mapping
        /// </summary>
        /// <param name="type">type should be an RDF resource</param>
        public void DeleteMappedType(string type)
        {
            AGRequestService.DoReq(this, "DELETE", string.Format("/mapping/type?type={0}", type));
        }

        /// <summary>
        /// Yields a list of literal types for which predicate mappings have been defined in this store. 
        /// </summary>
        /// <returns></returns>
        public string[] ListMappedPredicates()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/mapping/predicate");
        }

        /// <summary>
        /// Takes two arguments, predicate and encoding, and defines a predicate mapping on them. 
        /// </summary>
        /// <param name="predicate">predicate</param>
        /// <param name="encoding">encoding</param>
        public void AddMappedPredicate(string predicate, string encoding)
        {
            AGRequestService.DoReq(this, "POST", string.Format("/mapping/predicate?predicate={0}&encoding={1}",predicate,encoding));
        }

        /// <summary>
        /// Deletes a predicate mapping. Takes one parameter, predicate. 
        /// </summary>
        /// <param name="predicate">predicate</param>
        public void DeleteMappedPredicate(string predicate)
        {
            AGRequestService.DoReq(this, "DELETE", string.Format("/mapping/predicate?predicate={0}",predicate));
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        ///triple index
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// List all the indices
        /// </summary>
        public string[] ListIndices()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/indices");
        }


        /// <summary>
        /// List the valid indices
        /// </summary>
        /// <returns>set of index</returns>
        public string[] ListValidIndices()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/indices?listValid=true");
        }

        /// <summary>
        /// Add an index with specific type
        /// </summary>
        /// <param name="indexType">Index type</param>
        public void AddIndex(string indexType)
        {
            AGRequestService.DoReq(this, "PUT", "/indices/" + indexType);
        }

        /// <summary>
        /// Drop an index with type
        /// </summary>
        /// <param name="indexType">index type to drop</param>
        public void DropIndex(string indexType)
        {
            AGRequestService.DoReq(this, "DELETE", "/indices/" + indexType);
        }

        /// <summary>
        /// Tells the server to try and optimize the indices for this store
        /// </summary>
        /// <param name="wait">
        ///  Defaulting to false,Indicates whether the HTTP request should return right away or
        ///  whether it should wait for the operation to complete.
        /// </param>
        /// <param name="level">Level determines how much work should be done. </param>
        public void OptimizeIndex(bool wait = false, string level = null)
        {
            AGRequestService.DoReq(this, "POST", string.Format("/indices/optimize?wait={0}&level={1}",wait,level));
        }

        /// <summary>
        /// Open a new session
        /// </summary>
        /// <returns></returns>
        public string OpenSession(string spec, bool autocommit = false, int lifetime = -1, bool loadinitfile = false)
        {
            string relativeUrl = string.Empty;
            if (lifetime == -1)
            {
                relativeUrl = string.Format("/session?autoCommit={0},loadInitFile={1},store={2}", autocommit, loadinitfile, spec);
            }
            else
            {
                relativeUrl = string.Format("/session?autoCommit={0}, lifetime={1},loadInitFile={2}, store={3}", autocommit, lifetime, loadinitfile, spec);
            }
            return AGRequestService.DoReqAndGet<string>(this, "POST", relativeUrl);
            //return new AGRepository(sessionUrl, this.Username, this.Password);
        }

        /// <summary>
        /// Close the current session
        /// </summary>
        public void CloseSession()
        {
            try
            {
                AGRequestService.DoReq(this, "POST", "/session/close");
            }
            catch { }
        }

        /// <summary>
        /// Prepares a query,Preparing queries is only supported in a dedicated session,
        /// and the prepared queries will only be available in that session. 
        /// </summary>
        /// <param name="PQueryID">prepares query id</param>
        /// <param name="query">Query string</param>
        /// <param name="infer">Infer option, can be "false","rdfs++","restriction"</param>
        /// <param name="context">Context</param>
        /// <param name="namedContext">Named Context</param>
        /// <param name="bindings">Local bindings for variables</param>
        /// <param name="checkVariables">Whether to check the non-existing variable</param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="offset">Skip some of the results at the start</param>
        /// <returns>prepares query and saves query under id.</returns>
        public void PreparingQueries(string PQueryID, string query, string infer = "false", string context = null, string namedContext = null,
           Dictionary<string, string> bindings = null, bool checkVariables = false, int limit = -1, int offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("query", query);
            parameters.Add("queryLn", "sparql");
            parameters.Add("infer", infer);
            if (context != null) parameters.Add("context", context);
            if (namedContext != null) parameters.Add("namedContext", namedContext);

            if (bindings != null)
            {
                foreach (string vari in bindings.Keys)
                    parameters.Add("$" + vari, HttpUtility.UrlEncode(bindings[vari]));
            }
            parameters.Add("checkVariables", checkVariables.ToString());
            if (limit >= 0) parameters.Add("limit", limit.ToString());
            if (offset >= 0) parameters.Add("offset", offset.ToString());
            AGRequestService.DoReq(this, "PUT", "/queries/" + PQueryID, parameters);
        }

        /// <summary>
        ///  Executes a prepared query stored under the name id 
        /// </summary>
        /// <param name="PQueryID">prepared query id</param>
        public void ExecutePreparingQueries(string PQueryID)
        {
            AGRequestService.DoReq(this, "GET", "/queries/" + PQueryID);
        }

        /// <summary>
        /// Executes a prepared query stored under the name id with some parameters
        /// </summary>
        /// <param name="PQueryID">prepared query id</param>
        /// <param name="bindings">Local bindings for variables</param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="offset">Skip some of the results at the start</param>
        public void ExecutePreparingQueries(string PQueryID, Dictionary<string, string> bindings = null, int limit = -1, int offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AGRequestService.DoReq(this, "POST", "/queries/" + PQueryID, parameters);
        }

        /// <summary>
        /// Deletes the prepared query stored under id
        /// </summary>
        /// <param name="PQueryID">prepared query id</param>
        public void DeletePreparingQueries(string PQueryID)
        {
            AGRequestService.DoReq(this, "DELETE", "/queries/" + PQueryID);
        }

        /// <summary>
        /// Define Prolog functors, 
        /// which can be used in Prolog queries. 
        /// This is only allowed when accessing a dedicated session. 
        /// </summary>
        /// <param name="prologFunction">prolog function content</param>
        public void DefinePrologFunction(string prologFunction)
        {
            AGRequestService.DoReq(this, "POST", "/functor", prologFunction);
        }

        /// <summary>
        /// Commit the current session
        /// </summary>
        public void Commit()
        {
            AGRequestService.DoReq(this, "POST", "/session/commit");
        }

        /// <summary>
        /// Rollback the current session
        /// </summary>
        public void Rollback()
        {
            AGRequestService.DoReq(this, "POST", "/rollback");
        }

        /// <summary>
        ///     Enable the spogi cache in this repository. 
        ///     Takes an optional size argument to set the size of the cache.
        /// </summary>
        /// <param name="size">Triple cache size</param>
        public void EnableTripleCache(int size = -1)
        {
            string queryUrl = string.Empty;
            if (size == -1)
            {
                queryUrl = string.Format("/tripleCache");
            }
            else
            {
                queryUrl = string.Format("/tripleCache?size={0}", size);
            }
            AGRequestService.DoReq(this, "PUT", queryUrl);
        }

        /// <summary>
        /// Disable the spogi cache for this repository. 
        /// </summary>
        public void DisableTripleCache()
        {
            AGRequestService.DoReq(this, "DELETE", "/tripleCache");
        }

        /// <summary>
        /// Find out whether the 'SPOGI cache' is enabled, 
        /// and what size it has. Returns an integer 
        /// 0 when the cache is disabled, the size of the cache otherwise. 
        /// </summary>
        /// <returns>An integer denoting the triple cache size</returns>
        public int GetTripleCacheSize()
        {
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/tripleCache");
        }


        ///////////////////////////////////////////////////////////////////////////////////
        //  FreeText
        ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Use free-text indices to search for the given pattern.
        /// </summary>
        /// <param name="pattern">The text to search for</param>
        /// <param name="expression">An S-expression combining search strings using and, or, phrase, match, and fuzzy. </param>
        /// <param name="index">
        ///   An optional parameter that restricts the search to a specific free-text index.
        ///   If not given, all available indexes are used
        /// </param>
        /// <param name="sorted"> 
        ///     indicating whether the results should be sorted by relevance. Default is false. 
        /// </param>
        /// <param name="limit">An integer limiting the amount of results that can be returned.</param>
        /// <param name="offset">An integer telling the server to skip the first few results</param>
        /// <returns>an array of statements</returns>
        public string[][] EvalFreeTextIndex(string pattern, string expression = null, string index = null, bool sorted = false, int limit = -1, int offset = -1)
        {
            StringBuilder sbParameter = new StringBuilder(string.Format("?pattern={0}", pattern));
            if (expression != null)
            {
                sbParameter.Append(string.Format("&expression={0}", expression));
            }
            if (index != null)
            {
                sbParameter.Append(string.Format("&index={0}", index));
            }
            sbParameter.Append(string.Format("&sorted={0}", sorted));
            if (limit != -1)
            {
                sbParameter.Append(string.Format("&limit={0}", limit));
            }
            if (offset != -1)
            {
                sbParameter.Append(string.Format("&offset={0}", offset));
            }
            return AGRequestService.DoReqAndGet<string[][]>(this, "GET", "/freetext" + sbParameter.ToString());
        }



        /// <summary>
        /// Create a free-text index with the given parameters
        /// </summary>
        /// <param name="method">http method</param>
        /// <param name="name">string identifying the new index </param>
        /// <param name="predicates">If no predicates are given, triples are indexed regardless of predicate</param>
        /// <param name="indexLiterals">
        ///     IndexLiterals determines which literals to index.
        ///     It can be True (the default), False, or a list of resources, 
        ///     indicating the literal types that should be indexed
        /// </param>
        /// <param name="indexResources">
        ///     IndexResources determines which resources are indexed. 
        ///     It can be True, False (the default), or "short", 
        ///     to index only the part of resources after the last slash or hash character.
        /// </param>
        /// <param name="indexFields">
        ///     IndexFields can be a list containing any combination of the elements
        ///     "subject", "predicate", "object", and "graph".The default is ["object"]. 
        /// </param>
        /// <param name="minimumWordSize"> 
        ///     Determines the minimum size a word must have to be indexed.
        ///     The default is 3
        /// </param>
        /// <param name="stopWords">
        ///     StopWords should hold a list of words that should not be indexed. 
        ///     When not given, a list of common English words is used. 
        /// </param>
        /// <param name="wordFilters">
        ///     WordFilters can be used to apply some normalizing filters to words as they are indexed or queried.
        ///     Can be a list of filter names.  Currently, only "drop-accents" and "stem.english" are supported. 
        /// </param>
        /// <param name="innerChars">The character set to be used as the constituent characters of a word</param>
        /// <param name="borderChars"> The character set to be used as the border characters of indexed words. </param>
        /// <param name="tokenizer">An optional string. Can be either default or japanese.</param>
        public void ManipulateFreeTextIndex(string method, string name, string[] predicates = null,
                                            object indexLiterals = null,
                                            string indexResources = "true", string[] indexFields = null,
                                            int minimumWordSize = 3, string[] stopWords = null,
                                            string[] wordFilters = null, char[] innerChars = null,
                                            char[] borderChars = null, string tokenizer = null)
        {
            StringBuilder paramsBuilder = new StringBuilder();
            Action<string, string> AddParam = delegate(string paramName, string paramValue)
            {
                if (paramsBuilder.Length > 0)
                    paramsBuilder.Append(string.Format("&{0}={1}", paramName, paramValue));
                else
                    paramsBuilder.Append(string.Format("{0}={1}", paramName, paramValue));
            };

            if (predicates != null && predicates.Length > 0)
            {
                for (int i = 0; i < predicates.Length; i++)
                {
                    AddParam("predicate", predicates[i]);
                }
            }
            else AddParam("predicate", "");

            if (indexLiterals != null)
            {
                if (indexLiterals is string) AddParam("indexLiterals", indexLiterals.ToString());
                else if (indexLiterals is string[])
                {
                    string[] index_Literals = indexLiterals as string[];
                    for (int i = 0; i < index_Literals.Length; i++)
                        AddParam("indexLiteralType", index_Literals[i]); //if indexLiteralType is an array of literal types to index,
                }
            }
            else AddParam("indexLiterals", "");


            if (indexResources != null) AddParam("indexResources", indexResources);
            else AddParam("indexResources", "");

            if (indexFields != null && indexFields.Length > 0)
            {
                for (int i = 0; i < indexFields.Length; i++)
                    AddParam("indexField", indexFields[i]);
            }
            else AddParam("indexField", "");

            if (minimumWordSize != -1)
            {
                AddParam("minimumWordSize", minimumWordSize.ToString());
            }

            if (stopWords != null && stopWords.Length > 0)
            {
                for (int i = 0; i < stopWords.Length; i++)
                {
                    AddParam("stopword", stopWords[i]);
                }
            }
            else AddParam("stopword", "");

            if (wordFilters != null & wordFilters.Length > 0)
            {
                for (int i = 0; i < wordFilters.Length; i++)
                    AddParam("wordFilter", wordFilters[i]);
            }
            else AddParam("wordFilter", "");

            if (innerChars != null && innerChars.Length > 0)
            {
                for (int i = 0; i < innerChars.Length; i++)
                {
                    AddParam("innerChars", innerChars[i].ToString());
                }
            }

            if (borderChars != null && borderChars.Length > 0)
            {
                for (int i = 0; i < borderChars.Length; i++)
                {
                    AddParam("borderChars", borderChars[i].ToString());
                }
            }
            if (tokenizer != null) AddParam("tokenizer", tokenizer);
            else AddParam("tokenizer", "");

            AGRequestService.DoReq(this, method, "/freetext/indices/" + name, paramsBuilder.ToString());
        }

        /// <summary>
        /// Crate a free text index
        /// </summary>
        /// <seealso cref="ManipulateFreeTextIndex"/>
        public void CreateFreeTextIndex(string name, string[] predicates = null, object indexLiterals = null,
                                        string indexResources = "true", string[] indexFields = null,
                                        int minimumWordSize = -1, string[] stopWords = null,
                                        string[] wordFilters = null, char[] innerChars = null,
                                        char[] borderChars = null, string tokenizer = null)
        {
            ManipulateFreeTextIndex("PUT", name, predicates, indexLiterals, indexResources, indexFields, minimumWordSize, stopWords, wordFilters, innerChars, borderChars, tokenizer);
        }

        /// <summary>
        /// Modify a free text index
        /// </summary>
        /// <seealso cref="ManipulateFreeTextIndex"/>
        public void ModifyFreeTextIndex(string name, string[] predicates = null, object indexLiterals = null,
                                        string indexResources = "true", string[] indexFields = null,
                                        int minimumWordSize = -1, string[] stopWords = null,
                                        string[] wordFilters = null, char[] innerChars = null,
                                        char[] borderChars = null, string tokenizer = null)
        {
            ManipulateFreeTextIndex("POST", name, predicates, indexLiterals, indexResources, indexFields, minimumWordSize, stopWords, wordFilters, innerChars, borderChars, tokenizer);
        }

        /// <summary>
        /// Returns a list of names of free-text indices defined in this repository. 
        /// </summary>
        /// <returns></returns>
        public string[] ListFreeTextIndices()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/freetext/indices");
        }

        /// <summary>
        /// Delete the named free-text index
        /// </summary>
        /// <param name="indexName"></param>
        public void DeleteFreeTextIndex(string name)
        {
            AGRequestService.DoReq(this, "DELETE", "/freetext/indices/" + name);
        }

        /// <summary>
        /// List all the free text predicates
        /// </summary>
        /// <returns>The URIs of the free text predicates</returns>
        public string[] ListFreeTextPredicates()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/freetext/predicates");
        }

        /// <summary>
        /// Register a new free text predicate
        /// </summary>
        /// <param name="predicate">the URI of predicate</param>
        public void RegisterFreeTextPredicate(string predicate)
        {
            AGRequestService.DoReq(this, "POST", "/freetext/predicates", "predicate=" + predicate);
        }

        /// <summary>
        /// Returns the configuration parameters of the named free-text index
        /// </summary>
        /// <param name="indexName">Free text index name</param>
        /// <returns></returns>
        public FreeTextIndex GetFreeTextIndex(string indexName)
        {
            return AGRequestService.DoReqAndGet<FreeTextIndex>(this, "GET", "/freetext/indices/" + indexName);
        }

        /// <summary>
        /// Returns the configuration parameter of the named free-text index
        /// </summary>
        /// <param name="indexName">Free text index name</param>
        /// <param name="paramName">parameter name</param>
        /// <returns></returns>
        public string GetFreeTextIndexConfiguration(string indexName, string paramName)
        {
            return AGRequestService.DoReqAndGet<string>(this, "GET", string.Format("/freetext/indices/{0}/{1}", indexName, paramName));
        }

        /// <summary>
        ///     Returns a dictionary with fields "predicates",
        ///     "indexLiterals","indexResources","indexFields",
        ///     "minimumWordSize", "stopWords", and "wordFilters".
        /// </summary>
        /// <param name="index">Free text index name</param>
        /// <returns></returns>
        public Dictionary<string, string> GetFreeTextIndexConfiguration(string indexName)
        {
            return AGRequestService.DoReqAndGet<Dictionary<string, string>>(this, "GET", "/freetext/indices/" + indexName);
        }

        /// <summary>
        /// Evaluate a free text search
        /// </summary>
        /// <param name="pattern">The search pattern</param>
        /// <param name="infer">Whether to perform infer</param>
        /// <param name="limit">The size limit of result</param>
        /// <param name="indexs">The indices involved</param>
        /// <returns></returns>

        public string[] EvalFreeTextSearch(string pattern, bool infer = false, int limit = -1, string[] indexs = null)
        {
            string urlParam = "";
            if (indexs == null)
            {
                urlParam = string.Format("/freetext/pattern={0}&infer={1}&limit={2}&index={3}", pattern, infer, limit, "");
            }
            else
            {
                if (indexs.Length > 0)
                {
                    StringBuilder indexParam = new StringBuilder(string.Format("index={0}", indexs[0]));
                    for (int i = 1; i < indexs.Length; i++)
                    {
                        indexParam.Append(string.Format("&index={0}", indexs[i]));
                    }
                    urlParam = string.Format("/freetext/pattern={0}&infer={1}&limit={2}&{3}", pattern, infer, limit, indexParam.ToString());
                }
            }
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/freetext/indices/" + urlParam);
        }

        /// <summary>
        /// Define a prolog functor
        /// </summary>
        public void DefinePrologFunctors(string rules)
        {
            AGRequestService.DoReq(this, "POST", "/functor", rules);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //Geo-spatial               
        //////////////////////////////////////////////////////////////////////////////////////////////////// 
        public string[] ListGeoTypes()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/geo/types");
        }

        public List<Statement> GetStatementsInsideBox(string type, string predicate,
                                           float xMin, float xMax, float yMin, float yMax,
                                           float limit = -1, float offset = -1)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append(string.Format("type={0}", type));
            parameters.Append(string.Format("&predicate={0}", predicate));
            parameters.Append(string.Format("&xMin={0}", xMin));
            parameters.Append(string.Format("&xMax={0}", xMax));
            parameters.Append(string.Format("&yMin={0}", yMin));
            parameters.Append(string.Format("&yMax={0}", yMax));
            if (limit != -1) parameters.Append(string.Format("&limit={0}", limit));
            if (offset != -1) parameters.Append(string.Format("&offset={0}", offset));

            return AGRequestService.DoReqAndGet<List<Statement>>(this, "GET", "/geo/box", parameters.ToString());
        }
        public List<Statement> GetStatementsInsideCircle(string type, string predicate,
                                                         float x, float y, float radius,
                                                         float limit = -1, float offset = -1)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append(string.Format("type={0}", type));
            parameters.Append(string.Format("&predicate={0}", predicate));
            parameters.Append(string.Format("&x={0}", x));
            parameters.Append(string.Format("&y={0}", y));
            parameters.Append(string.Format("&radius={0}", radius));
            if (limit != -1) parameters.Append(string.Format("&limit={0}", limit));
            if (offset != -1) parameters.Append(string.Format("&offset={0}", offset));

            return AGRequestService.DoReqAndGet<List<Statement>>(this, "GET", "/geo/circle", parameters.ToString());
        }
        /// <summary>
        /// Get all the triples with a given predicate whose object lies within radius units 
        /// from the given latitude/longitude.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="predicate"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <param name="unit"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<Statement> GetStatementsHaversine(string type, string predicate,
                                                         float latitude, float longitude, float radius,
                                                         string unit = "km", float limit = -1, float offset = -1)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append(string.Format("type={0}", type));
            parameters.Append(string.Format("&predicate={0}", predicate));
            parameters.Append(string.Format("&lat={0}", latitude));
            parameters.Append(string.Format("&long={0}", longitude));
            parameters.Append(string.Format("&unit={0}", unit));
            parameters.Append(string.Format("&radius={0}", radius));
            if (limit != -1) parameters.Append(string.Format("&limit={0}", limit));
            if (offset != -1) parameters.Append(string.Format("&offset={0}", offset));
            return AGRequestService.DoReqAndGet<List<Statement>>(this, "GET", "/geo/haversine", parameters.ToString());
        }

        public List<Statement> GetStatementsInsidePolygon(string type, string predicate, object polygon,
                                                          float limit = -1, float offset = -1)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append(string.Format("type={0}", type));
            parameters.Append(string.Format("&predicate={0}", predicate));
            parameters.Append(string.Format("&polygon={0}", polygon));

            if (limit != -1) parameters.Append(string.Format("&limit={0}", limit));
            if (offset != -1) parameters.Append(string.Format("&offset={0}", offset));

            return AGRequestService.DoReqAndGet<List<Statement>>(this, "GET", "/geo/polygon", parameters.ToString());
        }


        /////////////////////////////////////////////////////////////////////////////////////////////
        // SNA   Social Network Analysis Methods
        /////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// subjectOf, objectOf, and undirected can be either a single predicate or a list of predicates.
        /// query should be a prolog query in the form (select ?x (q- ?node !<mypredicate> ?x)),
        /// where ?node always returns to the argument passed to the generator.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subjectOf"></param>
        /// <param name="objectOf"></param>
        /// <param name="undirected"></param>
        /// <param name="query"></param>
        public void RegisterSNAGenerator(string name, string[] subjectOf = null, string[] objectOf = null, string[] undirected = null, string query = null)
        {
            StringBuilder parameters = new StringBuilder(string.Format("/snaGenerators/{0}?", name));
            if (subjectOf != null)
            {
                foreach (string pred in subjectOf) parameters = AddParams("subjectOf", pred, parameters);
            }
            if (objectOf != null)
            {
                foreach (string pred in objectOf) parameters = AddParams("objectOf", pred, parameters);
            }
            if (undirected != null)
            {
                foreach (string pred in undirected) parameters = AddParams("undirected", pred, parameters);
            }
            if (query != null) parameters = AddParams("query", query, parameters);
            AGRequestService.DoReq(this, "PUT", parameters.ToString());
        }

        /// <summary>
        ///     Create a neighbor-matrix, which is a pre-computed generator
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group">
        ///     A set of N-Triples terms (can be passed multiple times)
        ///     which serve as start nodes for building up the matrix. 
        ///</param>
        /// <param name="generator">The generator to use, by name</param>
        /// <param name="depth">
        /// An integer specifying the maximum depth to which to compute the matrix. Defaults to 1
        /// </param>
        public void RegisterNeighborMatrix(string name, string[] group, string generator, int depth = 1)
        {
            StringBuilder parameters = new StringBuilder(string.Format("/neighborMatrices/{0}?", name));
            if (group != null)
            {
                foreach (string s in group) parameters = AddParams("group", s, parameters);
            }
            if (generator != null) parameters = AddParams("generator", generator, parameters);
            AddParams("depth", depth.ToString(), parameters);
            AGRequestService.DoReq(this, "PUT", parameters.ToString());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        // Tools
        //////////////////////////////////////////////////////////////////////////////////////////////////
        public static StringBuilder AddParams(string paramName, string paramValue, StringBuilder parameters)
        {
            if (parameters.Length > 0 && parameters.ToString().Contains("&"))
                parameters.Append(string.Format("&{0}={1}", paramName, paramValue));
            else
                parameters.Append(string.Format("{0}={1}", paramName, paramValue));
            return parameters;
        }
    }
}
