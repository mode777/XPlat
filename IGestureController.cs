using System.Collections.Generic;
using System.Drawing;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Internals
{
	public interface IGestureController
	{
		IList<GestureElement> GetChildElements(Point point);

		IList<IGestureRecognizer> CompositeGestureRecognizers { get; }
	}
}