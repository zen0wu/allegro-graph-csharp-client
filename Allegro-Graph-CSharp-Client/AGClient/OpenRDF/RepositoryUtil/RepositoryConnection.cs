﻿using System;
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

        public int Size(List<string> contexts = null)
        {
            if (contexts == null)
            {
                return _repository.GetMiniRepository().GetSize();
            }
            else if (contexts.Count == 1)
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

        public bool IsEmpty(List<string> contexts = null)
        {
            return Size(contexts) == 0;
        }

        public string GetSpec()
        {
            string catalogName = _repository.GetMiniRepository().GetCatalog().GetName();
            if (catalogName == null || catalogName == "/")
                catalogName = "";
            else
                catalogName += ":";
            return string.Format("<{0}{1}>", catalogName, _repository.GetDatabaseName());
        }

        public AGRepository GetMiniRepository()
        {
            return _repository.GetMiniRepository();
        }

        public string[] GetContextIDs()
        {
            return this.GetMiniRepository().ListContexts();
        }

        public string[][] GetStatements(string[] Subj, string[] Pred, string[] Obj, string[] Context,
                                        string Infer = "false", int Limit = -1, int Offset = -1)
        {
            return this.GetMiniRepository().GetStatements(Subj, Pred, Obj, Context, Infer, Limit, Offset);
        }


        public string[][] GetStatementsById(string ids, bool returnIDs = true)
        {
            return this.GetMiniRepository().GetStatementsById(ids, returnIDs);
        }

        public Statement CreateStatement(string subj, string pred, string obj, string context = null)
        {
            return ValueFactory.CreateStatement(subj, pred, obj, context);
        }

        public URI CreateURI(string uri = null, string nameSpace = null, string localname = null)
        {
            return ValueFactory.CreateURI(uri, nameSpace, localname);
        }

        public BNode CreateBNode(string nodeID = null)
        {
            return ValueFactory.GetBNode(nodeID);
        }

        /// <summary>
        ///  Loads a file into the triple store,a file can be loaded into only one context     
        /// </summary>
        /// <param name="filePath">the file to load</param>
        /// <param name="baseUrl">baseURI associate with loading a file</param>
        /// <param name="format">RDFFormat.NTRIPLES or RDFFormat.RDFXML</param>
        /// <param name="context">an optional context URI (subgraph URI), defaulting to null.If null, the triple(s) will be added to the null context (the default or background graph).</param>
        /// <param name="serverSide"></param>
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
        /// <param name="statement"></param>
        /// <param name="contexts"></param>
        public void AddStatement(Statement statement, string[] contexts = null)
        {
            AddTriple(statement.Subject, statement.Predicate, statement.Object, contexts);
        }

        public void AddStatement(string subj, string pred, string obj, string[] contexts = null)
        {
            AddTriple(subj, pred, obj, contexts);
        }

        /// <summary>
        ///     Add the supplied triple of values to this repository, optionally to
        ///     one or more named contexts. 
        /// </summary>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="contexts"></param>
        public void AddTriple(string subj, string pred, string obj, string[] contexts = null)
        {
            string[][] statements = null;
            if (contexts != null)
            {
                statements = new string[contexts.Length][];
                for (int i = 0; i < contexts.Length; i++)
                {
                    statements[i] = new string[] { subj, pred, obj, contexts[i] };
                }
            }
            else
            {
                statements = new string[1][];
                statements[0] = new string[] { subj, pred, obj };
            }
            this.GetMiniRepository().AddStatements(statements);
        }

        public void AddTriples(string[][] triples_or_quads)
        {
            this.GetMiniRepository().AddStatements(triples_or_quads);
        }

        /// <summary>
        ///     Removes the statement(s) with the specified subject, predicate and object
        ///     from the repository, optionally restricted to the specified contexts.
        /// </summary>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="contexts"></param>
        /// <returns></returns>
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

        public void RemoveQuads(string[][] Quads)
        {
            this.GetMiniRepository().DeleteStatements(Quads);
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