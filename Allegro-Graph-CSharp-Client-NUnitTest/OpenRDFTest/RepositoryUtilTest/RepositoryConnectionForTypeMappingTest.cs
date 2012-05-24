using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Sail;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;
using NUnit.Framework;
using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client_NUnitTest.OpenRDFTest.RepositoryUtilTest
{
    public partial class RepositoryConnectionTest
    {

        [Test]
        public void TestListTypeMapping()
        {
            DataType testDataType = new DataType();
            testDataType.Kind = "datatype";
            testDataType.Part = "<http://www.w3.org/2001/XMLSchema#int>";
            testDataType.Encoding = "<http://www.w3.org/2001/XMLSchema#int>";
            List<DataType> typeMappings = repoConn.ListTypeMapping();
            Assert.Greater(typeMappings.Count(), 0);
            Assert.Contains(testDataType, typeMappings);
        }
        [Test]
        public void TestListMappedTypes()
        {
            string intType = "<http://www.w3.org/2001/XMLSchema#int>";
            string[] types = repoConn.ListMappedTypes();
            Assert.Greater(types.Count(), 0);
            Assert.Contains(intType, types);
        }

        [Test]
        public void TestAddDeleteMappedType()
        {
            string exns = "http://www.example.com/";
            string ageType = string.Format("<{0}{1}>", exns, "myInteger");
            string intEncoding = "<http://www.w3.org/2001/XMLSchema#int>";
            //ageType = HttpUtility.UrlEncode(ageType);
            //intEncoding = HttpUtility.UrlEncode(intEncoding);
            repoConn.AddMappedType(ageType, intEncoding);

            List<DataType> typeMappings = repoConn.ListTypeMapping();
            foreach (DataType dt in typeMappings)
            {
                Console.WriteLine("{0}:{1}:{2}", dt.Kind, dt.Part, dt.Encoding);
            }
        }

        [Test]
        public void TestClearTypeMapping()
        {
            //Clear all non-automatic type

            //Console.WriteLine("In clear");
            repoConn.ClearTypeMapping();

            string exns = "http://www.example.com/";
            string myType = string.Format("<{0}{1}>", exns, "myInteger");
            string intEncoding = "<http://www.w3.org/2001/XMLSchema#int>";
            repoConn.AddMappedType(myType, intEncoding);

            int preSize = repoConn.ListTypeMapping().Count();
            repoConn.ClearTypeMapping();
            Assert.Greater(preSize, repoConn.ListTypeMapping().Count());

            //Clear all type
            //repoConn.ClearTypeMapping(true);
            //Assert.AreEqual(repoConn.ListTypeMapping().Count(), 0);
        }

        [Test]
        public void TestListMappedPredicates()
        {
            string[] mappedPreds = repoConn.ListMappedPredicates();
            foreach (string s in mappedPreds) Console.WriteLine(s);
        }

        [Test]
        public void TestMappedPredicates()
        {
            string exns = "http://www.example.com/";
            string agePred = string.Format("<{0}{1}>", exns, "age");
            string intEncoding = "<http://www.w3.org/2001/XMLSchema#int>";

            int preSize = repoConn.ListMappedPredicates().Count();

            repoConn.AddMappedPredicate(agePred, intEncoding);
            Assert.AreEqual(preSize, repoConn.ListMappedPredicates().Count() - 1);

            //string[] mappedpreds = repoConn.ListMappedPredicates();
            //foreach (string s in mappedpreds)
            //    Console.WriteLine(s);

            repoConn.DeleteMappedPredicate(agePred);
            Assert.AreEqual(preSize, repoConn.ListMappedPredicates().Count());
        }

    }
}
