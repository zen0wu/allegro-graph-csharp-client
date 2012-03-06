using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Util
{
    public class Misc
    {
        public static string UrlEnc(string baseUrl,Dictionary<string,object> vars)
        {
            StringBuilder url = new StringBuilder();
            url.Append(baseUrl+"?");
            bool isFirst = true;
            foreach (string key in vars.Keys)
            {
                if (isFirst)
                {
                    url.Append(string.Format("{0}={1}", key, HttpUtility.HtmlEncode(vars[key])));
                    isFirst = false;
                }
                else
                {
                    url.Append(string.Format("&{0}={1}", key, HttpUtility.HtmlEncode(vars[key])));
                }
            }
            return url.ToString();
        }
    }
}
