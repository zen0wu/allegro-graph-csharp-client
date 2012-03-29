using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        /// List the namespaces of the current repository
        /// </summary>
        /// <returns>A list of namespace</returns>
        public List<Namespace> GetNamespaces()
        {
            return this.GetMiniRepository().ListNamespaces();
        }

        /// <summary>
        /// Returns the namespace URI defined for the given prefix. 
        /// </summary>
        /// <param name="prefix">The prefix of the namespace</param>
        /// <returns>The namespace's name</returns>
        public string GetNamespaces(string prefix)
        {
            return this.GetMiniRepository().GetNamespaces(prefix);
        }

        /// <summary>
        /// Create a new namespace
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="nsUrl">Namespace's URL</param>
        public void SetNamespace(string prefix, string nsUrl)
        {
            this.GetMiniRepository().AddNamespace(prefix, nsUrl);
        }

        /// <summary>
        /// Removes a namespace declaration by removing the association between a prefix and a namespace name
        /// </summary>
        /// <param name="prefix">namespace prefix</param>
        public void RemoveNamespace(string prefix)
        {
            this.GetMiniRepository().DeleteNamespace(prefix);
        }

        /// <summary>
        /// Deletes all namespaces in this repository for the current user.
        /// </summary>
        /// <param name="reset">
        /// If a reset argument of true is passed, the user's namespaces are reset to the default set of namespaces. 
        ///</param>
        public void ClearNamespace(bool reset = true)
        {
            this.GetMiniRepository().ClearNamespaces(reset);
        }
    }
}
