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
        /// Fetches a result set of currently specified mappings. 
        /// </summary>
        /// <returns>List<DataType></returns>
        public List<DataType> ListTypeMapping()
        {
            return this.GetMiniRepository().ListTypeMapping();
        }

        /// <summary>
        /// Yields a list of literal types for which datatype mappings have been defined in this store.
        /// </summary>
        /// <returns>The set of type</returns>
        public string[] ListMappedTypes()
        {
            return this.GetMiniRepository().ListMappedTypes();
        }

        /// <summary>
       /// Clear type mappings for this repository. 
       /// </summary>
       /// <param name="isAll">
        ///      if true Clear all type mappings for this repository including the automatic ones.
        ///      else Clear all non-automatic type mappings for this repository. 
       /// </param>
        public void ClearTypeMapping(bool isAll = false)
        {
            this.GetMiniRepository().ClearNamespaces(isAll);
        }

        /// <summary>
        /// Takes two arguments, type (the RDF literal type) and encoding, 
        /// and defines a datatype mapping from the first to the second
        /// </summary>
        /// <param name="type">the RDF literal type</param>
        /// <param name="encoding">Encoding</param>
        public void AddMappedType(string type, string encoding)
        {
            this.GetMiniRepository().AddMappedType(type, encoding);
        }

        /// <summary>
        /// Deletes a datatype mapping
        /// </summary>
        /// <param name="type">type should be an RDF resource</param>
        public void DeleteMappedType(string type)
        {
            this.GetMiniRepository().DeleteMappedType(type);
        }

        /// <summary>
        /// Yields a list of literal types for which predicate mappings have been defined in this store. 
        /// </summary>
        /// <returns></returns>
        public string[] ListMappedPredicates()
        {
            return this.GetMiniRepository().ListMappedPredicates();
        }

        /// <summary>
        /// Takes two arguments, predicate and encoding, and defines a predicate mapping on them. 
        /// </summary>
        /// <param name="predicate">predicate</param>
        /// <param name="encoding">encoding</param>
        public void AddMappedPredicate(string predicate, string encoding)
        {
            this.GetMiniRepository().AddMappedPredicate(predicate, encoding);
        }

        /// <summary>
        /// Deletes a predicate mapping. Takes one parameter, predicate. 
        /// </summary>
        /// <param name="predicate">predicate</param>
        public void DeleteMappedPredicate(string predicate)
        {
            this.GetMiniRepository().DeleteMappedPredicate(predicate);
        }
    }
}
