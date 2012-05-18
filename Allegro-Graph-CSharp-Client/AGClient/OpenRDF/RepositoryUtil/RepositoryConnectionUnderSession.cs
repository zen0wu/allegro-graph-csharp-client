using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        /// Open a dedicated session. 
        /// </summary>
        /// <param name="spec">session url</param>
        /// <param name="autocommit">If autocommit is True, commits are done on each request, otherwise you will need to call commit() or rollback() as appropriate for your application.</param>
        /// <param name="lifetime">lifetime is an integer specifying the time to live in seconds of the session.</param>
        /// <param name="loadinitfile">If loadinitfile is True, then the current initfile will be loaded for you when the session starts.</param>
        public void OpenSession(string spec, bool autocommit = false, int lifetime = -1, bool loadinitfile = false)
        {
            _repository.OldUrl = _repository.Url;
            _repository.Url = _repository.GetMiniRepository().OpenSession(spec, autocommit, lifetime, loadinitfile);
        }

        /// <summary>
        /// Close a dedicated session connection. 
        /// </summary>
        public void CloseSession()
        {
            _repository.GetMiniRepository().CloseSession();
            _repository.Url = _repository.OldUrl;
        }

        /// <summary>
        /// Commit the current session
        /// </summary>
        public void Commit()
        {
            _repository.GetMiniRepository().Commit();
        }

        /// <summary>
        /// Rollback the current session
        /// </summary>
        public void Rollback()
        {
            _repository.GetMiniRepository().Rollback();
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
            _repository.GetMiniRepository().PreparingQueries(PQueryID,query,infer,context,namedContext,bindings,checkVariables,limit,offset);
        }

        /// <summary>
        ///  Executes a prepared query stored under the name id 
        /// </summary>
        /// <param name="PQueryID">prepared query id</param>
        public string ExecutePreparingQueries(string PQueryID)
        {
            return _repository.GetMiniRepository().ExecutePreparingQueries(PQueryID);
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
            _repository.GetMiniRepository().ExecutePreparingQueries(PQueryID, bindings, limit, offset);
        }

        /// <summary>
        /// Deletes the prepared query stored under id
        /// </summary>
        /// <param name="PQueryID">prepared query id</param>
        public void DeletePreparingQueries(string PQueryID)
        {
            _repository.GetMiniRepository().DeletePreparingQueries(PQueryID);
        }

        /// <summary>
        /// Define Prolog functors, 
        /// which can be used in Prolog queries. 
        /// This is only allowed when accessing a dedicated session. 
        /// </summary>
        /// <param name="prologFunction">prolog function content</param>
        public void DefinePrologFunction(string prologFunction)
        {
            _repository.GetMiniRepository().DefinePrologFunction(prologFunction);
        }

    }
}
