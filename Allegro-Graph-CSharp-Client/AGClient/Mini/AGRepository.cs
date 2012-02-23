using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Newtonsoft.Json;
using Allegro_Graph_CSharp_Client.AGClient.Util;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRepository : IAGUrl
    {
        private string RepoUrl;
        private AGCatalog Catalog;
        private string Name;

        public AGRepository(AGCatalog Catalog, string Name)
        {
            RepoUrl = Catalog.Url + "/repositories/" + Name;
            this.Name = Name;
            this.Catalog = Catalog;
        }

        public string Url { get { return RepoUrl; } }
        public string Username { get { return Catalog.Username; } }
        public string Password { get { return Catalog.Password; } }

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
        public void AddStatements(string[][] Quads)
        {
            AGRequestService.DoReq(this, "POST", "/statements", Quads);
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
            AGRequestService.DoReq(this, "POST", "/statements/delete", JsonConvert.SerializeObject(Quads));
        }

        /// <summary>
        /// 删除给定Id的元组
        /// </summary>
        /// <param name="Ids">给定的Id</param>
        public void DeleteStatementsById(string[] Ids)
        {
            AGRequestService.DoReq(this, "POST", "/statements/delete?ids=true", JsonConvert.SerializeObject(Ids));
        }

        /// <summary>
        /// 执行Sparql查询
        /// </summary>
        /// <param name="Query">查询</param>
        /// <param name="Infer">推理类型</param>
        /// <param name="Context">上下文</param>
        /// <param name="NamedContext">命名的上下文</param>
        /// <param name="Bindings">已知的变量绑定</param>
        /// <param name="CheckVariables">是否检查不存在的变量</param>
        /// <param name="Limit">返回结果的最多个数</param>
        /// <param name="Offset">跳过部分返回结果</param>
        /// <returns></returns>
        public string EvalSPARQLQuery(string Query, string Infer = "false", string Context = null, string NamedContext = null,
            Dictionary<string, string> Bindings = null, bool CheckVariables = false, int Limit = -1, int Offset = -1)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("query", Query);
            parameters.Add("queryLn", "sparql");
            parameters.Add("infer", Infer);
            if (Context != null) parameters.Add("context", Context);
            if (NamedContext != null) parameters.Add("namedContext", NamedContext);
            if (Bindings != null)
            {
                foreach (string vari in Bindings.Keys) 
                    parameters.Add("$" + vari, HttpUtility.UrlEncode(Bindings[vari]));
            }
            parameters.Add("checkVariables", CheckVariables.ToString());
            if (Limit >= 0) parameters.Add("limit", Limit.ToString());
            if (Offset >= 0) parameters.Add("offset", Offset.ToString());
            return AGRequestService.DoReqAndGet(this, "GET", "", parameters);
        }

        /// <summary>
        /// 返回给定条件的triples
        /// </summary>
        /// <param name="Subj">主语限制</param>
        /// <param name="Pred">谓语限制</param>
        /// <param name="Obj">宾语限制</param>
        /// <param name="Context">上下文限制</param>
        /// <param name="Infer">推理类型，默认为false，即不推理</param>
        /// <param name="Limit">返回结果的最多个数</param>
        /// <param name="Offset">跳过部分返回结果</param>
        /// <returns></returns>
        public string GetStatements(string[] Subj, string[] Pred, string[] Obj, string[] Context, string Infer = "false", 
            int Limit = -1, int Offset = -1)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Action<string, string[]> addArrayParam = delegate(string Key, string[] param)
            {
                if (param.Length > 0)
                {
                    if (param.Length == 1)
                        parameters.Add(Key, param[0]);
                    else
                        parameters.Add(Key, JsonConvert.SerializeObject(param));
                }
            };
            addArrayParam("subj", Subj);
            addArrayParam("pred", Pred);
            addArrayParam("obj", Obj);
            addArrayParam("context", Context);
            if (Limit >= 0) parameters.Add("limit", Limit.ToString());
            if (Offset >= 0) parameters.Add("offset", Offset.ToString());
            return AGRequestService.DoReqAndGet(this, "GET", "/statements", parameters);
        }

        /*
         * 目前的问题
         * 上面两个函数的返回类型
         * subjEnd的作用
         * subj参数能不能用数组传递
        */
    }
}
