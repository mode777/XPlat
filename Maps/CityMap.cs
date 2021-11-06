using NanoVGDotNet;
using net6test.UI;

namespace net6test.Maps
{
    public class CityMap
    {
        private readonly FeatureCollection coll;

        public CityMap(FeatureCollection coll)
        {
            this.coll = coll;
        }
        public void Draw(NVGcontext vg)
        {
            foreach (var item in coll.Features)
            {
                switch (item.Id)
                {
                    case "earth":
                        DrawEarth(vg, item as Polygon);
                        break;
                    case "buildings":
                        DrawBuildings(vg, item as MultiPolygon);
                        break;
                    case "prisms":
                        DrawPrisms(vg, item as MultiPolygon);
                        break;
                    case "squares":
                        DrawSquares(vg, item as MultiPolygon);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DrawSquares(NVGcontext vg, MultiPolygon? multiPolygon)
        {
            foreach (var polygon in multiPolygon.Coordinates)
            {
                foreach (var cluster in polygon)
                {
                    vg.BeginPath();
                    bool first = true;
                    foreach (var item in cluster)
                    {
                        if (first)
                        {
                            vg.MoveTo(item[0], item[1]);
                            first = false;
                        }
                        else
                        {
                            vg.LineTo(item[0], item[1]);
                        }
                    }
                    vg.FillColor("#00ff00");
                    vg.Fill();
                }
            }
        }

        private void DrawPrisms(NVGcontext vg, MultiPolygon? multiPolygon)
        {
            foreach (var polygon in multiPolygon.Coordinates)
            {
                foreach (var cluster in polygon)
                {
                    vg.BeginPath();
                    bool first = true;
                    foreach (var item in cluster)
                    {
                        if (first)
                        {
                            vg.MoveTo(item[0], item[1]);
                            first = false;
                        }
                        else
                        {
                            vg.LineTo(item[0], item[1]);
                        }
                    }
                    vg.FillColor("#ff0000");
                    vg.Fill();
                }
            }
        }

        private void DrawBuildings(NVGcontext vg, MultiPolygon? multiPolygon)
        {
            vg.Translate(5, 5);

            foreach (var polygon in multiPolygon.Coordinates)
            {
                foreach (var cluster in polygon)
                {
                    vg.BeginPath();
                    bool first = true;
                    foreach (var item in cluster)
                    {
                        if (first)
                        {
                            vg.MoveTo(item[0], item[1]);
                            first = false;
                        }
                        else
                        {
                            vg.LineTo(item[0], item[1]);
                        }
                    }
                    vg.FillColor("#00000044");

                    vg.Fill();
                }
            }
            vg.Translate(-5, -5);

            foreach (var polygon in multiPolygon.Coordinates)
            {
                

                foreach (var cluster in polygon)
                {
                    vg.BeginPath();
                    bool first = true;
                    foreach (var item in cluster)
                    {
                        if (first)
                        {
                            vg.MoveTo(item[0], item[1]);
                            first = false;
                        }
                        else
                        {
                            vg.LineTo(item[0], item[1]);
                        }
                    }
                    vg.FillColor("#ffffff");
                    vg.StrokeColor("#ffffff");
                    
                    vg.Fill();
                    vg.Stroke();
                }

            }
        }

        private void DrawEarth(NVGcontext vg, Polygon? earth)
        {
            foreach (var cluster in earth.Coordinates)
            {
                vg.BeginPath();
                bool first = true;
                foreach (var item in cluster)
                {
                    if (first)
                    {
                        vg.MoveTo(item[0], item[1]);
                        first = false;
                    } else
                    {
                        vg.LineTo(item[0], item[1]);
                    }
                }
                vg.FillColor("#ffffff44");
                vg.Fill();
            }
        }
    }
}




