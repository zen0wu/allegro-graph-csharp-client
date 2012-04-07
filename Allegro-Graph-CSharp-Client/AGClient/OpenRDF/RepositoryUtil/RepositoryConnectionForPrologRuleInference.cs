using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        /// <summary>
        /// Add a sequence of one or more rules (in ASCII format).
        /// </summary>
        /// <param name="rules">
        /// rule declarations start with '<-' or '<--'. 
        /// The former appends a new rule; the latter overwrites any rule with the same predicate.
        /// </param>
        /// <param name="language">language defaults to QueryLanguage.PROLOG. </param>
        public void AddRules(string rules, string language = "PROLOG")
        {
            if (language.Equals(Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query.QueryLanguage.PROLOG))
                this.GetMiniRepository().DefinePrologFunctors(rules);
            else
                throw new AGClient.Util.AGRequestException("Cannot add a rule because the rule language has not been set.");
        }

        /// <summary>
        /// Load a file of rules.
        /// </summary>
        /// <param name="fileName">file is assumed to reside on the client machine.</param>
        /// <param name="language">defaults to QueryLanguage.PROLOG.</param>
        public void loadRules(string fileName, string language = "PROLOG")
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
            string rules = sr.ReadToEnd();
            AddRules(rules, language);
        }
    }
}
