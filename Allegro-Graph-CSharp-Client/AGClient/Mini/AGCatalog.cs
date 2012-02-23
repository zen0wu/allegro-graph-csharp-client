using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGCatalog : IAGUrl
    {
        private string CatalogUrl;
        private AGServerInfo Server;
        private string Name;

        public AGCatalog(AGServerInfo Server, string Name)
        {
            CatalogUrl = Server.Url + "/catalogs/" + Name;
            this.Name = Name;
            this.Server = Server;
        }

        public string Url { get { return CatalogUrl; } }
        public string Username { get { return Server.Username; } }
        public string Password { get { return Server.Password; } }

        /// <summary>
        /// 列出当前目录下的仓库
        /// </summary>
        public string[] ListRepositories()
        {
            string result = AGRequestService.DoReqAndGet(this, "GET", "/repositories", false);
            JArray arr = JArray.Parse(result);
            string[] repos = new string[arr.Count];
            for (int i = 0; i < repos.Length; ++i)
                repos[i] = (string)arr[i]["id"];
            return repos;
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="Name">仓库名</param>
        public void DeleteRepository(string Name)
        {
            AGRequestService.DoReq(Server, "DELETE", "/repositories/" + Name);
        }

        /// <summary>
        /// 创建仓库
        /// </summary>
        /// <param name="Name">仓库名</param>
        //public void CreateRepository(string Name)
        //{
        //    AGRequestService.DoReq(Server, "PUT", "/repositories/" + Name);
        //}
        public void CreateRepository(string Name)
        {
            AGRequestService.DoReq(this, "PUT", "/repositories/" + Name);
        }

        /// <summary>
        /// 打开仓库
        /// </summary>
        /// <param name="Name">仓库名</param>
        /// <returns>打开的仓库</returns>
        public AGRepository OpenRepository(string Name)
        {
            return new AGRepository(this, Name);
        }
    }
}
