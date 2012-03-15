using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        public void OpenSession(string spec, bool autocommit = false, int lifetime = -1, bool loadinitfile = false)
        {
            _repository.OldUrl = _repository.Url;
            _repository.Url = _repository.GetMiniRepository().OpenSession(spec, autocommit, lifetime, loadinitfile);
        }
        public void CloseSession()
        {
            _repository.GetMiniRepository().CloseSession();
            _repository.Url = _repository.OldUrl;
        }
        public void Commit()
        {
            _repository.GetMiniRepository().Commit();
        }
        public void Rollback()
        {
            _repository.GetMiniRepository().Rollback();
        }
    }
}
