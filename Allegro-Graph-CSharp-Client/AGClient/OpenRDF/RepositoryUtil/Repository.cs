using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegro_Graph_CSharp_Client.AGClient.Mini;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public enum AccessVerb { RENEW, ACCESS, OPEN, CREATE, REPLACE };
    public class Repository
    {
        private AGRepository _agRepository;
        private Catalog _catalog;

        public Catalog catalog { get { return this._catalog; } }

        public Repository(Catalog catalog, string name)
        {
            _catalog = catalog;
            _agRepository = new AGRepository(catalog.agCatalog, name);
        }

        public Repository(AGRepository agRepository, string name = null)
        {
            _agRepository = agRepository;
        }

        public RepositoryConnection GetConnection()
        {
            return new RepositoryConnection(this);
        }

        public AGRepository GetMiniRepository()
        {
            return this._agRepository;
        }
        

        /// <summary>
        ///  Return the name of the database (remote triple store) that this repository is interfacing with.
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return _agRepository.DatabaseName;
        }

        public string GetSpec()
        {
            string catName = this.catalog.GetName();
            if (catName == null || catName == "/")
            {
                return Spec.Local(this.GetDatabaseName());
            }
            else
            {
                return Spec.Local(this.GetDatabaseName(), catName);
            }
        }
        /// <summary>
        ///   Initializes this repository. A repository needs to be initialized before
        ///   it can be used.  Return 'this' (so that we can chain this call if we like).
        /// </summary>
        /// <returns></returns>
        public Repository Initialize()
        {
            return this;
        }
        /// <summary>
        ///     Register an inlined datatype. 
        ///     Predicate is the URI of predicate used in the triple store. 
        ///     Datatype may be one of: XMLSchema.INT, XMLSchema.LONG, XMLSchema.FLOAT, XMLSchema.DATE, and XMLSchema.DATETIME.  
        ///     NativeType may be "int", "datetime", or "float".
        ///     You must supply nativeType and either predicate or datatype. 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="datatype"></param>
        /// <param name="nativeType"></param>
        //public void RegisterDatatypeMapping(string predicate = null, string datatype = null, string nativeType = null)
        //{
        //}

        /// <summary>
        ///   Shuts the store down, releasing any resources that it keeps hold of.
        ///   Once shut down, the store can no longer be used.
        /// </summary>
        public void ShutDown(){
            this._agRepository = null;
        }
    }
}
