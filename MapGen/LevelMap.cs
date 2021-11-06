namespace net6test.MapGenerator
{
    public class LevelMap : Grid<LevelElement>
    {
        public LevelMap(int w, int h) : 
            base(w, h, LevelElement.Wall, LevelElement.Wall)
        {
        }
    }


}