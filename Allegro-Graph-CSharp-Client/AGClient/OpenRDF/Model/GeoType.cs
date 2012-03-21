using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Allegro_Graph_CSharp_Client.AGClient.OpenRDF.RepositoryUtil;

namespace Allegro_Graph_CSharp_Client.AGClient.OpenRDF.Model
{
    public class GeoType
    {
        public static readonly string Cartesian = "CARTESIAN";
        public static readonly string Spherical = "SPHERICAL";
        string system;
        //RepositoryConnection connection = null;
        string scale;
        float unit;
        float xMin;
        float xMax;
        float yMin;
        float yMax;
        float latMin;
        float latMax;
        float longMin;
        float longMax;
        //string miniGeoType = null;
        public GeoType(string sys, string scale, float unit,
                       float xMin = 0, float xMax = 0, float yMin = 0, float yMax = 0,
                       float latMin = 0, float latMax = 0, float longMin = 0, float longMax = 0)
        {
            this.system = sys;
            this.scale = scale;
            this.unit = unit;
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
            this.latMin = latMin;
            this.latMax = latMax;
            this.longMin = longMin;
            this.longMax = longMax;
        }
    }
}
