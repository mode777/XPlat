using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using XPlat.NanoVg;

public class SvgPath
{
    private string path;

    [XmlAttribute("id")]
    public string Id { get; set; }

    [XmlAttribute("d")]
    public string Path
    {
        get => path;
        set
        {
            path = value;
            Parse();
        }
    }

    [XmlIgnore]
    private List<ISvgCommand> commands = new List<ISvgCommand>();

    public void Draw(NVGcontext vg)
    {
        foreach (var cmd in commands)
        {
            cmd.Execute(vg);
        }
    }

    private void Parse()
    {
        var numBuffer = new StringBuilder(8);
        var currentPoint = default(Vector2);
        var currentCommand = default(char);
        var prevCommand = default(char);
        var coordinates = new Queue<float>();
        Vector2 control1 = default(Vector2);
        Vector2 control2 = default(Vector2);

        void flushNum()
        {
            if (numBuffer.Length == 0) return;
            coordinates.Enqueue(float.Parse(numBuffer.ToString()));
            numBuffer.Clear();
        }

        Vector2 projectPoint(Vector2 control, Vector2 anchor){
            return anchor + (anchor - control);
        }

        void flushCommand()
        {
            switch (currentCommand)
            {
                case 'M':
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new MoveTo(currentPoint));
                    currentCommand = 'L';
                    break;
                case 'm':
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new MoveTo(currentPoint));
                    currentCommand = 'l';
                    break;
                case 'L':
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'l':
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'H':
                    currentPoint.X = coordinates.Dequeue();
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'h':
                    currentPoint.X += coordinates.Dequeue();
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'V':
                    currentPoint.Y = coordinates.Dequeue();
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'v':
                    currentPoint.Y += coordinates.Dequeue();
                    commands.Add(new LineTo(currentPoint));
                    break;
                case 'C':
                    control1 = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    control2 = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new CubicTo(control1, control2, currentPoint));
                    break;
                case 'c':
                    control1 = currentPoint + new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    control2 = currentPoint + new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new CubicTo(control1, control2, currentPoint));
                    break;
                case 'S':
                    control1 = prevCommand == 's' || prevCommand == 'S' ? projectPoint(control2, currentPoint) : currentPoint;
                    control2 = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new CubicTo(control1, control2, currentPoint));
                    break;
                case 's':
                    control1 = prevCommand == 's' || prevCommand == 'S' ? projectPoint(control2, currentPoint) : currentPoint;
                    control2 = currentPoint + new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new CubicTo(control1, control2, currentPoint));
                    break;
                case 'Q':
                    control1 = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new QuadTo(control1, currentPoint));
                    break;
                case 'q':
                    control1 = currentPoint + new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new QuadTo(control1, currentPoint));
                    break;
                case 'T':
                    control1 = prevCommand == 't' || prevCommand == 'T' ? projectPoint(control1, currentPoint) : currentPoint;
                    currentPoint = new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new QuadTo(control1, currentPoint));
                    break;
                case 't':
                    control1 = prevCommand == 't' || prevCommand == 'T' ? projectPoint(control1, currentPoint) : currentPoint;
                    currentPoint += new Vector2(coordinates.Dequeue(), coordinates.Dequeue());
                    commands.Add(new QuadTo(control1, currentPoint));
                    break;
                case 'A':
                case 'a':
                    throw new NotImplementedException();
                case 'Z':
                case 'z':
                    commands.Add(new ClosePath());
                    break;
                default:
                    return;
            }
            if (coordinates.Count > 0)
            {
                flushCommand();
            }
        }

        foreach (var c in path)
        {
            switch (c)
            {
                case 'M':
                case 'm':
                case 'L':
                case 'l':
                case 'H':
                case 'h':
                case 'V':
                case 'v':
                case 'C':
                case 'c':
                case 'S':
                case 's':
                case 'Q':
                case 'q':
                case 'T':
                case 't':
                case 'A':
                case 'a':
                case 'Z':
                case 'z':
                    flushNum();
                    flushCommand();
                    prevCommand = currentCommand;
                    currentCommand = c;
                    break;
                case ',':
                    flushNum();
                    break;
                case '-':
                    flushNum();
                    numBuffer.Append(c);
                    break;
                default:
                    numBuffer.Append(c);
                    break;
            }
        }
        flushNum();
        flushCommand();
    }
}




// class YourClass : IXmlSerializable
// {
//     public int? Age
//     {
//         get { return this.age; }
//         set { this.age = value; }
//     }

//     //OTHER CLASS STUFF//

//     #region IXmlSerializable members
//     public void WriteXml (XmlWriter writer)
//     {
//         if( Age != null )
//         {
//             writer.WriteValue( Age )
//         }
//     }

//     public void ReadXml (XmlReader reader)
//     {
//         Age = reader.ReadValue();
//     }

//     public XmlSchema GetSchema()
//     {
//         return(null);
//     }
//     #endregion
// }