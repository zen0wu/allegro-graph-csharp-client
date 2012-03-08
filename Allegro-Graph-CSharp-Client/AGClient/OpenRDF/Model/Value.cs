using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class URI
    {
        string _uri;
        public string Uri { get { return _uri; } }
        public URI(string uri,string nameSpace=null,string localName=null)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                if (uri[0] == '<' && uri[uri.Length - 1] == '>')
                {
                    _uri = uri.Substring(1,uri.Length-2);
                }
            }
            else if (!string.IsNullOrEmpty(nameSpace) && !string.IsNullOrEmpty(localName))
            {
                _uri = nameSpace + localName;
            }
        }
        public string ToNTriples()
        {
            return string.Format("<{0}>",Uri);
        }
        public override string ToString()
        {
            return _uri;
        }
    }

    public class BNode
    {
        string _id;
        public string ID { get { return _id; } }
        public string ToNTriples()
        {
            return string.Format("_:{0}", ID);
        }
        public BNode(string id)
        {
            this._id = id;
        }
    }

   public class Namespace
   {
       string _prefix;
       string _name;
       public string Prefix{
           get { return _prefix; }
           set { this._prefix = value; }
       }
       public string NameSpace
       {
           get { return _name; }
           set { this._name = value; }
       }

       public Namespace(string p, string n)
       {
           this._name = n;
           this._prefix = p;
       }

       public override string ToString()
       {
           return string.Format("{0} :: {1}",_prefix,_name);
       }
   }
}
