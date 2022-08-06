namespace XPlat.Engine
{
    public class SimpleFileWatcher
    {
        public SimpleFileWatcher(string filename)
        {
            Filename = filename;
        }

        private FileSystemWatcher watcher;
        public event EventHandler FileChanged;

        public string Filename { get; }

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
                FileChanged?.Invoke(this, EventArgs.Empty);
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