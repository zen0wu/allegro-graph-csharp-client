using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class FreeTextIndex
    {

        /// <summary>
        ///An array of strings. Empty if the index indexes all predicates, containing only the predicates that are indexed otherwise. 
        /// </summary>
        public string[] predicates { get; set; }

        /// <summary>
        /// Can be true (index all literals), false (no literals), or an array of literal types to index. 
        /// </summary>
        public string indexLiterals { get; set; }

        /// <summary>
        ///Can be true (index resources fully), false (don't index resources), or the string "short" to index only the part after the last # or / in the resource. 
        /// </summary>
        public string indexResources { get; set; }

        /// <summary>
        ///An array containing any of the strings "subject", "predicate", "object", and "graph". This indicates which fields of a triple are indexed. 
        /// </summary>
        public string[] indexFields { get; set; }

        /// <summary>
        ///An integer, indicating the minimum size a word must have to be indexed. 
        /// </summary>
        public int minimumWordSize { get; set; }

        /// <summary>
        ///A list of words, indicating the words that count as stop-words, and should not be indexed. 
        /// </summary>
        public string[] stopWords { get; set; }

        /// <summary>
        /// A list of word filters configured for this index (see below).
        /// </summary>
        public string[] wordFilters { get; set; }

        /// <summary>
        ///A  list of character specifiers configured for this index (see below).
        /// </summary>
        public string[] innerChars { get; set; }

        /// <summary>
        /// A list of character specifiers configured for this index.
        /// </summary>
        public string[] borderChars { get; set; }

        /// <summary>
        /// The name of the tokenizer being used (currently either default or japanese). 
        /// </summary>
        public string tokenizer { get; set; }
    }
}
