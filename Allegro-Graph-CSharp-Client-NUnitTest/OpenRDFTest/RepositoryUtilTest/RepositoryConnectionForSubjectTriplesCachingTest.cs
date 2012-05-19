using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;
using NUnit.Framework;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    public partial class RepositoryConnectionTest
    {
        [Test]
        public void TestTripleCache()
        {
            repoConn.EnableTripleCache(20);
            int size = repoConn.GetTripleCacheSize();
            Assert.AreEqual(size, 20);

            repoConn.DisableTripleCache();
            size = repoConn.GetTripleCacheSize();
            Assert.AreEqual(size, 0);
        }

    }
}
