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
        public override bool Equals(object obj)
        {
            if (obj is DataType)
            {
                DataType dtObj = obj as DataType;
                if (this.Kind == dtObj.Kind && this.Part == dtObj.Part && this.Encoding == dtObj.Encoding)
                    return true;
                else
                    return false;
            }
            else
                return base.Equals(obj);
        }
    }
}
