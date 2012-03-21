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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server Object</param>
        /// <param name="name">Catalog name, returns the root catalog if name is null</param>
        /// <returns></returns>
        public AGCatalog(AGServerInfo server, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                CatalogUrl = server.Url + "/";
            }
            else
            {
                CatalogUrl = server.Url + "/catalogs/" + name;
            }
            this.Name = name;
            this.Server = server;
        }

        public string Url { get { return CatalogUrl; } }
        public string Username { get { return Server.Username; } }
        public string Password { get { return Server.Password; } }

        /// <summary>
        /// List the repositories in the current catalog
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
        /// Delete a repository
        /// </summary>
        /// <param name="name">Repository name</param>
        public void DeleteRepository(string name)
        {
            //AGRequestService.DoReq(Server, "DELETE", "/repositories/" + Name);
            AGRequestService.DoReq(this, "DELETE", "/repositories/" + name);
        }

        /// <summary>
        /// Create a repository
        /// </summary>
        /// <param name="name">Repository name</param>
        public void CreateRepository(string name)
        {
            AGRequestService.DoReq(this, "PUT", "/repositories/" + name,null,true);
        }

        /// <summary>
        /// Open a repository
        /// </summary>
        /// <param name="name">Repository name</param>
        /// <returns>The opened repository</returns>
        public AGRepository OpenRepository(string name)
        {
            return new AGRepository(this, name);
        }

        /// <summary>
        /// Get the name of current catalog
        /// </summary>
        /// <returns>catalog name</returns>
        public string GetName()
        {
            return this.Name;
        }
    }
}
