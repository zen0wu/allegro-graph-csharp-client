using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.Util
{
    public class AGRequestException : Exception
    {
        public AGRequestException() : base() { }

        public AGRequestException(string Message) : base(Message) { }

        public AGRequestException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
