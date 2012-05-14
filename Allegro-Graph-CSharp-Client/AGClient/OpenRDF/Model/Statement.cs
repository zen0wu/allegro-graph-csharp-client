using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class Statement
    {
        private string _subject;
        private string _predicate;
        private string _object;
        private string _context;
        private string _id;

        
        public Statement(string subj, string pred, string obj, string context = null,string id=null)
        {
            this._subject = subj;
            this._predicate = pred;
            this._object = obj;
            this._context = context;
            this._id = id;
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public string Object
        {
            get { return _object; }
            set { _object = value; }
        }
        public string Predicate
        {
            get { return _predicate; }
            set { _predicate = value; }
        }
        public string Context
        {
            get { return _context; }
            set { _context = value; }
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}",Id,Subject,Predicate,Object);
        }
    }
}
