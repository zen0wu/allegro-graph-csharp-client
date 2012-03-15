using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil
{
    public partial class RepositoryConnection
    {
        public void AddRules(string rules, string language = "PROLOG")
        {
            if (language.Equals(Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Query.QueryLanguage.PROLOG))
                this.GetMiniRepository().DefinePrologFunctors(rules);
            else
                throw new AGClient.Util.AGRequestException("Cannot add a rule because the rule language has not been set.");
        }

        public void loadRules(string fileName, string language = "PROLOG")
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
            string rules = sr.ReadToEnd();
            AddRules(rules, language);
        }
    }
}
