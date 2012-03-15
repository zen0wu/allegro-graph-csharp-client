using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        ///     Create a free-text index with the given parameters.
        /// </summary>
        /// <param name="name"> a string identifying the new index</param>
        /// <param name="predicates">
        ///     An array of strings. 
        ///     Empty if the index indexes all predicates, 
        ///     containing only the predicates that are indexed otherwise
        /// </param>
        /// <param name="indexLiterals">
        ///     Can be true (index all literals), false (no literals), or an array of literal types to index.
        /// </param>
        /// <param name="indexResources">
        /// It can be True (the default), False, or a list of resources, 
        /// indicating the literal types that should be indexed.
        /// </param>
        /// <param name="indexFields">
        /// An array containing any of the strings "subject", "predicate", "object", and "graph".
        /// This indicates which fields of a triple are indexed. 
        /// </param>
        /// <param name="minimumWordSize">
        ///     An integer, indicating the minimum size a word must have to be indexed
        /// </param>
        /// <param name="stopWords">
        ///     A list of words, indicating the words that count as stop-words, and should not be indexed
        /// </param>
        /// <param name="wordFilters">
        ///     A list of word filters configured for this index 
        /// </param>
        /// <param name="innerChars">
        ///     A list of character specifiers configured for this index 
        /// </param>
        /// <param name="borderChars">
        ///     A list of character specifiers configured for this index. 
        /// </param>
        /// <param name="tokenizer">
        ///     The name of the tokenizer being used (currently either default or japanese). 
        /// </param>


        public void CreateFreeTextIndex(string name, string[] predicates = null, object indexLiterals = null,
                                        string indexResources = "true", string[] indexFields = null,
                                        int minimumWordSize = -1, string[] stopWords = null,
                                        string[] wordFilters = null, char[] innerChars = null,
                                        char[] borderChars = null, string tokenizer = null)
        {
            this.GetMiniRepository().CreateFreeTextIndex(name, predicates, indexLiterals, indexResources, indexFields, minimumWordSize, stopWords, wordFilters, innerChars, borderChars, tokenizer);
        }
        public void ModifyTextIndex(string name, string[] predicates = null, object indexLiterals = null,
                                        string indexResources = "true", string[] indexFields = null,
                                        int minimumWordSize = -1, string[] stopWords = null,
                                        string[] wordFilters = null, char[] innerChars = null,
                                        char[] borderChars = null, string tokenizer = null)
        {
            this.GetMiniRepository().ModifyFreeTextIndex(name, predicates, indexLiterals, indexResources, indexFields, minimumWordSize, stopWords, wordFilters, innerChars, borderChars, tokenizer);

        }

        public void DeleteFreeTextIndex(string name)
        {
            this.GetMiniRepository().DeleteFreeTextIndex(name);
        }

        /// <summary>
        /// List the names of free-text indices defined in this repository.
        /// </summary>
        /// <returns></returns>
        public string[] ListFreeTextIndices()
        {
           return this.GetMiniRepository().ListFreeTextIndices();
        }

        public void RegisterFreeTextPredicate(string predicate)
        {
            this.GetMiniRepository().RegisterFreeTextPredicate(predicate);
        }

        public Dictionary<string, string> GetFreeTextIndexConfiguration(string index)
        {
            return this.GetMiniRepository().GetFreeTextIndexConfiguration(index);
        }

        /// <summary>
        ///     Use free-text indices to search for the given pattern.
        ///     Returns an array of statements.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="infer"></param>
        /// <param name="limit"></param>
        /// <param name="index"> If no index is provided, all indices will be used. </param>
        public string[] EvalFreeTextSearch(string pattern, bool infer = false, int limit = -1, string[] indexs = null)
        {
            return this.GetMiniRepository().EvalFreeTextSearch(pattern, infer, limit, indexs);
        }
    }
}
