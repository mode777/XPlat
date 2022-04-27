using System;
using Gwen.Net.Control;

namespace Gwen.Net.OpenTk
{
    public interface IGwenGui : IDisposable
    {
        ControlBase Root { get; }

        void Load();

        void Resize(Size newSize);

        void Render();
    }
}
