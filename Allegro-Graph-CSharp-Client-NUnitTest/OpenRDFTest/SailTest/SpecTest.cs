using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.SailTest
{
    [TestFixture]
    public class SpecTest
    {
        [Test]
        public void LocalTest()
        {
            string result = Spec.Local("temp");
            Console.WriteLine(result);
            result = Spec.Local("temp", "ACatalog");
            Console.WriteLine(result);
        }

        [Test]
        public void RemoteTest()
        {
            string result = Spec.Remote("temp");
            Console.WriteLine(result);
            result = Spec.Remote("temp", "ACatalog");
            Console.WriteLine(result);
        }

        [Test]
        public void UrlTest()
        {
            string result = Spec.Url("http://127.0.0.1:10035");
            Console.WriteLine(result);
        }
        
        [Test]
        public void FederateTest()
        {
            string[] stores = new string[]{"rep1","rep2","rep3"};
            string result = Spec.Federate(stores);
            Console.WriteLine(result);
        }

        [Test]
        public void ReasonTest()
        {
            string result = Spec.Reason("store");
            Console.WriteLine(result);
        }

        [Test]
        public void GraphFilterTest()
        {
            string[] graphs = new string[] { "graph1", null, "graph3" };
            string result = Spec.GraphFilter("rep", graphs);
            Console.WriteLine(result);
        }
    }
}
