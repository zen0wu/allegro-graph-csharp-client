using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        public string[] ListIndices()
        {
            return _repository.GetMiniRepository().ListIndices();
        }

        public string[] ListValidIndices()
        {
            return _repository.GetMiniRepository().ListValidIndices();
        }

        public void AddIndex(string indexType)
        {
            _repository.GetMiniRepository().AddIndex(indexType);
        }
        public void DropIndex(string indexType)
        {
            _repository.GetMiniRepository().DropIndex(indexType);
        }
    }
}
