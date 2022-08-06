namespace XPlat.Engine
{

    public abstract class FileResource : Resource
    {
        private SimpleFileWatcher watcher;
        public string Filename { get; set; }
        public bool FileChanged { get; private set; } = true;

        public FileResource() : base()
        {
        }

        public void Load(){
            FileChanged = false;
            Value = LoadFile();
        }

        protected abstract object LoadFile();

        public void Watch(){
            if(Filename != null){
                watcher = new SimpleFileWatcher(Filename);
                watcher.FileChanged += (s,a) => FileChanged = true;
                watcher.Watch();
            }
        }

        public void StopWatching(){
            if(watcher != null){
                watcher.StopWatching();
            }
        }
    }
}