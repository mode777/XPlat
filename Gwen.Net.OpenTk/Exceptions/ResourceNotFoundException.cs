using System;

namespace Gwen.Net.OpenTk.Exceptions
{
    public class ResourceLoaderNotFoundException : Exception
    {
        public string ResourceName { get; }

        public ResourceLoaderNotFoundException(string resourceName)
            : base(string.Format(StringResources.ResourceLoaderNotFoundFormat, resourceName))
        {
            ResourceName = resourceName;
        }
    }
}