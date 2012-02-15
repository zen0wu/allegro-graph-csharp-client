using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    [TestFixture]
    class AGRepositoryTest
    {
        ///<summary>
        /// 测试构造函数以及同样的参数是否返回同样的对象
        ///<summary>
        [Test]
        public void SameObjectTest()
        {
            string baseUrl = "http://172.16.2.21:10035";
            string username = "chainyi";
            string password = "chainyi123";
            AGServerInfo server = new AGServerInfo(baseUrl, username, password);
            string repositoryName = "◦zhishi291";
            AGRepository repository1 = new AGRepository(server, repositoryName);
            AGRepository repository2 = new AGRepository(server, repositoryName);
            Assert.AreNotSame(repository1, repository2);
        }
    }
}
