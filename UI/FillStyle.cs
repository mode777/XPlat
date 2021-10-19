using NanoVGDotNet;

namespace net6test.UI
{
    public abstract class FillStyle {
        public static implicit operator FillStyle(string str){
            if(str[0] == '#'){
                return new ColorFillStyle(str);
            } else {
                throw new NotImplementedException();
            }
        }
        public abstract void Apply(NVGcontext vg);
    }
}
