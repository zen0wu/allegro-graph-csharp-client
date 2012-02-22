using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Util;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.MiniTest
{
    /// <summary>
    /// 测试AGClient类
    /// </summary>
    [TestFixture]
    class AGClientTest
    {
        private AGClient agClient;
        private AGServerInfo server;

        [TestFixtureSetUp]
        public void Init()
        {
            server = new AGServerInfo("http://172.16.2.21:10035", "chainyi", "chainyi123");
            agClient = new AGClient(server);
        }

        ///<summary>
        /// 测试构造函数以及同样的参数是否返回同样的对象
        ///<summary>
        [Test]
        public void SameObjectTest()
        {
            AGClient agClient1 = new AGClient(server);
            AGClient agClient2 = new AGClient(server);
            Assert.AreNotSame(agClient1, agClient2);
        }
        
        /// <summary>
        /// 测试 GetVersion()
        /// </summary>
        [Test]
        public void GetVersionTest()
        {
            string version = agClient.GetVersion();
            Console.Write(version);
            bool result = string.IsNullOrEmpty(version);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// 测试 ListCatalogs()
        /// </summary>
        [Test]
        public void ListCatalogsTest()
        {
            string[] catalogs = agClient.ListCatalogs();
            foreach (string catalog in catalogs)
            {
                Console.WriteLine(catalog);
            }
            Assert.IsNotEmpty(catalogs);
        }

        /// <summary>
        /// 测试 OpenCatalog()
        /// </summary>
        [Test]
        public void OpenCatalogTest()
        {
            string catalogName = "chainyi";
            //Assert.IsInstanceOf(Type.GetType("Allegro_Graph_CSharp_Client.AGClient.Mini.AGCatalog"), agClient.OpenCatalog(catalogName));
            bool result = agClient.OpenCatalog(catalogName) is AGCatalog;
            Assert.IsTrue(result);
        }
    }
}
