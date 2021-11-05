using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace net6test.Maps
{

    public static class GeoJson
    {
        public static GeoJsonElement? Load(string filename)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new UnionConverterFactory() }
            };
            return JsonSerializer.Deserialize<GeoJsonElement>(File.ReadAllText(filename), options);
        }
    }

    [UnionTag(nameof(Type))]
    [UnionCase(typeof(FeatureCollection), "FeatureCollection")]
    [UnionCase(typeof(Feature), "Feature")]
    [UnionCase(typeof(Polygon), "Polygon")]
    [UnionCase(typeof(GeometryCollection), "GeometryCollection")]
    [UnionCase(typeof(MultiPolygon), "MultiPolygon")]
    [UnionCase(typeof(LineString), "LineString")]
    public abstract class GeoJsonElement
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public sealed class FeatureCollection : GeoJsonElement
    {
        public GeoJsonElement[] Features { get; set; }
    }

    public sealed class Feature : GeoJsonElement
    {
        public float RoadWidth { get; set; }
        public float TowerRadius { get; set; }
        public float WallThickness { get; set; }
        public string Generator { get; set; }
        public string Version { get; set; }
    }
    public sealed class Polygon : GeoJsonElement
    {
        public float[][][] Coordinates { get; set; }
    }
    public sealed class GeometryCollection : GeoJsonElement
    {
        public GeoJsonElement[] Geometries { get; set; }
    }
    public sealed class MultiPolygon : GeoJsonElement
    {
        public float[][][][] Coordinates { get; set; }
    }
    public sealed class LineString : GeoJsonElement
    {
        public float Width { get; set; }
        public float[][] Coordinates { get; set; }
    }
}




