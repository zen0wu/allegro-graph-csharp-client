using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client_NUnitTest.MiniTest;

namespace Allegro_Graph_CSharp_Client_NUnitTest
{
    class RunTest
    {
        public static void Main()
        {
            AGCatalogTest test1 = new AGCatalogTest();
            test1.Init();
            test1.CreateRepositoryTest();
        }
    }
}
