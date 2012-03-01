using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    class Statement
    {
        private string _subject;
        private string _predicate;
        private string _object;
        private string _context;
        public Statement(string subj, string pred, string obj, string context = null)
        {
            this._subject = subj;
            this._predicate = pred;
            this._object = obj;
            this._context = context;
        }
        public string getContext()
        {
            return this._context;
        }
        string getObject()
        {
            return this._object;
        }
        string getPredicate()
        {
            return this._predicate;
        }
        string getSubject()
        {
            return this._subject;
        }
        //void setQuad(object string_tuple)
        //{
        //}

    }
}
