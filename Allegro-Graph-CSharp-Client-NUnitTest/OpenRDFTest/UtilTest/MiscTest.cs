using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Util;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.UtilTest
{
    [TestFixture]
    public class MiscTest
    {
        [Test]
        public void UrlEncTest()
        {
            string baseUrl = "http://localhost:10035/";
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("k1", "v1");
            vars.Add("a1", new string[] { "v1", "v2", "v3" });
            vars.Add("c1", "值1");
            vars.Add("t1", "this is a string with space");
            Console.WriteLine(Misc.UrlEnc(baseUrl, vars));
        }
    }
}
