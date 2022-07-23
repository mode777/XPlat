namespace XPlat.Engine
{
    public class Resource : IResource
    {
        public Resource()
        {
        }

        private object _value;
        public object Value { get => _value; protected set { _value = value; Changed?.Invoke(this, EventArgs.Empty); } }

        public string Id { get; set; }

        public event EventHandler Changed;

        public T GetValue<T>()
        {
            return (T)_value;
        }
    }
}