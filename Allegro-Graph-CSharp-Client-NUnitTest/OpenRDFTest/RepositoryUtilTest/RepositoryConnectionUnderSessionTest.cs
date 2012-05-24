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
        //Repository repo;
        //RepositoryConnection repoConn;
        [Test]
        public void TestOpenCloseSession()
        {
            string oldUrl = repo.Url;
            repoConn.OpenSession(repo.GetSpec());
            Assert.AreEqual(oldUrl, repo.UrlBeforeSession);
            Assert.AreNotEqual(oldUrl, repo.Url);
            repoConn.CloseSession();
            Assert.AreEqual(oldUrl, repo.Url);
        }

        [Test]
        public void TestTransaction()
        {
            int bn = 10;
            repoConn.Clear();
            repoConn.OpenSession(repoConn.GetSpec());
            for (int i = 0; i < bn; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            repoConn.Rollback();
            for (int i = bn; i < bn * 2; ++i)
                repoConn.AddStatement(CreateSampleStatement(i));
            repoConn.Commit();
            repoConn.CloseSession();
            Assert.AreEqual(repoConn.GetSize(), bn);
        }

        [Test]
        public void TestPreparingQueriesAndExecute()
        {
            string pID = "testPre";
            string query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            repoConn.OpenSession(repo.GetSpec());
            repoConn.PreparingQueries(pID,query);
            //Console.WriteLine(repoConn.ExecutePreparingQueries(pID));
            Assert.IsNotEmpty(repoConn.ExecutePreparingQueries(pID));
            repoConn.CloseSession();
        }
        [Test]
        public void TestDeletePreparingQueries()
        {
            string pID = "testPre";
            string query = "select ?subj ?pred ?obj {?subj ?pred ?obj}";
            repoConn.OpenSession(repo.GetSpec());
            repoConn.PreparingQueries(pID, query);
            //Console.WriteLine(repoConn.ExecutePreparingQueries(pID));
            repoConn.DeletePreparingQueries(pID);
            Assert.IsEmpty(repoConn.ExecutePreparingQueries(pID));
            repoConn.CloseSession();
        }

    }
}
