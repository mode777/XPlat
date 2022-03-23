//using net6test.MapGenerator;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.AI.Navigation;
//using System.Linq;

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshCollider))]
//public class MeshGenerator : MonoBehaviour
//{
//    // Texture Positions
//    const int TILES_X = 4;
//    const int TILES_Y = 4;


//    public int Seed = 1986;
//    public int Width = 64;
//    public int Height = 64;
//    public int Threshold = 6;
//    public float Scale = 2;
//    public GameObject Player;
//    public List<GameObject> Enemies;
//    public List<GameObject> Props;
//    public List<GameObject> PowerUps;
//    public GameObject PortalEnd;

//    Mesh mesh;

//    List<Vector3> vertices = new List<Vector3>();
//    List<int> triangles = new List<int>();
//    List<Vector2> uvs = new List<Vector2>();
//    List<GameObject> enemiesSpawned = new List<GameObject>();
//    List<GameObject> propsSpawned = new List<GameObject>();
//    private List<GameObject> spawnedPowerups = new List<GameObject>();
//    private LevelMap map;
//    private ProdGen pg;
//    private MapGen mg;
    
//    private const float ONE_POWERUP_CHANCE = 0.25f;
//    private const float TWO_POWERUPS_CHANCE = 0.1f;

//    // Start is called before the first frame update
//    // void Start()
//    // {
//    //     Generate();
//    // }

//    // void OnValidate()
//    // {        
//    //     Generate();
//    // }

//    [ContextMenu("ProdGen/Generate")]
//    public void Generate()
//    {
//        // Commented out - seed should be an outside parameter
//        //this.Seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
//        this.CleanSpawnedGameObjectList(this.enemiesSpawned);
//        this.CleanSpawnedGameObjectList(this.propsSpawned);
//        this.CleanSpawnedGameObjectList(this.spawnedPowerups);

//        pg = new ProdGen(Seed);
//        mg = new MapGen(pg, Width, Height, Threshold);
//        this.map = mg.Map;

//        mesh = new Mesh();
//        GetComponent<MeshFilter>().mesh = mesh;
//        //GetComponent<MeshRenderer>().material =

//        CreateShape();
//        UpdateMesh();
//        GetComponent<NavMeshSurface>().BuildNavMesh();

//        if (Player != null)
//        {
//            var c = pg.Select(mg.StartRoom.POIs);
//            if (c.IsEmpty) c = mg.StartRoom.Center();
//            Player.transform.position = new Vector3(c.X * Scale , 0, c.Y * Scale );
//        }

//        if(Enemies.Count > 0)
//        {
//            var rooms = mg.ActiveRooms.Count();
//            Debug.Log($"{rooms} Rooms are active");
//            foreach (var r in mg.ActiveRooms)
//            {
//                if (r == mg.StartRoom) continue;

//                var spawn = pg.Select(r.POIs);
//                if (!spawn.IsEmpty)
//                {
//                    SpawnEnemy(spawn.X, spawn.Y);
//                    r.POIs.Remove(spawn);
//                }


//                if(PowerUps.Count > 0)
//                {
//                    SpawnRandomPowerupsInRoom(r);
//                }

//                if(Props.Count > 0 && false)
//                {                    
//                    var amnt = r.POIs.Count / 4;
//                    var copy = r.POIs.ToArray().ToList();
//                    pg.Shuffle(copy);
//                    for (int i = 0; i < amnt; i++)
//                    {
//                        var p = copy[i];
//                        SpawnProp(p.X, p.Y);
//                        r.POIs.Remove(p);
//                    }
//                }
//            }
//        }

//        var endRoomCenter = pg.Select(mg.EndRoom.POIs);
//        if (endRoomCenter.IsEmpty) endRoomCenter = mg.EndRoom.Center();
//        this.PortalEnd.transform.position = new Vector3(endRoomCenter.X * this.Scale + Scale/2.0f, 1f, endRoomCenter.Y * this.Scale + Scale / 2.0f);
//    }

//    private void SpawnEnemy(int x, int y)
//    {
//        var en = pg.Select(Enemies);
//        if(en != null)
//        {
//            var inst = Instantiate(en);
//            inst.transform.position = new Vector3(x * Scale, 1f, y * Scale);
//            enemiesSpawned.Add(inst);
//            Debug.Log($"Spawned {en.name} at {x},{y}");
//        }
//    }

//    private void SpawnProp(int x ,int y)
//    {
//        var en = Props.First();
//        if (en != null)
//        {
//            var inst = Instantiate(en);
//            inst.transform.position = new Vector3(x * Scale, 1f, y * Scale);
//            propsSpawned.Add(inst);
//        }
//    }

//    private void UpdateMesh()
//    {
//        mesh.Clear();
                
//        mesh.vertices = vertices.ToArray();
//        mesh.uv = uvs.ToArray();
//        mesh.triangles = triangles.ToArray();
//        mesh.RecalculateNormals();
//        GetComponent<MeshCollider>().sharedMesh = mesh;
//    }

//    private void CreateShape()
//    {
//        vertices.Clear();
//        triangles.Clear();
//        uvs.Clear();
//        for (int y = 0; y <= map.H; y++)
//        {
//            for (int x = 0; x <= map.W; x++)
//            {
//                var v = map[x, y];
//                var n = map.Neighbours(x, y);
//                if (v != LevelElement.Wall)
//                {
//                    if(v != LevelElement.Hole) AddFloor(x, y);
//                    if (n[0] == LevelElement.Wall) AddWallE(x, y);
//                    if (n[1] == LevelElement.Wall) AddWallW(x, y);
//                    if (n[2] == LevelElement.Wall) AddWallN(x, y);
//                    if (n[3] == LevelElement.Wall) AddWallS(x, y);
//                    if(v == LevelElement.Hole)
//                    {
//                        if (n[0] != LevelElement.Hole) AddWallE(x, y, -1);
//                        if (n[1] != LevelElement.Hole) AddWallW(x, y, -1);
//                        if (n[2] != LevelElement.Hole) AddWallN(x, y, -1);
//                        if (n[3] != LevelElement.Hole) AddWallS(x, y, -1);
//                    }
//                }
//                else
//                {
//                    if (n.Any(x => x != LevelElement.Wall))
//                        AddCap(x, y);
//                }
//            }
//        }
//    }

//    private void AddUvsForTile(int x, int y)
//    {
//        var sizeX = 1 / (float)TILES_X;
//        var sizeY = 1 / (float)TILES_Y;
//        float margin = 0.01f;

//        var startX = sizeX * x + margin;
//        var endX = startX + sizeX - margin;

//        var startY = 1f - (sizeY * y + margin);
//        var endY = 1f - (sizeY * y + sizeY - margin);

//        uvs.Add(new Vector2(endX, endY));
//        uvs.Add(new Vector2(startX, endY));
//        uvs.Add(new Vector2(endX, startY));
//        uvs.Add(new Vector2(startX, startY));
//    }

//    private void AddWallE(int x, int y, int offset = 0)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;
//        offset *= (int)Scale;

//        var last = vertices.Count;
//        vertices.Add(new Vector3(x + Scale, Scale+offset, y));
//        vertices.Add(new Vector3(x + Scale, Scale+offset, y + Scale));
//        vertices.Add(new Vector3(x + Scale, +offset, y));
//        vertices.Add(new Vector3(x + Scale, +offset, y + Scale));

//        AddUvsForTile(0, 0);

//        triangles.Add(last);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 3);
//    }

//    private void AddWallW(int x, int y, int offset = 0)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;
//        offset *= (int)Scale;

//        var last = vertices.Count;
//        vertices.Add(new Vector3(x, Scale+offset, y));
//        vertices.Add(new Vector3(x, Scale+offset, y + Scale));
//        vertices.Add(new Vector3(x, +offset, y));
//        vertices.Add(new Vector3(x, +offset, y + Scale));

//        AddUvsForTile(0, 0);

//        triangles.Add(last);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 3);
//        triangles.Add(last + 2);
//    }

//    private void AddWallN(int x, int y, int offset = 0)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;
//        offset *= (int)Scale;

//        var last = vertices.Count;
//        vertices.Add(new Vector3(x, Scale +offset, y));
//        vertices.Add(new Vector3(x + Scale, Scale +offset, y));
//        vertices.Add(new Vector3(x, +offset, y));
//        vertices.Add(new Vector3(x+Scale, +offset, y));

//        AddUvsForTile(0, 0);

//        triangles.Add(last);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 3);
//    }

//    private void AddWallS(int x, int y, int offset = 0)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;
//        offset *= (int)Scale;

//        var last = vertices.Count;
//        vertices.Add(new Vector3(x, Scale +offset, y+Scale));
//        vertices.Add(new Vector3(x + Scale, Scale +offset, y+Scale));
//        vertices.Add(new Vector3(x, +offset, y+Scale));
//        vertices.Add(new Vector3(x + Scale, +offset, y+Scale));

//        AddUvsForTile(0, 0);

//        triangles.Add(last);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 3);
//        triangles.Add(last + 2);
//    }

//    private void AddFloor(int x, int y, int offset = 0)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;

//        var last = vertices.Count;

//        vertices.Add(new Vector3(x, 0, y));
//        vertices.Add(new Vector3(x, 0, y + Scale));
//        vertices.Add(new Vector3(x + Scale, 0, y));
//        vertices.Add(new Vector3(x + Scale, 0, y + Scale));

//        if (pg.Roll(80))
//        {
//            AddUvsForTile(1, 0);
//        } else
//        {
//            AddUvsForTile(1, 1);
//        }

//        triangles.Add(last);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 3);
//        triangles.Add(last + 2);
//    }

//    private void AddCap(int x, int y)
//    {
//        x *= (int)Scale;
//        y *= (int)Scale;

//        var last = vertices.Count;

//        vertices.Add(new Vector3(x, Scale, y));
//        vertices.Add(new Vector3(x, Scale, y + Scale));
//        vertices.Add(new Vector3(x + Scale, Scale, y));
//        vertices.Add(new Vector3(x + Scale, Scale, y + Scale));

//        AddUvsForTile(0, 1);

//        triangles.Add(last);
//        triangles.Add(last + 1);
//        triangles.Add(last + 2);
//        triangles.Add(last + 1);
//        triangles.Add(last + 3);
//        triangles.Add(last + 2);
//    }

//    private void SpawnRandomPowerupsInRoom(Node room)
//    {
//        var chance = pg.RangeF(0, 1);
//        var mod = mg.IsOnCriticalPath(room) ? 1 : 2;
            

//        var amountToSpawn = 0;

//        if (chance < TWO_POWERUPS_CHANCE * mod)
//            amountToSpawn = 1;
//        else if (chance < ONE_POWERUP_CHANCE * mod)
//            amountToSpawn = 2;
//        else
//            return;

//        var spawned = 0;

//        while (spawned < amountToSpawn)
//        {
//            var rndPowerup = this.pg.Select(PowerUps);
//            var spawn = pg.Select(room.POIs);
//            if (spawn.IsEmpty) continue;
//            var spawnedGO = Instantiate(rndPowerup, new Vector3(spawn.X * this.Scale, 1f, spawn.Y * this.Scale ), Quaternion.identity);
//            room.POIs.Remove(spawn);
//            spawned++;
//            this.spawnedPowerups.Add(spawnedGO);
//        }

//    }

//    private void CleanSpawnedGameObjectList(List<GameObject> list)
//    {
//        foreach (var gameObject in list)
//        {
//            DestroyImmediate(gameObject);
//        }
//        list.Clear();
//    }


//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
