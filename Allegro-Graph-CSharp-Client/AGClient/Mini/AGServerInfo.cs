using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
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
        /// 用户名
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
    }
}
