using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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
        /// Get the version of allegrograph server
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return AGRequestService.DoReqAndGet<string>(Server, "GET", "/version", null, false);
        }

        /// <summary>
        /// List all of the Catalog's name
        /// </summary>
        /// <returns></returns>
        public string[] ListCatalogs()
        {
            string result = AGRequestService.DoReqAndGet(Server, "GET", "/catalogs", null, false);
            JArray arr = JArray.Parse(result);
            string[] catalogs = new string[arr.Count];
            for (int i = 0; i < catalogs.Length; ++i)
                catalogs[i] = (string)arr[i]["id"];
            return catalogs;
        }

        /// <summary>
        /// Open Catalog
        /// </summary>
        /// <param name="Name">Catalog name</param>
        /// <returns>Catalog</returns>
        public AGCatalog OpenCatalog(string Name)
        {
            return new AGCatalog(Server, Name);
        }


        /// <summary>
        /// Open a session on a federated, reasoning, or filtered store.
        /// </summary>
        /// <returns></returns>
        public AGRepository OpenSession(string spec, bool autoCommit = false, int lifetime = -1, bool loadInitFile = false)
        {
            string param = string.Empty;
            if (lifetime == -1)
            {
                param = string.Format("/session?autoCommit={0}&loadInitFile={1}&store={2}", autoCommit, loadInitFile, spec);
            }
            else
            {
                param = string.Format("/session?autoCommit={0}&loadInitFile={1}&store={2}&lifetime={3}", autoCommit, loadInitFile, spec, lifetime);
            }
            //Console.WriteLine(param);
            string sessionUrl = AGRequestService.DoReqAndGet(this.Server, "POST", param);
            return new AGRepository(sessionUrl, Server.Username, Server.Password);
        }

        /// <summary>
        /// Get the initialization file contents
        /// </summary>
        /// <param name="Name">Catalog name</param>
        /// <returns>return opened Catalog</returns>
        public string GetInitFile()
        {
            return AGRequestService.DoReqAndGet(Server, "GET", "/initfile", null, false);
        }

        /// <summary>
        /// Replace the current initialization file contents with the
        /// 'content' string or remove if null. `restart`, which defaults
        /// to true, specifies whether any running shared back-ends should
        /// be shut down, so that subsequent requests will be handled by
        /// back-ends that include the new code.
        /// </summary>
        /// <param name="Name">Catalog name</param>
        /// <returns>return Catalog</returns>

        public void SetInitFile(string content = null, bool restart = true)
        {
            //Console.WriteLine(Server.Url + "/initfile");
            if (string.IsNullOrEmpty(content))
            {
                AGRequestService.DoReq(Server, "DELETE", "/initfile");
            }
            else
            {
                //AGRequestService.DoReq(Server, "PUT", "/initfile?" + "restart="+restart, content,true);
                AGRequestService.DoReq(Server, "PUT", string.Format("/initfile?restart={0}", restart), content, true);
            }
        }
    }
}
