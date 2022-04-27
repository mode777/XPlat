using System;

namespace Gwen.Net.RichText
{
    public abstract class Part
    {
        public abstract string[] Split(ref Font font);
    }
}