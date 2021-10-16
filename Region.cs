using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public struct Region
	{
		// While Regions are currently rectangular, they could in the future be transformed into any shape.
		// As such the internals of how it keeps shapes is hidden, so that future internal changes can occur to support shapes
		// such as circles if required, without affecting anything else.

		IReadOnlyList<RectangleF> Regions { get; }
		readonly Thickness? _inflation;

		Region(IList<RectangleF> positions) : this()
		{
			Regions = new ReadOnlyCollection<RectangleF>(positions);
		}

		Region(IList<RectangleF> positions, Thickness inflation) : this(positions)
		{
			_inflation = inflation;
		}

		public static Region FromLines(double[] lineHeights, double maxWidth, double startX, double endX, double startY)
		{
			var positions = new List<RectangleF>();
			var endLine = lineHeights.Length - 1;
			var lineHeightTotal = startY;

			for (var i = 0; i <= endLine; i++)
				if (endLine != 0) // MultiLine
				{
					if (i == 0) // First Line
						positions.Add(new RectangleF((float)startX, (float)lineHeightTotal, (float)maxWidth - (float)startX, (float)lineHeights[i]));

					else if (i != endLine) // Middle Line
						positions.Add(new RectangleF(0, (float)lineHeightTotal, (float)maxWidth, (float)lineHeights[i]));

					else // End Line
						positions.Add(new RectangleF(0, (float)lineHeightTotal, (float)endX, (float)lineHeights[i]));

					lineHeightTotal += lineHeights[i];
				}
				else // SingleLine
					positions.Add(new RectangleF((float)startX, (float)lineHeightTotal, (float)endX - (float)startX, (float)lineHeights[i]));

			return new Region(positions);
		}

		public bool Contains(Point pt)
		{
			return Contains(pt.X, pt.Y);
		}

		public bool Contains(double x, double y)
		{
			if (Regions == null)
				return false;

			for (int i = 0; i < Regions.Count; i++)
				if (Regions[i].Contains((float)x, (float)y))
					return true;

			return false;
		}

		public Region Deflate()
		{
			if (_inflation == null)
				return this;

			return Inflate(_inflation.Value.Left * -1, _inflation.Value.Top * -1, _inflation.Value.Right * -1, _inflation.Value.Bottom * -1);
		}

		public Region Inflate(double size)
		{
			return Inflate(size, size, size, size);
		}

		public Region Inflate(double left, double top, double right, double bottom)
		{
			if (Regions == null)
				return this;

			RectangleF[] RectangleFs = new RectangleF[Regions.Count];
			for (int i = 0; i < Regions.Count; i++)
			{
				var region = Regions[i];

				if (i == 0) // this is the first line
					region.Y -= (float)top;

				region.X -= (float)left;
				region.Width += (float)right + (float)left;

				if (i == Regions.Count - 1) // This is the last line
					region.Height += (float)bottom + (float)top;

				RectangleFs[i] = region;
			}

			var inflation = new Thickness(_inflation == null ? left : left + _inflation.Value.Left,
									   _inflation == null ? top : top + _inflation.Value.Top,
									   _inflation == null ? right : right + _inflation.Value.Right,
									   _inflation == null ? bottom : bottom + _inflation.Value.Bottom);

			return new Region(RectangleFs, inflation);
		}
	}
}