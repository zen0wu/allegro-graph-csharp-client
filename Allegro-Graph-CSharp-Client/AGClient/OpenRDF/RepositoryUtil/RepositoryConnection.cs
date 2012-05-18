using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Rio;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        private Repository _repository;
        public RepositoryConnection(Repository repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// Returns the amount of statements in the repository, as an integer
        /// Use 'null' to get the size of the default graph (the unnamed context). 
        /// </summary>
        /// <param name="contexts">String set of named context,default to null</param>
        /// <returns>Returns the amount of statements in the repository</returns>
        public int GetSize(string[] contexts = null)
        {
            if (contexts == null)
            {
                return _repository.GetMiniRepository().GetSize();
            }
            else if (contexts.Length == 1)
            {
                return _repository.GetMiniRepository().GetSize(contexts[0]);
            }
            else
            {
                int count = 0;
                foreach (string context in contexts)
                {
                    count += _repository.GetMiniRepository().GetSize(context);
                }
                return count;
            }
        }

        /// <summary>
        /// Returns true if this repository does not contain any (explicit)statements.
        /// </summary>
        /// <param name="contexts">String set of named context,default to null</param>
        /// <returns></returns>
        public bool IsEmpty(string[] contexts = null)
        {
            return GetSize(contexts) == 0;
        }


        /// <summary>
        /// Returns a string composed of the catalog name concatenated with the repository name
        /// </summary>
        /// <returns></returns>
        public string GetSpec()
        {
            string catalogName = _repository.GetMiniRepository().GetCatalog().GetName();
            if (catalogName == null || catalogName == "/")
                catalogName = "";
            else
                catalogName += ":";
            return string.Format("<{0}{1}>", catalogName, _repository.GetDatabaseName());
        }

        /// <summary>
        /// Get the MimiRepository
        /// </summary>
        /// <returns></returns>
        public AGRepository GetMiniRepository()
        {
            return _repository.GetMiniRepository();
        }

        /// <summary>
        /// List the contexts in this repository
        /// </summary>
        /// <returns>The names of contexts</returns>
        public string[] GetContextIDs() { return this.GetMiniRepository().ListContexts(); }

        /// <summary>
        /// Retrieves statements (triples) by matching against their components.
        /// All parameters are optional — when none are given, every statement in the store is returned. 
        /// </summary>
        /// <param name="Subj">
        ///     Match a specific subject.When given, should be a term in N-triples format.
        /// </param>
        /// <param name="Pred">
        ///     Match a specific predicate. Can be passed multiple times, like subj, to match a set
        /// </param>
        /// <param name="Obj">Match a specific object. Pass multiple values to match a set.</param>
        /// <param name="Context">
        ///     Can be given multiple times. 
        ///     Restricts the query to the given list of named graphs. 
        ///     When not specified, all graphs in the store are used. 
        /// </param>
        /// <param name="Infer">
        ///     Used to turn on reasoning for this query. 
        ///     Valid values are false, rdfs++, and hasvalue. Default is false — no reasoning. 
        /// </param>
        /// <param name="Limit">An integer indicating the maximum amount of results to return.</param>
        /// <param name="Offset">An integer. Tell the server to skip a number of results before it starts returning.</param>
        /// <returns>Found statements</returns>
        public string[][] GetStatements(string[] Subj, string[] Pred, string[] Obj, string[] Context,
                                        string Infer = "false", int Limit = -1, int Offset = -1)
        {
            return this.GetMiniRepository().GetStatements(Subj, Pred, Obj, Context, Infer, Limit, Offset);
        }

        /// <summary>
        /// Return all statements whose triple ID matches an ID in the set 'ids'.
        /// </summary>
        /// <param name="ids">set of ids</param>
        /// <param name="returnIDs">Whether to return ids</param>
        /// <returns></returns>
        public string[][] GetStatementsById(string ids, bool returnIDs = true)
        {
            return this.GetMiniRepository().GetStatementsById(ids, returnIDs);
        }

        public string[] GetStatementIDs(string[] Subj, string[] Pred, string[] Obj, string[] Context,
                                        string Infer = "false", int Limit = -1, int Offset = -1)
        {
            return this.GetMiniRepository().GetStatementIDs(Subj, Pred, Obj, Context, Infer, Limit, Offset);
        }

        public string[] GetStatementIDs()
        {
            return this.GetMiniRepository().GetStatementIDs();
        }

        /// <summary>
        /// Create Statement object
        /// </summary>
        /// <param name="subj">subject</param>
        /// <param name="pred">predicate</param>
        /// <param name="obj">object</param>
        /// <param name="context">context</param>
        /// <returns>Statement object</returns>
        public Statement CreateStatement(string subj, string pred, string obj, string context = null)
        {
            return ValueFactory.CreateStatement(subj, pred, obj, context);
        }

        public URI CreateURI(string uri = null, string nameSpace = null, string localname = null)
        {
            return ValueFactory.CreateURI(uri, nameSpace, localname);
        }

        /// <summary>
        /// Create blank node 
        /// </summary>
        /// <param name="nodeID">specify blank node id </param>
        /// <returns>BNode object</returns>
        public BNode CreateBNode(string nodeID = null)
        {
            return ValueFactory.GetBNode(nodeID);
        }


        /// <summary>
        /// Get all blank nodes in this repository
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <returns>The URIs of blank nodes</returns>
        public string[] GetBlankNodes(int amount = 1)
        {
            return _repository.GetMiniRepository().GetBlankNodes(amount);
        }

        /// <summary>
        ///  Loads a file into the triple store,a file can be loaded into only one context     
        /// </summary>
        /// <param name="filePath">the file to load</param>
        /// <param name="baseUrl">baseURI associate with loading a file</param>
        /// <param name="format">RDFFormat.NTRIPLES or RDFFormat.RDFXML</param>
        /// <param name="context">an optional context URI (subgraph URI), defaulting to null.If null, the triple(s) will be added to the null context (the default or background graph).</param>
        /// <param name="serverSide">Whether this request is on server side</param>
        public void AddFile(string filePath, string baseUrl = null, RDFFormat format = null, string context = null, bool serverSide = false)
        {
            if (baseUrl != null)
            {
                filePath = string.Format("{0}\\{1}", baseUrl, filePath);
            }
            FileInfo file = new FileInfo(filePath);
            string[] nTripleExts = { ".nt", ".ntriples" };
            string[] rdfExts = { ".rdf", ".owl" };
            string fileFormat = string.Empty;
            if (nTripleExts.Contains(file.Extension.ToLower()))
            {
                fileFormat = "ntriples";
            }
            else if (rdfExts.Contains(file.Extension.ToLower()))
            {
                fileFormat = "rdf/xml";
            }
            _repository.GetMiniRepository().LoadFile(filePath, fileFormat, null, context, false);
        }

        /// <summary>
        ///  Add the supplied statement to the specified contexts in the repository.
        /// </summary>
        /// <param name="statement">Statement object</param>
        /// <param name="contexts">named contexts</param>
        public void AddStatement(Statement statement)
        {
            AddTriple(statement.Subject, statement.Predicate, statement.Object, statement.Context);
        }

        /// <summary>
        ///     Add statement to this repository, 
        ///     optionally to one or more named contexts. 
        /// </summary>
        /// <param name="subj">subject</param>
        /// <param name="pred">predicate</param>
        /// <param name="obj">object</param>
        /// <param name="contexts">named contexts</param>
        public void AddStatement(string subj, string pred, string obj, string context = null)
        {
            AddTriple(subj, pred, obj, context);
        }

        /// <summary>
        ///     Add the supplied triple of values to this repository, 
        ///     optionally to one or more named contexts. 
        /// </summary>
        /// <param name="subj">subject</param>
        /// <param name="pred">predicate</param>
        /// <param name="obj">object</param>
        /// <param name="contexts">named context</param>
        public void AddTriple(string subj, string pred, string obj, string context = null)
        {
            string[][] statements = new string[1][]; ;
            if (context != null)
            {
                statements[0] = new string[] { subj, pred, obj, context };
            }
            else
            {

                statements[0] = new string[] { subj, pred, obj };
            }
            this.GetMiniRepository().AddStatements(statements);
        }

        /// <summary>
        ///    Add a new statement
        /// </summary>
        /// <param name="triples_or_quads">
        ///    A list of quadtuple, which consists of subject, predicate, object, and context
        /// </param>
        public void AddTriples(string[][] triples_or_quads)
        {
            this.GetMiniRepository().AddStatements(triples_or_quads);
        }

        /// <summary>
        ///     Removes the statement(s) with the specified subject, predicate and object
        ///     from the repository, optionally restricted to the specified contexts.
        /// </summary>
        /// <param name="subj">Subject</param>
        /// <param name="pred">Predicate</param>
        /// <param name="obj">Object</param>
        /// <param name="context">Context</param>
        /// <returns>The number of statements deleted</returns>
        public int RemoveTriples(string subj, string pred, string obj, string[] contexts = null)
        {
            if (contexts == null || contexts.Length == 0)
            {
                return this.GetMiniRepository().DeleteMatchingStatements(subj, pred, obj, null);
            }
            else
            {
                int count = 0;
                foreach (string context in contexts)
                {
                    count += this.GetMiniRepository().DeleteMatchingStatements(subj, pred, obj, context);
                }
                return count;
            }
        }

        /// <summary>
        /// Delete a statement in contexts
        /// </summary>
        /// <param name="statement">Target statement</param>
        /// <param name="contexts">Context(named graph)</param>
        /// <returns>The number of statements deleted</returns>
        public int RemoveStatement(Statement statement, string[] contexts = null)
        {
            return RemoveTriples(statement.Subject, statement.Predicate, statement.Object, contexts);
        }
        /// <summary>
        /// Remove all quads with matching IDs.
        /// </summary>
        /// <param name="tids">a list of triple/tuple IDs</param>
        /// <returns></returns>
        public void RemoveQuadsByID(string[] tids)
        {
            this.GetMiniRepository().DeleteStatementsById(tids);
        }

        /// <summary>
        /// Remove the given statements
        /// </summary>
        /// <param name="quads">Statements to be deleted</param>
        public void RemoveQuads(string[][] Quads)
        {
            this.GetMiniRepository().DeleteStatements(Quads);
        }

        /// <summary>
        /// Deletes all duplicate statements that are currently present in the store
        /// </summary>
        /// <param name="indexMode">The indexmode can be either spog (the default) or spo to indicate</param>
        public void RemoveDuplicateStatements(string indexMode = "spog")
        {
            this.GetMiniRepository().DeleteDuplicateStatements();
        }
        /// <summary>
        ///  Removes all statements from designated contexts in the repository.  
        ///  If 'contexts' is null, clears the repository of all statements.
        /// </summary>
        /// <param name="contexts"></param>
        public void Clear(string[] contexts = null)
        {
            RemoveTriples(null, null, null, contexts);
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
        public string EvalSPARQLQuery(string queryLanguage, string queryString, string contexts = null,
                                               string namedContexts = null, bool includeInferred = false,
                                               Dictionary<string, string> bindings = null, bool checkVariables = false,
                                               string infer = "false", int limit = -1, int offset = -1)
        {
            string queryResult = string.Empty;
            if (queryLanguage == QueryLanguage.SPARQL)
            {
                queryResult = this.GetMiniRepository().EvalSPARQLQuery(queryString,
                                                                     infer,
                                                                     contexts,
                                                                     namedContexts,
                                                                     bindings,
                                                                     checkVariables,
                                                                     limit,
                                                                     offset);
            }
            else
            {
                queryResult = this.GetMiniRepository().EvalPrologQuery(queryString, infer, limit);
            }
            return queryResult;
        }


        public BooleanQuery PrepareBooleanQuery(string queryLanguage, string queryString, string contexts = null,
                                                string namedContexts = null, bool includeInferred = false,
                                                Dictionary<string, string> bindings = null, bool checkVariables = false)
        {
            BooleanQuery bQuery = new BooleanQuery();
            bQuery.Querylanguage = queryLanguage;
            bQuery.QueryString = queryString;
            bQuery.Contexts = contexts;
            bQuery.NamedContexts = namedContexts;
            bQuery.IncludeInferred = includeInferred;
            bQuery.Bindings = bindings;
            bQuery.CheckVariables = checkVariables;
            bQuery.Connection = this;
            return bQuery;
        }

        public StringArrayQuery PrepareStringArrayQuery(string queryLanguage, string queryString, string contexts = null,
                                                string namedContexts = null, bool includeInferred = false,
                                                Dictionary<string, string> bindings = null, bool checkVariables = false)
        {
            StringArrayQuery sQuery = new StringArrayQuery();
            sQuery.Querylanguage = queryLanguage;
            sQuery.QueryString = queryString;
            sQuery.Contexts = contexts;
            sQuery.NamedContexts = namedContexts;
            sQuery.IncludeInferred = includeInferred;
            sQuery.Bindings = bindings;
            sQuery.CheckVariables = checkVariables;
            sQuery.Connection = this;
            return sQuery;
        }

        /// <summary>
        /// Exports all statements with a specific subject, predicate and/or object from the repository, 
        /// optionally from the specified contexts.
        /// </summary>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="type">represent the export file type.maybe N-Triple,N-Quads,RDF/XML,Trix</param>
        /// <param name="contexts"></param>
        /// <param name="infer"></param>
        public void ExportStatements(string[] subj, string[] pred, string[] obj, string type, string[] contexts = null, string infer = "false")
        {
            string[][] statements = this.GetStatements(subj, pred, obj, contexts, infer);
            if (type == "RDF/XML") type = ".xml";
            else if (type == "N-Triple") type = ".nt";
            else if (type == "N-Quads") type = ".np";
            else type = ".trix";
            string exportFile = this.GetMiniRepository().DatabaseName + type;
            //FileInfo export = new FileInfo(exportFile);
            Export2File(statements, exportFile, type);
        }

        public void Export(string type = "N-Triple", string[] contexts = null)
        {
            ExportStatements(null, null, null, type, contexts);
        }

        void Export2File(string[][] statements, string exportFile, string type)
        {
            //string fileName = "e:/" + exportFile;
            //Console.WriteLine(fileName);
            StreamWriter sw = new StreamWriter(new FileStream(exportFile, FileMode.CreateNew, FileAccess.Write));
            if (type.Equals(".nt") || type.Equals(".nq"))
            {
                for (int i = 0; i < statements.GetLength(0); i++)
                {
                    foreach (string s in statements[i])
                    {
                        sw.Write(s);
                        sw.Write(" ");
                    }
                    sw.Write(".");
                    sw.WriteLine();
                }
            }
            sw.Flush();
            sw.Close();
            //else if (type.Equals(".xml"))
            //{
            //}
            //else if (type.Equals(".trix"))
            //{
            //    sw.WriteLine("<?xml version=\"1.0\"?>");
            //    sw.WriteLine("<TriX xmlns=\"http://www.w3.org/2004/03/trix/trix-1/\">");
            //    sw.WriteLine("<graph>");
            //    sw.WriteLine("<default/>");
            //}
            //sw.WriteLine("</graph>");
            //sw.WriteLine("</TriX/>");
            //sw.Close();

        }
    }
}
