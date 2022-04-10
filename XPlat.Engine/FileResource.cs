namespace XPlat.Engine
{
    public abstract class FileResource : Resource
    {
        private FileSystemWatcher watcher;
        public string Filename { get; protected set; }
        public bool FileChanged { get; private set; } = true;

        public FileResource(string id, string path) : base(id)
        {
            this.Filename = path;
        }

        public void Load(){
            Value = LoadFile();
            FileChanged = false;
        }

        protected abstract object LoadFile();

        public void Watch(){
            var dir = Path.GetDirectoryName(Filename);
            var file = Path.GetFileName(Filename);
            watcher = new FileSystemWatcher(dir);
            if(!string.IsNullOrEmpty(file)) watcher.Filter = file;
            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.LastAccess
                | NotifyFilters.Attributes
                | NotifyFilters.Size
                | NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.Security;
            watcher.Changed += (s, args) =>
            {
                FileChanged = true;
            };
            watcher.EnableRaisingEvents = true;
        }

        public void StopWatching(){
            if(watcher != null){
                watcher.EnableRaisingEvents = false;
                watcher = null;
            }
        }
    }
}