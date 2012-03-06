using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Util
{
    public class Uris
    {
        public static int GetLocalNameIndex(URI uri)
        {
            int idx = uri.Uri.LastIndexOf("#");
            if (idx < 0)
                idx = uri.Uri.LastIndexOf("/");
            if (idx < 0)
                idx = uri.Uri.LastIndexOf(":");
            if (idx < 0)
                throw new AGRequestException("No separator character found in URI: " + uri.Uri);
            return idx;
        }

        public static string AsURIString(object value)
        {
            string temp = value.ToString();
            if (temp.StartsWith("<"))
                return temp;
            else
                return string.Format("<{0}>", temp);
        }
    }
}
