using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGServerInfo
    {
        public AGServerInfo(string Url, int Port, string Username, string Password)
        {
            this.Url = Url;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
        }

        public string Url { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
