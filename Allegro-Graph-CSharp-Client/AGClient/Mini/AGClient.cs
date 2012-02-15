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

        /// <summary>
        /// 返回AG服务器的版本
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return AGRequestService.DoReqAndGet<string>(Server, "GET", "/version", false);
        }

        /// <summary>
        /// 列出所有的Catalog
        /// </summary>
        /// <returns></returns>
        public string[] ListCatalogs()
        {
            string result = AGRequestService.DoReqAndGet(Server, "GET", "/catalogs", false);
            JArray arr = JArray.Parse(result);
            string[] catalogs = new string[arr.Count];
            for (int i = 0; i < catalogs.Length; ++i)
                catalogs[i] = (string)arr[i]["id"];
            return catalogs;
        }

        /// <summary>
        /// 打开特定的Catalog
        /// </summary>
        /// <param name="Name">Catalog的唯一名字</param>
        /// <returns>返回打开的Catalog</returns>
        public AGCatalog OpenCatalog(string Name)
        {
            return new AGCatalog(Server, Name);
        }
    }
}
