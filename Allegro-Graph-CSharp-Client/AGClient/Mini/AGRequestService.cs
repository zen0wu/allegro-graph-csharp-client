using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Newtonsoft.Json;

using Allegro_Graph_CSharp_Client.AGClient.Util;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRequestService
    {

        private static string GenerateUrlParameters(Dictionary<string, object> parameters)
        {
            StringBuilder builder = new StringBuilder();
            Action<string, string> appendParameter = delegate(string key, string value) {
                if (builder.Length > 0)
                    builder.Append("&");
                builder.Append(key + "=" + value);
            };

            foreach (string key in parameters.Keys)
            {
                object value = parameters[key];
                if (value is string)
                    appendParameter(key, HttpUtility.UrlEncode(value as string));
                else if (value is string[])
                {
                    foreach (string v in value as string[])
                        appendParameter(key, HttpUtility.UrlEncode(v));
                }
                else
                    appendParameter(key, HttpUtility.UrlEncode(value.ToString()));
            }
            return builder.ToString();
        }

        private static void PrepareReq(IAGUrl Base, string Method, string RelativeUrl, object Body, out string AbsUrl, out string BodyString, out string ContentType)
        {
            AbsUrl = Base.Url + RelativeUrl;
            ContentType = "application/json; utf-8";
            BodyString = null;
            if (Body == null)
            {
                BodyString = null;
            }
            else if (Body is Dictionary<string, object>)
            {
                string parameters = GenerateUrlParameters((Dictionary<string, object>)Body);
                // GET方法将参数加入到URL中
                if (Method == "GET" || Method == "DELETE")
                {
                    if (parameters.Length > 0)
                        AbsUrl += "?" + parameters;
                }
                else if (Method == "POST" || Method == "PUT")
                    // POST方法将参数放到Body中
                    BodyString = parameters;
            }
            else if (Body is string)
            {
                ContentType = "plain/text; utf-8";
                BodyString = (string)Body;
            }
            else {
                // 其他情况，如果Body不是String，将其转化成JSON格式
                BodyString = JsonConvert.SerializeObject(Body);
            }
        }

        /// <summary>
        /// 执行HTTP请求，无返回
        /// </summary>
        /// <param name="Base">URL信息</param>
        /// <param name="Method">HTTP方法</param>
        /// <param name="RelativeUrl">相对URL</param>
        /// <param name="NeedsAuth">是否需要认证</param>
        /// <param name="Body">发送的内容, null表示不发送内容</param>
        public static void DoReq(IAGUrl Base, string Method, string RelativeUrl, object Body = null, bool NeedsAuth = true) 
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, out absUrl, out bodyString, out contentType);
            string username = null, password = null;
            if (NeedsAuth)
            {
                username = Base.Username;
                password = Base.Password;
            }

            RequestUtil.DoReq(absUrl, Method, bodyString, contentType, username, password);
        }

        /// <summary>
        /// 执行HTTP请求，返回JSON格式的结果
        /// </summary>
        /// <seealso cref="DoReq"/>
        public static string DoReqAndGet(IAGUrl Base, string Method, string RelativeUrl, object Body = null, bool NeedsAuth = true) 
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, out absUrl, out bodyString, out contentType);
            string username = null, password = null;
            if (NeedsAuth)
            {
                username = Base.Username;
                password = Base.Password;
            }

            return RequestUtil.DoJsonReq(absUrl, Method, bodyString, contentType, username, password);
        }

        /// <summary>
        /// 执行HTTP请求，返回JSON格式的结果，并将结果解析成T类型
        /// </summary>
        /// <seealso cref="DoReqAndGet"/>
        public static T DoReqAndGet<T>(IAGUrl Base, string Method, string RelativeUrl, object Body = null, bool NeedsAuth = true) 
        {
            string result = DoReqAndGet(Base, Method, RelativeUrl, Body, NeedsAuth);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
