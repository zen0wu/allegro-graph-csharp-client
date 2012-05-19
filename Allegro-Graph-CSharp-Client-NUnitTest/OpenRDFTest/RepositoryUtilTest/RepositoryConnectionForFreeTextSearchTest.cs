using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    public partial class RepositoryConnectionTest
    {
        private static string exns = "http://example.org/people/";
        private static string alice = string.Format("<{0}#{1}>", exns, "Alice");
        private static string fullName = string.Format("<{0}#{1}>", exns, "fullname");
        private static string aliceName = "\"Alice B. Toklas\"";

        private static string tom = string.Format("<{0}#{1}>", exns, "Tom");
        private static string tomName = "\"Tom f. Feng\"";

        [Test]
        public void TestEvalFreeTextIndex()
        {
            repoConn.AddStatement(alice, fullName, aliceName);
            string pattern = "alice";
            repoConn.CreateFreeTextIndex("freeTextIndex1", new String[] { pattern });

            string[][] result = repoConn.EvalFreeTextIndex(pattern);
            bool isFound = false;
            for (int i = 0; i < result.Length; i++)
            {
                foreach (string s in result[i])
                {
                    if (s.ToLower().Contains(pattern))
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }
            Assert.IsTrue(isFound);
            repoConn.DeleteFreeTextIndex("freeTextIndex1");
            repoConn.RemoveTriples(null, null, null);
        }

        [Test]
        public void TestModifyTextIndex()
        {
            repoConn.AddStatement(tom, fullName, tomName);
            string pattern = "alice";
            repoConn.CreateFreeTextIndex("freeTextIndex1", new String[] { pattern });

            string[][] result = repoConn.EvalFreeTextIndex(pattern);
            bool isFound = false;
            for (int i = 0; i < result.Length; i++)
            {
                foreach (string s in result[i])
                {
                    if (s.ToLower().Contains(pattern))
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }
            Assert.IsFalse(isFound);

            pattern = "tom";
            repoConn.ModifyTextIndex("freeTextIndex1", new String[] { pattern });
            result = repoConn.EvalFreeTextIndex(pattern);
            isFound = false;
            for (int i = 0; i < result.Length; i++)
            {
                foreach (string s in result[i])
                {
                    if (s.ToLower().Contains(pattern))
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;
            }
            Assert.IsTrue(isFound);
            repoConn.DeleteFreeTextIndex("freeTextIndex1");
            repoConn.Clear();
        }

        [Test]
        public void TestCreateDeleteFreeTextIndex()
        {
            string[] predicates = new string[1] { string.Format("<{0}#{1}>", exns, "fullname") };
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

        [Test]
        public void TestGetFreeTextIndexConfiguration()
        {
            string pattern1 = "alice";
            string pattern2 = "tom";
            string indexName = "freeTextIndex1";
            repoConn.CreateFreeTextIndex(indexName, new String[] { pattern1, pattern2 }, "xxx", "true");

            FreeTextIndex freeTextIndex = repoConn.GetFreeTextIndexConfiguration(indexName);
            //string result = AGRequestService.DoReqAndGet(repoConn.GetMiniRepository(), "GET", "/freetext/indices/" + indexName);
            Assert.Contains(pattern1, freeTextIndex.predicates);

            string oneStopWords = "are";
            string paramValue = repoConn.GetFreeTextIndexConfiguration(indexName, "stopWords");
            Assert.IsTrue(paramValue.Contains(oneStopWords));
            repoConn.DeleteFreeTextIndex(indexName);
        }

    }
}
