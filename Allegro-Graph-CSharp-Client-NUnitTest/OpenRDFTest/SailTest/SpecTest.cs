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
        public void TestLocal()
        {
            string result = Spec.Local("temp");
            Assert.AreEqual(result, "<temp>");
            result = Spec.Local("temp", "ACatalog");
            Assert.AreEqual(result, "<ACatalog:temp>");
        }

        [Test]
        public void TestRemote()
        {
            string result = Spec.Remote("temp");
            Assert.AreEqual(result, "<http://localhost:10035/repositories/temp>");
            result = Spec.Remote("temp", "ACatalog");
            Assert.AreEqual(result, "<http://localhost:10035/catalogs/ACatalog/repositories/temp>");
        }

        [Test]
        public void TestUrl()
        {
            string result = Spec.Url("http://127.0.0.1:10035");
            Assert.AreEqual(result, "<http://127.0.0.1:10035>");
        }
        
        [Test]
        public void TestFederate()
        {
            string[] stores = new string[]{"rep1","rep2","rep3"};
            string result = Spec.Federate(stores);
            Assert.AreEqual(result, "<rep1>+<rep2>+<rep3>");
        }

        [Test]
        public void TestReason()
        {
            string result = Spec.Reason("store");
            Assert.AreEqual(result, "<store>[rdf++]");
        }

        [Test]
        public void TestGraphFilter()
        {
            string[] graphs = new string[] { "graph1", null, "graph3" };
            string result = Spec.GraphFilter("rep", graphs);
            Assert.AreEqual(result, "<rep>{<graph1> null <graph3>} ");
        }
    }
}
