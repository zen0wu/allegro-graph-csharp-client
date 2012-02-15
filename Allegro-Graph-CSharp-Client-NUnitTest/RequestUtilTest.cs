﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using System.Net;

namespace Allegro_Graph_CSharp_Client_NUnitTest
{
    [TestFixture]
    class RequestUtilTest
    {
        /// <summary>
        /// 测试DoReq函数
        /// </summary>

        private string url;    //请求的URL
        private string method; //请求的方法
        private string body;   //请求的内容

        //初始化参数
        [Test]
        [SetUp]
        public void Init()
        {
            url = "http://localhost:45678/index.aspx";
            method = "";
            body = "";
        }

        /// <summary>
        /// 使用GET方法调用DoReq
        /// </summary>
        /// 测试说明：测试通不过，产生ProtocolViolationException
        [Test]
        //[ExpectedException(typeof(System.Net.ProtocolViolationException))]
        public void TestDoReqWithGetMethod()
        {
            method = "GET";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用POST方法调用DoReq
        /// </summary>        
        [Test]
        public void TestDoReqWithPostMethod()
        {
            method = "POST";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用PUT方法调用DoReq
        /// </summary>        
        [Test]
        public void TestDoReqWithPutMethod()
        {
            method = "PUT";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用不正确的Url调用DoReq
        /// </summary> 
        [Test]
        public void TestDoReqWithWrongUrl()
        {
            method = "POST";
            url = "localhost:45678/index.aspx";
            RequestUtil.DoReq(url, method, body);
        }

    }
}
