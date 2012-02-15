using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Allegro_Graph_CSharp_Client.AGClient.Mini;

namespace Allegro_Graph_CSharp_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AGServerInfo info = new AGServerInfo("http://172.16.2.21:10035", "chainyi", "chainyi123");
            AGClient.Mini.AGClient client = new AGClient.Mini.AGClient(info);
            Console.WriteLine("VERSION = " + client.GetVersion());
            foreach (string catalog in client.ListCatalogs())
            {
                Console.WriteLine(catalog);
            }
            Console.ReadLine();
        }
    }
}
