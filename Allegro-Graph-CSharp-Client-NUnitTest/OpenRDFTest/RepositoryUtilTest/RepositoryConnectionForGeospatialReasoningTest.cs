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
        public void TestListGeoTypes()
        {
            string[] geoTypes = repoConn.ListGeoTypes();
            foreach (string s in geoTypes)
                Console.WriteLine(s);
        }

        [Test]
        public void TestSetSphericalGeoType()
        {
            string tagetType = "\"<http://franz.com/ns/allegrograph/3.0/geospatial/spherical/degrees/-180.0/180.0/-90.0/90.0/2.0>\"";
            string geoType = repoConn.SetSphericalGeoType(2);
            Assert.AreEqual(geoType, tagetType);
        }

        [Test]
        public void TestSetCartesianGeoType()
        {
            string tagetType = "\"<http://franz.com/ns/allegrograph/3.0/geospatial/cartesian/1.0/20.0/1.0/20.0/2.0>\"";
            string geoType = repoConn.SetCartesianGeoType(2,1,20,1,20);
            Assert.AreEqual(geoType, tagetType);
        }
       
        [Test]
        public void TestGetStatementsOfGeo()
        {
            string prefix = "http://example.org/people/";
            string predicate = string.Format("<{0}{1}>",prefix,"location");
            string subj1 = string.Format("<{0}{1}>", prefix, "alice");
            string subj2 = string.Format("<{0}{1}>", prefix, "tom");
            string cartesianGeoType1 = "<http://franz.com/ns/allegrograph/3.0/geospatial/cartesian/1.0/20.0/1.0/20.0/2.0>";
            string litSubj1Loc = ValueFactory.CreateGeoLiteral("+10.0+10.0", cartesianGeoType1);

            //Console.WriteLine("{0}:{1}:{2}",subj1,predicate,litSubj1Loc);

            //TestCase-1
            repoConn.AddStatement(subj1, predicate, litSubj1Loc);
            List<Statement> stats = repoConn.GetStatementsInsideBox(string.Format("{0}", cartesianGeoType1), predicate, 1, 20, 1, 20);
            foreach (Statement stat in stats)
            {
                Console.WriteLine(stat.Subject);
            }

            repoConn.RemoveTriples(null, predicate, null);
        }
    }
}
