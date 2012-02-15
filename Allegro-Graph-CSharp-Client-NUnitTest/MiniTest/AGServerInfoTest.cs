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

    }
}
