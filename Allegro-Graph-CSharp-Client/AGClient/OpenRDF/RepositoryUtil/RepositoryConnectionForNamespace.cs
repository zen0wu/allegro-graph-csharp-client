using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        public List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace> GetNamespaces()
        {
            return this.GetMiniRepository().ListNamespaces();
        }

        public string GetNamespaces(string prefix)
        {
            return this.GetMiniRepository().GetNamespaces(prefix);
        }

        /// <summary>
        /// Sets the prefix for a namespace.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="name"></param>
        public void SetNamespace(string prefix, string name)
        {
            this.GetMiniRepository().AddNamespace(prefix, name);
        }

        /// <summary>
        /// Removes a namespace declaration by removing the association between a prefix and a namespace name
        /// </summary>
        /// <param name="prefix"></param>
        public void RemoveNamespace(string prefix)
        {
            this.GetMiniRepository().DeleteNamespace(prefix);
        }

        /// <summary>
        ///    Deletes all namespaces in this repository for the current user. If a
        ///    reset` argument of `True` is passed, the user's namespaces are reset
        ///    to the default set of namespaces, otherwise all namespaces are cleared.
        /// </summary>
        /// <param name="reset"></param>
        public void ClearNamespace(bool reset = true)
        {
            this.GetMiniRepository().ClearNamespaces(reset);
        }
    }
}
