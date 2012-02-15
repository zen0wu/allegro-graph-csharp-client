using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Mini
{
    public interface IAGUrl
    {
        string Url { get; }

        string Username { get; }

        string Password { get; }
    }
}
