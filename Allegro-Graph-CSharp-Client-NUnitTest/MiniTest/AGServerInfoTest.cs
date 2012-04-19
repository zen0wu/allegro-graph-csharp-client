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
    /// 测试AGServerInfo类
    /// </summary>
    class AGServerInfoTest
    {
        ///<summary>
        /// 测试构造函数以及同样的参数是否返回同样的对象
        ///<summary>
        [Test]
        public void SameObjectTest()
        {
            string BaseUrl = "http://172.16.2.21:10035";
            string Username = "chainyi";
            string Password = "chainyi123";
            AGServerInfo ag1 = new AGServerInfo(BaseUrl, Username, Password);
            AGServerInfo ag2 = new AGServerInfo(BaseUrl, Username, Password);
            Assert.AreNotSame(ag1, ag2);
        }
        [Test]
        public void TestGetStatementsWithTID()
        {
            string baseUrl = "http://172.16.2.21:10035";
            string username = "chainyi";
            string password = "chainyi123";
            AGServerInfo server = new AGServerInfo(baseUrl, username, password);
            string[][] results = AGRequestService.DoReqAndGet<string[][]>(server, "GET", "/catalogs/chainyi/repositories/CSharpClient/statements", "application/x-quints+json");
            for (int i = 0; i < results.Length; i++)
            {
                StringBuilder sb = new StringBuilder("Triple :\t");
                for (int j = 0; j < results[i].Length; j++)
                {
                    sb.Append(results[i][j]);
                    if (j != results[i].Length - 1)
                        sb.Append("\t");
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine(results[i].Length);
            }
        }

    }
}
