namespace Microsoft.Maui
{
	public interface IElement
	{
		/// <summary>
		/// Gets or sets the View Handler of the Element.
		/// </summary>
		IElementHandler? Handler { get; set; }

		/// <summary>
		/// Gets the Parent of the Element.
		/// </summary>
		IElement? Parent { get; }
	}
}


namespace Microsoft.Maui.Controls
{
	using System;
	using Microsoft.Maui.Controls.Internals;

	internal interface IElement
	{
		Element Parent { get; set; }

		//Use these 2 instead of an event to avoid cloning way too much multicastdelegates on mono
		void AddResourcesChangedListener(Action<object, ResourcesChangedEventArgs> onchanged);
		void RemoveResourcesChangedListener(Action<object, ResourcesChangedEventArgs> onchanged);
	}
}