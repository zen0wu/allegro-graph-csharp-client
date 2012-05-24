using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    public partial class RepositoryConnectionTest
    {
        [Test]
        public void TestIndices()
        {
            string type = "spogi";
            repoConn.AddIndex(type);
            string[] indices = repoConn.ListIndices();
            Assert.True(indices.Any(e => e == type));
            indices = repoConn.ListValidIndices();
            Assert.True(indices.Any(e => e == type));
            repoConn.DropIndex(type);
        }

        [Test]
        public void TestOptimizeIndex()
        {
            repoConn.OptimizeIndex();
        }
    }
}
