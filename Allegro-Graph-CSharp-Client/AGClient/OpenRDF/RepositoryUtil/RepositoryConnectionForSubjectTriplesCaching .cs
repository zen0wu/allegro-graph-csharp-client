using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        public void EnableTripleCache(int size = -1)
        {
            _repository.GetMiniRepository().EnableTripleCache();
        }

        public void DisableTripleCache()
        {
            _repository.GetMiniRepository().DisableTripleCache();
        }

        public int GetTripleCacheSize()
        {
            return _repository.GetMiniRepository().GetTripleCacheSize();
        }
    }
}
