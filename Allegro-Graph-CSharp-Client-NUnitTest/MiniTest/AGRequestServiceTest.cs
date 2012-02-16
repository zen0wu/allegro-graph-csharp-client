using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    /// <summary>
    /// 测试AGRequestService类
    /// </summary>
    
    [TestFixture]
    class AGRequestServiceTest
    {
        private AGServerInfo server;
        [TestFixtureSetUp]
        public void Init()
        {
            string baseUrl = "http://172.16.2.21:10035";
            string username = "chainyi";
            string password = "chainyi123";
            server = new AGServerInfo(baseUrl, username, password);
        }

        /// <summary>
        /// 测试DoReq
        /// </summary>
        /// 
        [Test]
        public void DoReqTest()
        {
            string method = "GET";
            string relativeUrl = "/catalogs/chainyi/repositories/test/statements";
            AGRequestService.DoReq(server, method, relativeUrl);

        }

        [Test]
        public void DoReqAndGetTest()
        {
            string method = "GET";
            string relativeUrl = "/catalogs/chainyi/repositories/test/statements";
            string result = AGRequestService.DoReqAndGet(server, method, relativeUrl);
            Assert.Pass(result);
        }
    }
}
