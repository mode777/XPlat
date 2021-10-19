using System.Globalization;
using System.Text.RegularExpressions;

namespace net6test.UI
{
    public readonly struct Quantity {
        public static implicit operator Quantity(string str){
            var split = Regex.Split(str, "([0-9\\.]+)([vhwpx%]*)");
            float val = float.Parse(split[1], NumberStyles.Float, CultureInfo.InvariantCulture);
            switch(split[2]){
                case "px":
                case "":
                    return new Quantity(val, Unit.Pixel);
                case "%":
                    return new Quantity(val, Unit.Percent);
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
    }
}
