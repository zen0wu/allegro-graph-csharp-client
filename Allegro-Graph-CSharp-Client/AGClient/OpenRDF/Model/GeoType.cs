using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class GeoType
    {
        string Cartesian = "CARTESIAN";
        string Spherical = "SPHERICAL";
        string system;
        RepositoryConnection connection = null;
        string scale;
        string unit;
        int xMin;
        int xMax;
        int yMin;
        int yMax;
        int latMin;
        int latMax;
        int longMin;
        int longMax;
        string miniGeoType = null;
    }
}
