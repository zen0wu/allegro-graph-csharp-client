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
                // If it's "GET" or "DELETE", put the parameters into the URL
                if (Method == "GET" || Method == "DELETE")
                {
                    if (parameters.Length > 0)
                        AbsUrl += "?" + parameters;
                }
                else if (Method == "POST" || Method == "PUT")
                    // Else, parameters will go into the body
                    BodyString = parameters;
            }
            else if (Body is string)
            {
                ContentType = "plain/text; utf-8";
                BodyString = (string)Body;
            }
            else {
                // If body is not string, jsonize it
                BodyString = JsonConvert.SerializeObject(Body);
            }
        }

        /// <summary>
        /// Execute a non-returning HTTP request
        /// </summary>
        /// <param name="Base">URL</param>
        /// <param name="Method">HTTP Method</param>
        /// <param name="RelativeUrl">Relative URL to the Base</param>
        /// <param name="NeedsAuth">If authorization is needed, default true</param>
        /// <param name="Body">HTTP Body</param>
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
        /// DoReq with specific content-type
        /// <seealso cref="DoReq"/>
        /// </summary>
        public static void DoReq(IAGUrl Base, string Method, string RelativeUrl, string ContentType, object Body = null, bool NeedsAuth = true)
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, out absUrl, out bodyString, out contentType);
            string username = null, password = null;
            if (NeedsAuth)
            {
                username = Base.Username;
                password = Base.Password;
            }
            contentType = ContentType;
            RequestUtil.DoReq(absUrl, Method, bodyString, contentType, username, password);
        }

        /// <summary>
        /// Make a HTTP request with return value in the JSON format
        /// </summary>
        /// <seealso cref="DoReq"/>
        public static string DoReqAndGet(IAGUrl Base, string Method, string RelativeUrl, object Body = null, bool NeedsAuth = true) 
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, out absUrl, out bodyString, out contentType);
            //Console.WriteLine(absUrl);
            string username = null, password = null;
            if (NeedsAuth)
            {
                username = Base.Username;
                password = Base.Password;
            }

            return RequestUtil.DoJsonReq(absUrl, Method, bodyString, contentType, username, password);
        }

        /// <summary>
        /// Make a HTTP request with return value in the JSON format and deserialize into type T
        /// </summary>
        /// <seealso cref="DoReqAndGet"/>
        public static T DoReqAndGet<T>(IAGUrl Base, string Method, string RelativeUrl, object Body = null, bool NeedsAuth = true) 
        {
            string result = DoReqAndGet(Base, Method, RelativeUrl, Body, NeedsAuth);
            return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// DoReqAndGet with specific accept headers
        /// <seealso cref="DoReqAndGet"/>
        /// </summary>
        public static string DoReqAndGet(IAGUrl Base, string Method, string RelativeUrl, string Accept,object Body = null, bool NeedsAuth = true)
        {
            string absUrl, contentType, bodyString;
            PrepareReq(Base, Method, RelativeUrl, Body, out absUrl, out bodyString, out contentType);
            //Console.WriteLine(absUrl);
            string username = null, password = null;
            if (NeedsAuth)
            {
                username = Base.Username;
                password = Base.Password;
            }
            return RequestUtil.DoJsonReq(absUrl, Method, bodyString,Accept, contentType, username, password);
        }

        /// <summary>
        /// DoReqAndGet with specific accept headers and type T
        /// <seealso cref="DoReqAndGet"/>
        /// </summary>
        public static T DoReqAndGet<T>(IAGUrl Base, string Method, string RelativeUrl, string Accept, object Body = null, bool NeedsAuth = true)
        {
            string result = DoReqAndGet(Base, Method, RelativeUrl,Accept, Body, NeedsAuth);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
