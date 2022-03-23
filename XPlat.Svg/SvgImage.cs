using System.Numerics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XPlat.Svg
{
    public class SvgImage
    {
        public static SvgImage Load(string path)
        {
            var img = new SvgImage();
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SvgRoot));
                img.Document = (SvgRoot)serializer.Deserialize(fileStream);
            }
            return img;
        }

        public SvgRoot Document { get; set; }
    }
}
