using System.Xml;
using System.Xml.Serialization;
using XPlat.NanoVg;

public class SvgGraphics
{
    [XmlAttribute("id")]
    public string Id { get; set; }
    [XmlAttribute("fill")]
    public string Fill { get; set; }
    [XmlAttribute("transform")]
    public string Transform { get; set; }
    [XmlAttribute("stroke")]
    public string Stroke { get; set; }
    [XmlAttribute("stroke-width")]
    public string StrokeWidth { get; set; }

    internal void Draw(NVGcontext vg)
    {
        foreach (var g in Graphics)
        {
            g.Draw(vg);
        }
        foreach (var p in Path)
        {
            vg.BeginPath();
            p.Draw(vg);
            if(Fill != null){
                vg.FillColor(Fill);
                vg.Fill();
            }
            if(Stroke != null){
                vg.StrokeWidth(StrokeWidth == null ? 1 : float.Parse(StrokeWidth));
                vg.StrokeColor(Stroke);
                vg.Stroke();
            }
        }
    }

    [XmlElement("g")]
    public List<SvgGraphics> Graphics { get; set; }
    [XmlElement("path")]
    public List<SvgPath> Path { get; set; }
}


