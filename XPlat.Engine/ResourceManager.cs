using System.Collections;

namespace XPlat.Engine
{
    public class ResourceManager : IEnumerable<IResource>
    {
        private readonly Dictionary<string, IResource> _resources = new Dictionary<string, IResource>();

        public void Store(IResource res)
        {
            _resources[res.Id] = res;
        }

        public IResource Load(string id)
        {
            return _resources[id];
        }

        public T? LoadValue<T>(string id) where T : class
        {
            return _resources[id]?.GetValue<T>();
        }

        public IEnumerator<IResource> GetEnumerator()
        {
            return _resources.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.Values.GetEnumerator();
        }
    }
}