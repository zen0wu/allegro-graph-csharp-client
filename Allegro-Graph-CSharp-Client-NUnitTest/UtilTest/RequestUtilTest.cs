﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;

namespace Allegro_Graph_CSharp_Client_NUnitTest.UtilTest
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
        [TestFixtureSetUp]
        public void Init()
        {
            url = "http://172.16.2.21:10035/";
            method = "GET"; //默认使用get方式
            body =null;
        }

        ///<summary>
        ///测试异常AGRequestException
        /// </summary>
        /// 测试失败：异常在MakeReq()就抛出，不会在DoReq()中捕捉
        [Test]
        [ExpectedException(typeof(Allegro_Graph_CSharp_Client.AGClient.Util.AGRequestException))]
        public void DoReqAGRequestExceptionTest()
        {
            url = "http://172.16.2.21:10035/test"; //不存在的url,产生404错误
            RequestUtil.DoReq(url,method,body);
            Assert.Fail("Should have gotten an AGRequestException");
        }

        ///<summary>
        ///函数DoJsonReq测试，获取返回的returnBody
        /// </summary>
        /// 
        [Test]
        public void DoReqGetReturnBodyTest()
        {
            url = "http://172.16.2.21:10035"; 
            string returnBody =  RequestUtil.DoJsonReq(url, method, body,"text/html");
            Assert.Pass(returnBody);
        }

        /// <summary>
        /// 使用GET方法调用DoReq
        /// </summary>
        /// 测试失败：测试通不过，产生ProtocolViolationException
        /// GET模式下，不能通过RequestStream发送数据
        [Test]
        //[ExpectedException(typeof(System.Net.ProtocolViolationException))]
        public void DoReqWithGetMethodTest()
        {
            method = "GET";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用POST方法调用DoReq
        /// </summary>        
        [Test]
        public void DoReqWithPostMethodTest()
        {
            method = "POST";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用PUT方法调用DoReq
        /// </summary>        
        [Test]
        public void DoReqWithPutMethodTest()
        {
            method = "PUT";
            RequestUtil.DoReq(url, method, body);
        }

        /// <summary>
        /// 使用不正确的Url调用DoReq
        /// </summary> 
        [Test]
        public void DoReqWithWrongUrlTest()
        {
            method = "POST";
            url = "172.16.2.21:10035";
            RequestUtil.DoReq(url, method, body);
        }

    }
}
