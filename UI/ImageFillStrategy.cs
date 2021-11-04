using System.Drawing;

namespace net6test.UI
{
    public abstract class ImageFillStrategy {

        public static implicit operator ImageFillStrategy(string type){
            switch (type.ToLower())
            {
                case "stretch":
                    return ImageFillStrategy.Stretch;
                case "fit":
                    return ImageFillStrategy.Fit;
                case "cover":
                    return ImageFillStrategy.Cover;
                default:
                    throw new Exception("Unknown fill strategy");
            }
        }

        public static readonly ImageFillStrategy Stretch = new StretchFillStrategy();
        public static readonly ImageFillStrategy Fit = new FitFillStrategy();
        public static readonly ImageFillStrategy Cover = new CoverFillStrategy();

        public abstract RectangleF Apply(NvgImage img, RectangleF rect);
        
        public class StretchFillStrategy : ImageFillStrategy
        {
            public override RectangleF Apply(NvgImage img, RectangleF rect) => rect;
        }

        public class FitFillStrategy : ImageFillStrategy
        {
            public override RectangleF Apply(NvgImage img, RectangleF rect) {
                var ratioFit = rect.Width / rect.Height;
                var ratioImg = img.Width / (float)img.Height;
                if(ratioFit > ratioImg){
                    var factor = rect.Height / img.Height;
                    var width = img.Width * factor;
                    var diff = (rect.Width - width) / 2f;
                    return new RectangleF(rect.X + diff, rect.Y, width, rect.Height);
                } else {
                    var factor = rect.Width / img.Width;
                    var height = img.Height * factor;
                    var diff = (rect.Height - height) / 2f;
                    return new RectangleF(rect.X, rect.Y + diff, rect.Width, height);

                }
            }
        }

        public class CoverFillStrategy : ImageFillStrategy
        {
            public override RectangleF Apply(NvgImage img, RectangleF rect) {
                var ratioFit = rect.Width / rect.Height;
                var ratioImg = img.Width / (float)img.Height;
                if(ratioFit < ratioImg){
                    var factor = rect.Height / img.Height;
                    var width = img.Width * factor;
                    var diff = (rect.Width - width) / 2f;
                    return new RectangleF(rect.X + diff, rect.Y, width, rect.Height);
                } else {
                    var factor = rect.Width / img.Width;
                    var height = img.Height * factor;
                    var diff = (rect.Height - height) / 2f;
                    return new RectangleF(rect.X, rect.Y + diff, rect.Width, height);

                }
            }
        }

        
    }
}
