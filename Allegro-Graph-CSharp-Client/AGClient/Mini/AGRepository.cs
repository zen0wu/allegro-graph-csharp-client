using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Constructor for OpenSession
        /// </summary>
        /// <param name="repoUrl">Session Url</param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public AGRepository(string repoUrl, string userName, string password)
        {
            this.RepoUrl = repoUrl;
            this.Catalog = new AGCatalog(new AGServerInfo(repoUrl, userName, password), null);
        }

        public string Url
        {
            get { return RepoUrl; }
            set { RepoUrl = value; }
        }
        public string Username { get { return Catalog.Username; } }
        public string Password { get { return Catalog.Password; } }
        public string DatabaseName { get { return this.Name; } }

        /// <summary>
        /// 返回仓库的大小
        /// </summary>
        /// <param name="Context">可以指定一个Named Graph作为Context</param>
        /// <returns>仓库的大小</returns>
        public int GetSize(string Context = null)
        {
            Dictionary<string, object> parameters = null;
            if (Context != null)
            {
                parameters = new Dictionary<string, object>();
                parameters.Add("context", Context);
            }
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/size", parameters);
        }

        public AGCatalog GetCatalog()
        {
            return this.Catalog;
        }

        public string[] ListContexts()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/contexts");
        }

        public string[] GetBlankNodes(int amount = 1)
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "POST", "/blankNodes", string.Format("amount={0}", amount));
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
            Dictionary<string, object> body = new Dictionary<string, object>();
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
        public void DeleteStatements(string[][] Quads)
        {
            AGRequestService.DoReq(this, "POST", "/statements/delete", Quads);
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
        public DataTable EvalSPARQLQuery(string Query, string Infer = "false", string Context = null, string NamedContext = null,
            Dictionary<string, string> Bindings = null, bool CheckVariables = false, int Limit = -1, int Offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
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

            JObject rawResult = JObject.Parse(AGRequestService.DoReqAndGet(this, "GET", "", parameters));
            JArray headers = rawResult["names"] as JArray;
            JArray contents = rawResult["values"] as JArray;

            DataTable resultTable = new DataTable();
            foreach (JToken columnName in headers)
                resultTable.Columns.Add(new DataColumn(columnName.Value<string>()));

            foreach (JArray rowObj in contents)
            {
                DataRow aRow = resultTable.NewRow();
                int index = 0;
                foreach (JToken cell in rowObj)
                    aRow[index++] = cell.Value<string>();
                resultTable.Rows.Add(aRow);
            }

            return resultTable;
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
        public string[][] GetStatements(string[] Subj, string[] Pred, string[] Obj, string[] Context, string Infer = "false",
            int Limit = -1, int Offset = -1)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Action<string, string[]> addArrayParam = delegate(string Key, string[] param)
            {
                if (param != null && param.Length > 0)
                {
                    if (param.Length == 1)
                        parameters.Add(Key, param[0]);
                    else
                        parameters.Add(Key, param);
                }
            };
            addArrayParam("subj", Subj);
            addArrayParam("pred", Pred);
            addArrayParam("obj", Obj);
            addArrayParam("context", Context);
            if (Limit >= 0) parameters.Add("limit", Limit.ToString());
            if (Offset >= 0) parameters.Add("offset", Offset.ToString());
            return AGRequestService.DoReqAndGet<string[][]>(this, "GET", "/statements", parameters);
        }

        public string[][] GetStatementsById(string ids, bool returnIDs = true)
        {
            string accept = string.Empty;
            if (returnIDs)
            {
                accept = "application/x-quints+json";
            }
            else
            {
                accept = "application/json";
            }
            return AGRequestService.DoReqAndGet<string[][]>(this, "GET", "/statements/id/id=" + ids, accept, null, true);
        }

        public void DeleteStatementsById(string[] ids)
        {
            AGRequestService.DoReq(this, "POST", "/statements/delete?ids=true", JsonConvert.SerializeObject(ids));
        }

        public List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace> ListNamespaces()
        {
            return AGRequestService.DoReqAndGet<List<Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model.Namespace>>(this, "GET", "/namespaces");
        }

        public string GetNamespaces(string prefix)
        {
            return AGRequestService.DoReqAndGet<string>(this, "GET", "/namespaces/" + prefix);
        }

        public void AddNamespace(string prefix, string name)
        {
            AGRequestService.DoReq(this, "PUT", "/namespaces/" + prefix, "text/plain", null, true);
        }

        public void DeleteNamespace(string prefix)
        {
            AGRequestService.DoReq(this, "DELETE", "/namespaces/" + prefix);
        }

        public void ClearNamespaces(bool reset = true)
        {
            AGRequestService.DoReq(this, "DELETE", string.Format("/namespaces/reset={0}", reset));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        /// <param name="baseUrl"></param>
        /// <param name="context"></param>
        /// <param name="serverSide"></param>
        public void LoadFile(string filePath, string format, string baseUrl = null, string context = null, bool serverSide = false)
        {
            string contentType = string.Empty;
            if (format == "ntriples")
            {
                contentType = "text/plain";
            }
            else if (format == "rdf/xml")
            {
                contentType = "application/rdf+xml";
            }
            string fileContent = string.Empty;
            if (!serverSide)
            {
                StreamReader sr = new StreamReader(filePath);
                fileContent = sr.ReadToEnd();
                sr.Close();
                filePath = null;
            }
            string vars = string.Format("file={0}&context={1}&baseUrl={2}", HttpUtility.UrlEncode(filePath), HttpUtility.UrlEncode(context), HttpUtility.UrlEncode(baseUrl));
            string relativeUrl = "/statements?" + vars;
            AGRequestService.DoReq(this, "POST", relativeUrl, contentType, fileContent);
        }

        public string[] ListIndices()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/indices");
        }
        public string[] ListValidIndices()
        {
            return AGRequestService.DoReqAndGet<string[]>(this, "GET", "/indices?listValid=true");
        }
        public void AddIndex(string indexType)
        {
            AGRequestService.DoReq(this, "PUT", "/indices/" + indexType);
        }
        public void DropIndex(string indexType)
        {
            AGRequestService.DoReq(this, "DELETE", "/indices/" + indexType);
        }

        public string OpenSession(string spec, bool autocommit = false, int lifetime = -1, bool loadinitfile = false)
        {
            string relativeUrl = string.Empty;
            if (lifetime == -1)
            {
                relativeUrl = string.Format("/session?autoCommit={0},loadInitFile={1},store={2}", autocommit, loadinitfile, spec);
            }
            else
            {
                relativeUrl = string.Format("/session?autoCommit={0}, lifetime={1},loadInitFile={2}, store={3}", autocommit, lifetime, loadinitfile, spec);
            }
            return AGRequestService.DoReqAndGet<string>(this, "POST", relativeUrl);
            //return new AGRepository(sessionUrl, this.Username, this.Password);
        }
        public void CloseSession()
        {
            try
            {
                AGRequestService.DoReq(this, "POST", "/session/close");
            }
            catch { }
        }
        public void Commit()
        {
            AGRequestService.DoReq(this, "POST", "/session/commit");
        }
        public void Rollback()
        {
            AGRequestService.DoReq(this, "POST", "/rollback");
        }

        public void EnableTripleCache(int size = -1)
        {
            string queryUrl = string.Empty;
            if (size == -1)
            {
                queryUrl = string.Format("/tripleCache");
            }
            else
            {
                queryUrl = string.Format("/tripleCache?size={0}", size);
            }
            AGRequestService.DoReq(this, "PUT", queryUrl);
        }

        public void DisableTripleCache()
        {
            AGRequestService.DoReq(this, "DELETE", "/tripleCache");
        }

        public int GetTripleCacheSize()
        {
            return AGRequestService.DoReqAndGet<int>(this, "GET", "/tripleCache");
        }
    }
}
