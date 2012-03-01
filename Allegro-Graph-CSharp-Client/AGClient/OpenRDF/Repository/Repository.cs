using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Repository
{
    public enum AccessVerb { RENEW, ACCESS, OPEN, CREATE, REPLACE };
    public class Repository
    {
        private AGRepository _agRepository;
        public Repository(Catalog catalog, string name)
        {
            _agRepository = new AGRepository(catalog.agCatalog, name);
        }

    }
}
