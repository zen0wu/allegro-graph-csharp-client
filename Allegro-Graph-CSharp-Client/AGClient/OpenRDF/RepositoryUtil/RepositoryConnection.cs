using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Rio;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public class RepositoryConnection
    {
        private Repository _repository;
        public RepositoryConnection(Repository repository)
        {
            this._repository = repository;
        }

        /// <summary>
        ///     Loads a file into the triple store,a file can be loaded into only one context     
        /// </summary>
        /// <param name="filePath">the file to load</param>
        /// <param name="baseUrl">baseURI associate with loading a file</param>
        /// <param name="format">RDFFormat.NTRIPLES or RDFFormat.RDFXML</param>
        /// <param name="context">an optional context URI (subgraph URI), defaulting to null.If null, the triple(s) will be added to the null context (the default or background graph).</param>
        /// <param name="serverSide"></param>
        public void AddFile(string filePath, string baseUrl = null, RDFFormat format = null, string context = null, bool serverSide = false)
        {
        }
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
            _repository.GetMiniRepository().AddStatements(statements);
        }

        public void AddTriples(string[][] triples_or_quads)
        {
            _repository.GetMiniRepository().AddStatements(triples_or_quads);
        }

        public int RemoveTriples(string subj, string pred,string obj,string contexts=null)
        {
            return _repository.GetMiniRepository().DeleteMatchingStatements(subj, pred, obj, contexts);
        }
        public int RemoveStatement(Statement statement, string contexts = null)
        {
            return _repository.GetMiniRepository().DeleteMatchingStatements(statement.Subject, statement.Predicate, statement.Object, contexts);
        }
        /// <summary>
        /// Remove all quads with matching IDs.
        /// </summary>
        /// <param name="tids">a list of triple/tuple IDs</param>
        /// <returns></returns>
        public void RemoveQuadsByID(string[] tids)
        {
            _repository.GetMiniRepository().DeleteStatementsById(tids);
        }

        public Dictionary<string, string> GetNamespaces()
        {
            return _repository.GetMiniRepository().ListNamespaces();
        }
    }
}
