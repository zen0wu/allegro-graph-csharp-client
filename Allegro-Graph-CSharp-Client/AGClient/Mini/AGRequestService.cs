using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Allegro_Graph_CSharp_Client.AGClient.Util;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRequestService
    {
        private static void PrepareReq(IAGUrl Base, string Method, string RelativeUrl, object Body, bool NeedsUserInfo, out string AbsUrl, out string BodyString, out string ContentType)
        {
            AbsUrl = Base.Url + RelativeUrl;
            ContentType = "application/json; utf-8";
            BodyString = "";
            if (Body == null)
            {
                BodyString = null;
            }
            else if (Method == "GET" && Body is Dictionary<string, string>)
            {
                // GET方法将参数加入到URL中
                bool isFirst = true;
                Dictionary<string, string> parameters = (Dictionary<string, string>)Body;
                foreach (string key in parameters.Keys)
                {
                    AbsUrl += (isFirst ? "?" : "&");
                    AbsUrl += key + "=" + parameters[key];
                    isFirst = false;
                }
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

            // 添加用户名密码
            if (NeedsUserInfo)
                AbsUrl += ";" + Base.Username + ";" + Base.Password;
        }

        public static void DoReq(IAGUrl Base, string Method, string RelativeUrl, bool NeedsUserInfo = true, object Body = null) 
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, NeedsUserInfo, out absUrl, out bodyString, out contentType);
            RequestUtil.DoReq(absUrl, Method, bodyString, contentType);
        }

        public static string DoReqAndGet(IAGUrl Base, string Method, string RelativeUrl, bool NeedsUserInfo = true, object Body = null) 
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, NeedsUserInfo, out absUrl, out bodyString, out contentType);

            return RequestUtil.DoJsonReq(absUrl, Method, bodyString, contentType);
        }

        public static T DoReqAndGet<T>(IAGUrl Base, string Method, string RelativeUrl, bool NeedsUserInfo = true, object Body = null) 
        {
            string result = DoReqAndGet(Base, Method, RelativeUrl, NeedsUserInfo, Body);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
