using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Allegro_Graph_CSharp_Client.AGClient.Util
{
    public class RequestUtil
    {
        private static void MakeReq(string Url, string Method, string Body, string ContentType, string Accept, out HttpStatusCode StatusCode, out string ReturnBody)
        {
            HttpWebRequest req = WebRequest.Create(Url) as HttpWebRequest;
            req.Method = Method;
            req.ContentType = ContentType;
            // Accept = null，代表此请求不需要返回值
            bool needsReturns = Accept != null;
            req.Accept = needsReturns ? Accept : "*/*";

            if (Body != null)
            {
                StreamWriter reqInWriter = new StreamWriter(req.GetRequestStream());
                reqInWriter.Write(Body);
                reqInWriter.Close();
                req.GetRequestStream().Close();
            }

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            StatusCode = resp.StatusCode;

            if (needsReturns)
            {
                StreamReader reqOutReader = new StreamReader(resp.GetResponseStream());
                ReturnBody = reqOutReader.ReadToEnd();
                reqOutReader.Close();
                reqOutReader.Close();
            }
            else
                ReturnBody = null;
        }

        /// <summary>
        /// 发起一个不需返回的HTTP请求
        /// </summary>
        /// <param name="Url">请求的URL</param>
        /// <param name="Method">请求的方法</param>
        /// <param name="Body">请求的内容</param>
        /// <param name="ContentType">请求内容的类型，默认为JSON</param>
        public static void DoReq(string Url, string Method, string Body, string ContentType = "application/json; utf-8")
        {
            HttpStatusCode status;
            string returnBody;
            MakeReq(Url, Method, Body, ContentType, null, out status, out returnBody);

            if ((int)status < 200 || (int)status > 204)
                throw new AGRequestException("Error while performing the request with error code " + (int)status);
        }

        /// <summary>
        /// 发起一个有返回值的请求，返回值的形式是JSON
        /// </summary>
        /// <param name="Url">请求的URL</param>
        /// <param name="Method">请求的方法</param>
        /// <param name="Body">请求的内容</param>
        /// <param name="ContentType">请求内容的类型，默认为JSON</param>
        /// <returns>请求的返回内容</returns>
        public static string DoJsonReq(string Url, string Method, string Body, string ContentType = "application/json; utf-8")
        {
            HttpStatusCode status;
            string returnBody;
            MakeReq(Url, Method, Body, ContentType, "application/json; utf-8", out status, out returnBody);

            if ((int)status < 200 || (int)status > 204)
                throw new AGRequestException("Error while performing the request with error code " + (int)status);

            return returnBody;
        }
    }
}
