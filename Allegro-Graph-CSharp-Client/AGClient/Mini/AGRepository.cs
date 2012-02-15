using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public class AGRepository : IAGUrl
    {
        private string RepoUrl;
        private AGServerInfo Server;
        private string Name;

        public AGRepository(AGServerInfo Server, string Name)
        {
            RepoUrl = Server.Url + "/repositories/" + Name;
            this.Name = Name;
            this.Server = Server;
        }

        public string Url { get { return RepoUrl; } }
        public string Username { get { return Server.Username; } }
        public string Password { get { return Server.Password; } }
    }
}
