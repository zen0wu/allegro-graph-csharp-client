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
        private static string exns="http://example.org/people/";
        private static string alice = string.Format("<{0}#{1}>", exns, "alice");
        private static string fullName = string.Format("<{0}#{1}>", exns, "fullname");
        private static string aliceName = "\"Alice B. Toklas\"";

        //[TestFixtureSetUp]
        //public void InitInFreeTextIndex()
        //{
        //    repoConn.AddStatement(alice, fullName, aliceName);
        //}
        //[TestFixtureTearDown]

        [Test]
        public void TestEvalFreeTextIndex()
        {
            repoConn.AddStatement(alice, fullName, aliceName);
            repoConn.CreateFreeTextIndex("freeTextIndex1", new String[]{"alice"});
            string[][] result = repoConn.EvalFreeTextIndex("alice");
            for (int i = 0; i < result.Length; i++)
            {
                foreach (string s in result[i])
                {
                    Console.WriteLine(s);
                }
            }
            repoConn.DeleteFreeTextIndex("freeTextIndex1");
            repoConn.RemoveTriples(null, null, aliceName);
            //Console.WriteLine((repoConn.GetStatements(null, null, null, null)).Length);
        }
        [Test]
        public void TestEvalFreeTextSearch()
        {
            repoConn.AddStatement(alice, fullName, aliceName);
            string[] result = repoConn.EvalFreeTextSearch("alice");
            foreach (string s in result) Console.WriteLine(s);
            repoConn.RemoveTriples(null, null, aliceName);
        }

        [Test]
        public void TestCreateDeleteFreeTextIndex()
        {
            string[] predicates = new string[1]{ string.Format("<{0}#{1}>", exns, "fullname")};
            repoConn.CreateFreeTextIndex("freeTextIndex1", predicates);
            Assert.Contains("freeTextIndex1", repoConn.ListFreeTextIndices());
            repoConn.DeleteFreeTextIndex("freeTextIndex1");
            Assert.IsFalse(repoConn.ListFreeTextIndices().Contains("freeTextIndex1"));
        }

        [Test]
        public void TestListFreeTextIndices()
        {
            string[] predicates = new string[1] { string.Format("<{0}#{1}>", exns, "fullname") };
            repoConn.CreateFreeTextIndex("freeTextIndex1", predicates);
            string[] freeTIndices = repoConn.ListFreeTextIndices();
            Assert.Greater(freeTIndices.Count(), 0);
            repoConn.DeleteFreeTextIndex("freeTextIndex1");
        }
        
    }
}
