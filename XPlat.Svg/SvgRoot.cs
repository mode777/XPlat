using System.Xml;
using System.Xml.Serialization;
using XPlat.Core;
using XPlat.NanoVg;

[XmlRoot("svg", Namespace = "http://www.w3.org/2000/svg")]
public class SvgRoot
{
    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlElement("g")]
    public List<SvgGraphics> Graphics { get; set; }

    public void Draw(NVGcontext vg){
        vg.Translate(500,300);
        vg.Scale(MathF.Sin(Time.RunningTime) + 3, MathF.Sin(Time.RunningTime) + 3);
        //vg.Rotate(Time.RunningTime / 10);
        foreach (var g in Graphics)
        {   
            g.Draw(vg);
        }
    }
}