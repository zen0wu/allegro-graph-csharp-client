using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    /// <summary>
    /// The server object of Allegro Graph
    /// </summary>
    public class AGServerInfo : IAGUrl
    {
        public AGServerInfo(string BaseUrl, string Username, string Password)
        {
            this.Url = BaseUrl;
            this.Username = Username;
            this.Password = Password;
        }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; private set; }
    }
}
