using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
         /// <summary>
        /// Use free-text indices to search for the given pattern.
        /// </summary>
        /// <param name="pattern">The text to search for</param>
        /// <param name="expression">An S-expression combining search strings using and, or, phrase, match, and fuzzy. </param>
        /// <param name="index">
        ///   An optional parameter that restricts the search to a specific free-text index.
        ///   If not given, all available indexes are used
        /// </param>
        /// <param name="sorted"> 
        ///     indicating whether the results should be sorted by relevance. Default is false. 
        /// </param>
        /// <param name="limit">An integer limiting the amount of results that can be returned.</param>
        /// <param name="offset">An integer telling the server to skip the first few results</param>
        /// <returns>an array of statements</returns>
        public string[][] EvalFreeTextIndex(string pattern, string expression = null, string index = null, bool sorted = false, int limit = -1, int offset = -1)
        {
            return this._repository.GetMiniRepository().EvalFreeTextIndex(pattern, expression, index, sorted, limit, offset);
        }

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


        /// <summary>
        /// Modify a free-text index with the given parameters.
        /// </summary>
        /// <param name="name">string identifying the new index </param>
        /// <param name="predicates">If no predicates are given, triples are indexed regardless of predicate</param>
        /// <param name="indexLiterals">
        ///     IndexLiterals determines which literals to index.
        ///     It can be True (the default), False, or a list of resources, 
        ///     indicating the literal types that should be indexed
        /// </param>
        /// <param name="indexResources">
        ///     IndexResources determines which resources are indexed. 
        ///     It can be True, False (the default), or "short", 
        ///     to index only the part of resources after the last slash or hash character.
        /// </param>
        /// <param name="indexFields">
        ///     IndexFields can be a list containing any combination of the elements
        ///     "subject", "predicate", "object", and "graph".The default is ["object"]. 
        /// </param>
        /// <param name="minimumWordSize"> 
        ///     Determines the minimum size a word must have to be indexed.
        ///     The default is 3
        /// </param>
        /// <param name="stopWords">
        ///     StopWords should hold a list of words that should not be indexed. 
        ///     When not given, a list of common English words is used. 
        /// </param>
        /// <param name="wordFilters">
        ///     WordFilters can be used to apply some normalizing filters to words as they are indexed or queried.
        ///     Can be a list of filter names.  Currently, only "drop-accents" and "stem.english" are supported. 
        /// </param>
        /// <param name="innerChars">The character set to be used as the constituent characters of a word</param>
        /// <param name="borderChars"> The character set to be used as the border characters of indexed words. </param>
        /// <param name="tokenizer">An optional string. Can be either default or japanese.</param>
        public void ModifyTextIndex(string name, string[] predicates = null, object indexLiterals = null,
                                        string indexResources = "true", string[] indexFields = null,
                                        int minimumWordSize = -1, string[] stopWords = null,
                                        string[] wordFilters = null, char[] innerChars = null,
                                        char[] borderChars = null, string tokenizer = null)
        {
            this.GetMiniRepository().ModifyFreeTextIndex(name, predicates, indexLiterals, indexResources, indexFields, minimumWordSize, stopWords, wordFilters, innerChars, borderChars, tokenizer);

        }

        /// <summary>
        /// Delete named FreeTextIndex
        /// </summary>
        /// <param name="name">FreeTextIndex name</param>
        public void DeleteFreeTextIndex(string name)
        {
            this.GetMiniRepository().DeleteFreeTextIndex(name);
        }

        /// <summary>
        /// List the names of free-text indices defined in this repository.
        /// </summary>
        /// <returns>Free text index names</returns>
        public string[] ListFreeTextIndices()
        {
           return this.GetMiniRepository().ListFreeTextIndices();
        }


        /// <summary>
        /// Register a new free text predicate
        /// </summary>
        /// <param name="predicate">the URI of predicate</param>
        public void RegisterFreeTextPredicate(string predicate)
        {
            this.GetMiniRepository().RegisterFreeTextPredicate(predicate);
        }

        /// <summary>
        /// Returns the configuration parameter of the named free-text index
        /// </summary>
        /// <param name="indexName">Free text index name</param>
        /// <param name="paramName">parameter name</param>
        /// <returns></returns>
        public string GetFreeTextIndexConfiguration(string indexName,string paramName)
        {
            return this.GetMiniRepository().GetFreeTextIndexConfiguration(indexName,paramName);
        }


        /// <summary>
        ///     Returns a dictionary with fields "predicates",
        ///     "indexLiterals","indexResources","indexFields",
        ///     "minimumWordSize", "stopWords", and "wordFilters".
        /// </summary>
        /// <param name="index">Free text index name</param>
        /// <returns></returns>
        public Dictionary<string, string> GetFreeTextIndexConfiguration(string indexName)
        {
            return this.GetMiniRepository().GetFreeTextIndexConfiguration(indexName);
        }

        /// <summary>
        /// Returns the configuration parameters of the named free-text index
        /// </summary>
        /// <param name="indexName">Free text index name</param>
        /// <returns>FreeTextIndex object</returns>
        public FreeTextIndex GetFreeTextIndex(string indexName)
        {
            return _repository.GetMiniRepository().GetFreeTextIndex(indexName);
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
