using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class ValueFactory
    {
        public static BNode GetBNode(string nodeID = null)
        {
            if (nodeID == null)
            {
                return new BNode((new Random()).Next().ToString());
            }
            else
            {
                return new BNode(nodeID);
            }
        }
        /// <summary>
        ///     Create a new statement with the supplied subject, predicate and object
        //      and associated context. 
        /// </summary>
        /// <param name="subj">subject</param>
        /// <param name="pred">predicate</param>
        /// <param name="obj">object</param>
        /// <param name="context">context</param>
        /// <returns></returns>
        public static Statement CreateStatement(string subj, string pred, string obj, string context = null)
        {
            return new Statement(subj, pred, obj, context);
        }
        /// <summary>
        ///     Creates a new URI from the supplied string-representation(s).
        //      If two non-keyword arguments are passed, assumes they represent a
        //      namespace/localname pair.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="nameSpace"></param>
        /// <param name="localname"></param>
        /// <returns></returns>
        public static URI CreateURI(string uri,string nameSpace,string localname)
        {
            return new URI(uri, nameSpace, localname);  
        }

        public static string CreateGeoLiteral(string literal, string literalType)
        {
            return string.Format("\"{0}\"^^{1}",literal,literalType);
        }
    }
}
