using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGClient
    {
        private AGServerInfo Server;

        public AGClient(AGServerInfo Server)
        {
            this.Server = Server;
        }

        public string GetVersion()
        {
            return AGRequestService.DoReqAndGet<string>(Server, "GET", "/version", false);
        }

        public string[] ListCatalogs()
        {
            string result = AGRequestService.DoReqAndGet(Server, "GET", "/catalogs", false);
            JArray arr = JArray.Parse(result);
            string[] catalogs = new string[arr.Count];
            for (int i = 0; i < catalogs.Length; ++i)
                catalogs[i] = (string)arr[i]["id"];
            return catalogs;
        }

        public AGCatalog OpenCatalog(string Name)
        {
            return new AGCatalog(Server, Name);
        }
    }
}
