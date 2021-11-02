using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace net6test.UI
{

    public readonly struct Quantity
    {
        public static implicit operator float(Quantity q) => q.ToPixels();

        public static implicit operator Quantity(float v)
        {
            return new Quantity(v, Unit.Point);
        }

        public static implicit operator Quantity(string str)
        {
            var split = Regex.Split(str, "([\\-0-9\\.]+)([vhwpx%]*)");
            float val = float.Parse(split[1], NumberStyles.Float, CultureInfo.InvariantCulture);
            switch (split[2])
            {
                case "px":
                case "":
                    return new Quantity(val, Unit.Point);
                case "vw":
                    return new Quantity(val, Unit.ViewportWidth);
                case "vh":
                    return new Quantity(val, Unit.ViewportHeight);
                default:
                    throw new Exception("Invalid Unit");
            }
        }

        public readonly float Value;
        public readonly Unit Unit;

        public Quantity(float value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public float ToPixels() 
        {
            var p = IPlatformInfo.Default;
            switch (Unit)
            {
                case Unit.Point:
                    return Value * p.RetinaScale;
                case Unit.ViewportWidth:
                    return (Value / 100f) * p.RendererSize.Width;
                case Unit.ViewportHeight:
                    return (Value / 100f) * p.RendererSize.Height;
                default:
                    throw new Exception("Unknown unit");
            }
}
    }
}
