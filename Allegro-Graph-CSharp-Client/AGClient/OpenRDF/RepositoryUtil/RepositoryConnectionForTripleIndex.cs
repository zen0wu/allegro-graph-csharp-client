using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        /// List the valid indices
        /// </summary>
        /// <returns>set of index</returns>
        public string[] ListIndices()
        {
            return this.GetMiniRepository().ListIndices();
        }

        /// <summary>
        /// List the valid indices
        /// </summary>
        /// <returns>set of index</returns>
        public string[] ListValidIndices()
        {
            return this.GetMiniRepository().ListValidIndices();
        }

        /// <summary>
        /// Ensures that the index indicated by type is present in this store
        /// </summary>
        /// <param name="indexType">Index type</param>
        public void AddIndex(string indexType)
        {
            this.GetMiniRepository().AddIndex(indexType);
        }

        /// <summary>
        /// Removes the index indicated by type from the store
        /// </summary>
        /// <param name="indexType">index type to drop</param>
        public void DropIndex(string indexType)
        {
            this.GetMiniRepository().DropIndex(indexType);
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
            this.GetMiniRepository().OptimizeIndex(wait, level);
        }
    }

}
