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
        private AGServerInfo server;
        private AGCatalog catalog;
        private AGRepository repository;
        
        [Test]
        [TestFixtureSetUp]
        public void init()
        {
            //对于非root catalog，访问时要增加catalog的路径,是否增加个成员表示repository所在的catalog
            string baseUrl = "http://172.16.2.21:10035"; 
            string username = "chainyi";
            string password = "chainyi123";
            server = new AGServerInfo(baseUrl, username, password);
            catalog = new AGCatalog(server, "chainyi");
            string repositoryName = "CSharpClient";
            repository = new AGRepository(catalog, repositoryName);
            Console.WriteLine(repository.Url);
        }

        ///<summary>
        /// 测试构造函数以及同样的参数是否返回同样的对象
        ///<summary>
        [Test]
        public void SameObjectTest()
        {
            string repositoryName = "CSharpClient";
            AGRepository repository1 = new AGRepository(catalog, repositoryName);
            AGRepository repository2 = new AGRepository(catalog, repositoryName);
            Assert.AreNotSame(repository1, repository2);
        }

        /// <summary>
        /// 测试 GetSize()
        /// </summary>
        [Test]
        public void GetSizeTest()
        {
            int size = repository.GetSize(null);
            Console.Write(size);
            Assert.IsNotNull(size);
        }
        
        /// <summary>
        /// 测试 AddStatements()
        /// </summary>
        [Test]
        public void AddStatementsTest()
        {
            string[][] quads = new string[1][] {
                new string[4] { "<http://example/q?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/q?abc=1&def=2>" ,"<http://example/q?abc=1&def=2>" } };
            repository.AddStatements(quads);
        }

        /// <summary>
        /// 测试 AddStatements()
        /// </summary>
        [Test]
        [ExpectedException(typeof(AGRequestException))]
        public void AddStatementsTest2()
        {
            string[][] quads = new string[1][] {
                new string[3] { "<http://example/q?abc=1&def=2>", "<http://www.w3.org/1999/02/22-rdf-syntax-ns#value>", "<http://example/q?abc=1&def=2>" } };
            repository.AddStatements(quads);
        }
    }
}
