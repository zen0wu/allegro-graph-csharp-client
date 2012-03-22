using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        ///     Creates a new generator under the given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subjectOf">
        ///     A predicate. Accepted multiple times. Causes the new generator to follow edges with the given predicate
        /// </param>
        /// <param name="objectOf">
        ///     Like objectOf, but follow edges from object to subject
        /// </param>
        /// <param name="undirected">
        ///     Like the above, but follow edges in both directions.
        /// </param>
        /// <param name="query">
        ///     A Prolog query, in which the variable ?node can be used to refer to the 'start' node, 
        ///     and whose results will be used as 'resulting' nodes.
        ///     User namespaces may be used in this query
        /// </param>
        public void RegisterSNAGenerator(string name, string[] subjectOf = null, string[] objectOf = null, string[] undirected = null, string query = null)
        {
            this.GetMiniRepository().RegisterSNAGenerator(name, subjectOf, objectOf, undirected, query);
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
            this.GetMiniRepository().RegisterNeighborMatrix(name, group, generator, depth);
        }
    }
}
