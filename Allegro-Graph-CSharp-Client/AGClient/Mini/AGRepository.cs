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
            return AGRequestService.DoReqAndGet<string[]>(this, "POST", "/blankNodes", string.Format("amount={0}", amount));
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

        //public DataTable EvalSPARQLQuery(string Query, bool isReturnDataTable, string Infer = "false", string Context = null, string NamedContext = null,
        //    Dictionary<string, string> Bindings = null, bool CheckVariables = false, int Limit = -1, int Offset = -1)
        //{
        //    JObject rawResult = JObject.Parse(EvalSPARQLQuery(Query,Infer,Context,NamedContext,Bindings,CheckVariables,Limit,Offset));
        //    JArray headers = rawResult["names"] as JArray;
        //    JArray contents = rawResult["values"] as JArray;

        //    DataTable resultTable = new DataTable();
        //    foreach (JToken columnName in headers)
        //        resultTable.Columns.Add(new DataColumn(columnName.Value<string>()));

        //    foreach (JArray rowObj in contents)
        //    {
        //        DataRow aRow = resultTable.NewRow();
        //        int index = 0;
        //        foreach (JToken cell in rowObj)
        //            aRow[index++] = cell.Value<string>();
        //        resultTable.Rows.Add(aRow);
        //    }
        //    return resultTable;
        //}

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
        /// Get the statements by their ids
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
        /// List the namespaces of the current repository
        /// </summary>
        /// <returns>A list of namespace</returns>
        public List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace> ListNamespaces()
        {
            return AGRequestService.DoReqAndGet<List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace>>(this, "GET", "/namespaces");
        }

        /// <summary>
        /// Get a specific namespace
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
        /// <param name="name">Namespace's name</param>
        public void AddNamespace(string prefix, string name)
        {
            AGRequestService.DoReq(this, "PUT", "/namespaces/" + prefix, "text/plain", null, true);
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
        /// Clear all the namespace
        /// </summary>
        /// <param name="reset">Whether to reset</param>
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
        /// Enable the triple cache
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
        /// Remove the triple cache
        /// </summary>
        public void DisableTripleCache()
        {
            AGRequestService.DoReq(this, "DELETE", "/tripleCache");
        }

        /// <summary>
        /// Get the size of the current triple cache size
        /// </summary>
        /// <returns>An integer denoting the triple cache size</returns>
        public int GetTripleCacheSize()
        {
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/tripleCache");
        }

        /// <summary>
        /// Create a new free-text index
        /// </summary>
        public void ManipulateFreeTextIndex(string method, string name, string[] predicates = null, object indexLiterals = null,
                                  string indexResources = "true", string[] indexFields = null,
                                  int minimumWordSize = -1, string[] stopWords = null,
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
        /// Define a prolog functor
        /// </summary>
        public void DefinePrologFunctors(string rules)
        {
            AGRequestService.DoReq(this, "POST", "/functor", rules);
        }

        /// <summary>
        ///     Returns a dictionary with fields "predicates",
        ///     "indexLiterals","indexResources","indexFields",
        ///     "minimumWordSize", "stopWords", and "wordFilters".
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFreeTextIndexConfiguration(string index)
        {
            return AGRequestService.DoReqAndGet<Dictionary<string, string>>(this, "GET", "/freetext/indices/" + index);
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
                    StringBuilder indexParam = new StringBuilder(string.Format("index={0}",indexs[0]));
                    for (int i = 1; i < indexs.Length; i++)
                    {
                        indexParam.Append(string.Format("&index={0}", indexs[i]));
                    }
                    urlParam = string.Format("/freetext/pattern={0}&infer={1}&limit={2}&{3}", pattern, infer, limit,indexParam.ToString() );
                }
            }
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/freetext/indices/" + urlParam);
        }
    }
}
