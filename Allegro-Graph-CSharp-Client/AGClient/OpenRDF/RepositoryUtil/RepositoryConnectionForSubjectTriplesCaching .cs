using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {

        /// <summary>
        /// Enable the triple cache
        /// </summary>
        /// <param name="size">Triple cache size</param>
        public void EnableTripleCache(int size = -1)
        {
            _repository.GetMiniRepository().EnableTripleCache(size);
        }

        /// <summary>
        /// Disable the spogi cache for this repository. 
        /// </summary>
        public void DisableTripleCache()
        {
            _repository.GetMiniRepository().DisableTripleCache();
        }

        /// <summary>
        /// Find out whether the 'SPOGI cache' is enabled, 
        /// and what size it has. Returns an integer 
        /// 0 when the cache is disabled, the size of the cache otherwise. 
        /// </summary>
        public int GetTripleCacheSize()
        {
            return _repository.GetMiniRepository().GetTripleCacheSize();
        }
    }
}
