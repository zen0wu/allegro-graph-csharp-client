using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Rio
{
    public class RDFFormat
    {
        string name;
        string[] mime_types;
        string char_set;
        string[] file_extensions;
        bool supports_namespaces;
        bool supportsContexts;

        public string Name { get { return this.name; } }
        public string[] Mime_types { get { return this.Mime_types; } }
        public string Char_set { get { return this.char_set; } }
        public string[] File_extensions { get { return this.file_extensions; } }
        public bool Supports_namespaces { get { return this.supports_namespaces; } }
        public bool SupportsContexts { get { return this.supportsContexts; } }

        public RDFFormat(string formatName, string[] mimeTypes, string charset = "UTF-8", string[] fileExtensions = null,
                         bool supportsNamespaces = false, bool supportsContexts = false)
        {
            this.name = formatName;
            this.mime_types = mimeTypes;
            this.char_set = charset;
            this.file_extensions = fileExtensions;
            this.supports_namespaces = supportsNamespaces;
            this.supportsContexts = supportsContexts;
        }
        //The RDF/XML file format.
        public static readonly RDFFormat RDFXML = new RDFFormat("RDF/XML",
                                                        new string[] { "application/rdf+xml", "application/xml" },
                                                        "UTF-ASCII",
                                                        new string[] { "rdf", "rdfs", "owl", "xml" },
                                                        true,
                                                        false);
        //The N-Triples file format.
        public static readonly RDFFormat NTRIPLES = new RDFFormat("NTRIPLES",
                                                          new string[] { "text/plain" },
                                                          "UTF-8",
                                                          new string[] { "nt" },
                                                          false,
                                                          false);
    }
}
