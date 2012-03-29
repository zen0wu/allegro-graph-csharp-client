using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class DataType
    {
        /// <summary>
        /// Datatype or predicate
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// The resource associated with the mapping
        /// </summary>
        public string Part { get; set; }
        /// <summary>
        /// Encoding fields.
        /// </summary>
        public string Encoding { get; set; }
    }
}
