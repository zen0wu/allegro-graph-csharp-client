using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    class XMLSchema
    {
        public static readonly string NS = "http://www.w3.org/2001/XMLSchema#";
        public static URI DURATION = new URI(null, NS, "duration");
        public static URI DATETIME = new URI(null, NS, "dateTime");
        public static URI TIME = new URI(null, NS, "time");
        public static URI DATE = new URI(null, NS, "date");
        public static URI GYEARMONTH = new URI(null, NS, "gYearMonth");
        public static URI GYEAR = new URI(null, NS, "gYear");
        public static URI GMONTHDAY = new URI(null, NS, "gMonthDay");
        public static URI GDAY = new URI(null, NS, "gDay");
        public static URI GMONTH = new URI(null, NS, "gMonth");
        public static URI STRING = new URI(null, NS, "string");
        public static URI BOOLEAN = new URI(null, NS, "boolean");
        public static URI BASE64BINARY = new URI(null, NS, "base64Binary");
        public static URI HEXBINARY = new URI(null, NS, "hexBinary");
        public static URI FLOAT = new URI(null, NS, "float");
        public static URI DECIMAL = new URI(null, NS, "decimal");
        public static URI DOUBLE = new URI(null, NS, "double");
        public static URI ANYURI = new URI(null, NS, "anyURI");
        public static URI QNAME = new URI(null, NS, "QName");
        public static URI NOTATION = new URI(null, NS, "NOTATION");
        public static URI NORMALIZEDSTRING = new URI(null, NS, "normalizedString");
        public static URI TOKEN = new URI(null, NS, "token");
        public static URI LANGUAGE = new URI(null, NS, "language");
        public static URI NMTOKEN = new URI(null, NS, "NMTOKEN");
        public static URI NMTOKENS = new URI(null, NS, "NMTOKENS");
        public static URI NAME = new URI(null, NS, "Name");
        public static URI NCNAME = new URI(null, NS, "NCName");
        public static URI ID = new URI(null, NS, "ID");
        public static URI IDREF = new URI(null, NS, "IDREF");
        public static URI IDREFS = new URI(null, NS, "IDREFS");
        public static URI ENTITY = new URI(null, NS, "ENTITY");
        public static URI ENTITIES = new URI(null, NS, "ENTITIES");
        public static URI INTEGER = new URI(null, NS, "integer");
        public static URI LONG = new URI(null, NS, "long");
        public static URI INT = new URI(null, NS, "int");
        public static URI SHORT = new URI(null, NS, "short");
        public static URI NUMBER = new URI(null, NS, "shnumberort");
        public static URI BYTE = new URI(null, NS, "byte");
        public static URI NON_POSITIVE_INTEGER = new URI(null, NS, "nonPositiveInteger");
        public static URI NEGATIVE_INTEGER = new URI(null, NS, "negativeInteger");
        public static URI NON_NEGATIVE_INTEGER = new URI(null, NS, "nonNegativeInteger");
        public static URI POSITIVE_INTEGER = new URI(null, NS, "positiveInteger");
        public static URI UNSIGNED_LONG = new URI(null, NS, "unsignedLong");
        public static URI UNSIGNED_INT = new URI(null, NS, "unsignedInt");
        public static URI UNSIGNED_SHORT = new URI(null, NS, "unsignedShort");
        public static URI UNSIGNED_BYTE = new URI(null, NS, "unsignedByte");
    }
}
