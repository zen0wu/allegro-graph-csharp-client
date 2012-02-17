using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Allegro_Graph_CSharp_Client.AGClient.Util;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRepository : IAGUrl
    {
        private string RepoUrl;
        private AGServerInfo Server;
        private string Name;

        public AGRepository(AGServerInfo Server, string Name)
        {
            RepoUrl = Server.Url + "/repositories/" + Name;
            this.Name = Name;
            this.Server = Server;
        }

        public string Url { get { return RepoUrl; } }
        public string Username { get { return Server.Username; } }
        public string Password { get { return Server.Password; } }

        /// <summary>
        /// 返回仓库的大小
        /// </summary>
        /// <param name="Context">可以指定一个Named Graph作为Context</param>
        /// <returns>仓库的大小</returns>
        public int GetSize(string Context)
        {
            Dictionary<string, string> parameters = null;
            if (Context != null) 
            {
                parameters = new Dictionary<string, string>();
                parameters.Add("context", Context);
            }
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/size", parameters);
        }

        /// <summary>
        /// 增加三元组
        /// </summary>
        /// <param name="Quads">每个元素长度为4，分别为主语、谓语、宾语、上下文</param>
        public void AddStatements(string[,] Quads)
        {
            if (Quads.GetLength(1) != 4)
                throw new AGRequestException("Parameters in AddStatements must be of type [,4]");
            AGRequestService.DoReq(this, "POST", "/statememts", JsonConvert.SerializeObject(Quads));
        }

        /// <summary>
        /// 删除给定条件下的元组
        /// </summary>
        /// <param name="Subj">主语</param>
        /// <param name="Pred">谓语</param>
        /// <param name="Obj">宾语</param>
        /// <param name="Context">上下文</param>
        /// <returns></returns>
        public int DeleteMatchingStatements(string Subj, string Pred, string Obj, string Context)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            if (Subj != null)
                body.Add("subj", Subj);
            if (Pred != null)
                body.Add("pred", Pred);
            if (Obj != null)
                body.Add("obj", Obj);
            if (Context != null)
                body.Add("context", Context);
            return AGRequestService.DoReqAndGet<int>(this, "DELETE", "/statements", body);
        }

        /// <summary>
        /// 删除给定的元组
        /// </summary>
        /// <param name="Quads">待删除的元组</param>
        public void DeleteStatements(string[,] Quads)
        {
            if (Quads.GetLength(1) != 4)
                throw new AGRequestException("Parameters in DeleteStatements must be of type [,4]");
            AGRequestService.DoReq(this, "POST", "/statememts/delete", JsonConvert.SerializeObject(Quads));
        }

        /// <summary>
        /// 删除给定Id的元组
        /// </summary>
        /// <param name="Ids">给定的Id</param>
        public void DeleteStatementsById(string[] Ids)
        {
            AGRequestService.DoReq(this, "POST", "/statememts/delete?ids=true", JsonConvert.SerializeObject(Ids));
        }
    }
}
