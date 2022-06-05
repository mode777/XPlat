namespace AzgaarMapReader
{



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Biomes
    {
        public List<int> i { get; set; }
        public List<string> name { get; set; }
        public List<string> color { get; set; }
        public List<BiomesMartix> biomesMartix { get; set; }
        public List<int> habitability { get; set; }
        public List<int> iconsDensity { get; set; }
        public List<List<string>> icons { get; set; }
        public List<int> cost { get; set; }
    }

    public class BiomesMartix
    {
        public int _0 { get; set; }
        public int _1 { get; set; }
        public int _2 { get; set; }
        public int _3 { get; set; }
        public int _4 { get; set; }
        public int _5 { get; set; }
        public int _6 { get; set; }
        public int _7 { get; set; }
        public int _8 { get; set; }
        public int _9 { get; set; }
        public int _10 { get; set; }
        public int _11 { get; set; }
        public int _12 { get; set; }
        public int _13 { get; set; }
        public int _14 { get; set; }
        public int _15 { get; set; }
        public int _16 { get; set; }
        public int _17 { get; set; }
        public int _18 { get; set; }
        public int _19 { get; set; }
        public int _20 { get; set; }
        public int _21 { get; set; }
        public int _22 { get; set; }
        public int _23 { get; set; }
        public int _24 { get; set; }
        public int _25 { get; set; }
    }

    public class Burg
    {
        public int? cell { get; set; }
        public double? x { get; set; }
        public double? y { get; set; }
        public int? state { get; set; }
        public int? i { get; set; }
        public int? culture { get; set; }
        public string name { get; set; }
        public int? feature { get; set; }
        public int? capital { get; set; }
        public int? port { get; set; }
        public double? population { get; set; }
        public string type { get; set; }
        public Coa coa { get; set; }
        public int? citadel { get; set; }
        public int? plaza { get; set; }
        public int? walls { get; set; }
        public int? shanty { get; set; }
        public int? temple { get; set; }
    }

    public class Campaign
    {
        public string name { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }

    public class Cells {
        public List<Cell> cells { get; set; }
        public List<object> features { get; set; }
        public List<Culture> cultures { get; set; }
        public List<Burg> burgs { get; set; }
        public List<State> states { get; set; }
        public List<object> provinces { get; set; }
        public List<Religion> religions { get; set; }
        public List<River> rivers { get; set; }
        public List<Marker> markers { get; set; }
    }

    public class Cell
    {
        public int i { get; set; }
        public List<int> v { get; set; }
        public List<int> c { get; set; }
        public List<double> p { get; set; }
        public int g { get; set; }
        public int h { get; set; }
        public int area { get; set; }
        public int f { get; set; }
        public int t { get; set; }
        public int haven { get; set; }
        public int harbor { get; set; }
        public int fl { get; set; }
        public int r { get; set; }
        public int conf { get; set; }
        public int biome { get; set; }
        public int s { get; set; }
        public double pop { get; set; }
        public int culture { get; set; }
        public int burg { get; set; }
        public int road { get; set; }
        public int crossroad { get; set; }
        public int state { get; set; }
        public int religion { get; set; }
        public int province { get; set; }

    }

    public class Charge
    {
        public string charge { get; set; }
        public string t { get; set; }
        public string p { get; set; }
        public double size { get; set; }
        public string divided { get; set; }
    }

    public class Coa
    {
        public string t1 { get; set; }
        public List<Charge> charges { get; set; }
        public string shield { get; set; }
        public List<Ordinary> ordinaries { get; set; }
        public Division division { get; set; }
    }

    public class Coords
    {
        public double latT { get; set; }
        public double latN { get; set; }
        public double latS { get; set; }
        public int lonT { get; set; }
        public int lonW { get; set; }
        public int lonE { get; set; }
    }

    public class Culture
    {
        public string name { get; set; }
        public int i { get; set; }
        public int @base { get; set; }
        public int? origin { get; set; }
        public string shield { get; set; }
        public int? center { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public double? expansionism { get; set; }
        public string code { get; set; }
    }

    public class Division
    {
        public string division { get; set; }
        public string t { get; set; }
        public string line { get; set; }
    }

    public class Info
    {
        public string version { get; set; }
        public string description { get; set; }
        public DateTime exportedAt { get; set; }
        public string mapName { get; set; }
        public string seed { get; set; }
        public long mapId { get; set; }
    }

    public class Marker
    {
        public string icon { get; set; }
        public string type { get; set; }
        public int dx { get; set; }
        public int px { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int cell { get; set; }
        public int i { get; set; }
        public int? dy { get; set; }
    }

    public class Military
    {
        public string icon { get; set; }
        public string name { get; set; }
        public double rural { get; set; }
        public double urban { get; set; }
        public int crew { get; set; }
        public int power { get; set; }
        public string type { get; set; }
        public int separate { get; set; }
        public int i { get; set; }
        public int a { get; set; }
        public int cell { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double bx { get; set; }
        public double by { get; set; }
        public U u { get; set; }
        public int n { get; set; }
        public int state { get; set; }
    }

    public class NameBas
    {
        public string name { get; set; }
        public int i { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public string d { get; set; }
        public double m { get; set; }
        public string b { get; set; }
    }

    public class Note
    {
        public string id { get; set; }
        public string name { get; set; }
        public string legend { get; set; }
    }

    public class Options
    {
        public bool pinNotes { get; set; }
        public bool showMFCGMap { get; set; }
        public List<int> winds { get; set; }
        public string stateLabelsMode { get; set; }
        public int year { get; set; }
        public string era { get; set; }
        public string eraShort { get; set; }
        public List<Military> military { get; set; }
    }

    public class Ordinary
    {
        public string ordinary { get; set; }
        public string t { get; set; }
        public string line { get; set; }
    }

    public class Religion
    {
        public int i { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public int? culture { get; set; }
        public string type { get; set; }
        public string form { get; set; }
        public string deity { get; set; }
        public int? center { get; set; }
        public int? origin { get; set; }
        public string code { get; set; }
        public string expansion { get; set; }
        public int? expansionism { get; set; }
    }

    public class River
    {
        public int i { get; set; }
        public int source { get; set; }
        public int mouth { get; set; }
        public int discharge { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double widthFactor { get; set; }
        public int sourceWidth { get; set; }
        public int parent { get; set; }
        public List<int> cells { get; set; }
        public int basin { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public Info info { get; set; }
        public Settings settings { get; set; }
        public Coords coords { get; set; }
        public Cells cells { get; set; }
        public Biomes biomes { get; set; }
        public List<Note> notes { get; set; }
        public List<NameBas> nameBases { get; set; }
    }

    public class Settings
    {
        public string distanceUnit { get; set; }
        public string distanceScale { get; set; }
        public string areaUnit { get; set; }
        public string heightUnit { get; set; }
        public string heightExponent { get; set; }
        public string temperatureScale { get; set; }
        public string barSize { get; set; }
        public string barLabel { get; set; }
        public string barBackOpacity { get; set; }
        public string barBackColor { get; set; }
        public string barPosX { get; set; }
        public string barPosY { get; set; }
        public int populationRate { get; set; }
        public int urbanization { get; set; }
        public string mapSize { get; set; }
        public string latitudeO { get; set; }
        public string temperatureEquator { get; set; }
        public string temperaturePole { get; set; }
        public string prec { get; set; }
        public Options options { get; set; }
        public string mapName { get; set; }
        public bool hideLabels { get; set; }
        public string stylePreset { get; set; }
        public bool rescaleLabels { get; set; }
        public int urbanDensity { get; set; }
    }

    public class State
    {
        public int i { get; set; }
        public string name { get; set; }
        public double urban { get; set; }
        public double rural { get; set; }
        public int burgs { get; set; }
        public int area { get; set; }
        public int cells { get; set; }
        public List<int> neighbors { get; set; }
        public List<object> diplomacy { get; set; }
        public List<int> provinces { get; set; }
        public string color { get; set; }
        public double? expansionism { get; set; }
        public int? capital { get; set; }
        public string type { get; set; }
        public int? center { get; set; }
        public int? culture { get; set; }
        public Coa coa { get; set; }
        public List<Campaign> campaigns { get; set; }
        public string form { get; set; }
        public string formName { get; set; }
        public string fullName { get; set; }
        public List<double> pole { get; set; }
        public double? alert { get; set; }
        public List<Military> military { get; set; }
    }

    public class U
    {
        public int archers { get; set; }
        public int cavalry { get; set; }
        public int artillery { get; set; }
        public int infantry { get; set; }
        public int? fleet { get; set; }
    }

}