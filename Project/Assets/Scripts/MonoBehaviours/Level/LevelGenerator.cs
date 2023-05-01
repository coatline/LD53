using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] float minEnemySpawnDist;
    [SerializeField] PlayerInputs playerPrefab;
    [SerializeField] Tilemap decorTilemap;
    [SerializeField] LevelEnder levelEnder;
    [SerializeField] Tilemap tilemap;

    PlayerInputs player;
    Level level;
    int[,] map;

    List<Vector2Int> floorTiles;
    Vector2Int walkerPos;
    int walkerSize;


    void Awake()
    {
        floorTiles = new List<Vector2Int>();

        level = Game.I.CurrentLevel();

        if (level == null)
            return;

        map = new int[level.levelWidth, level.levelHeight];

        for (int i = 0; i < level.walkerIterations.Length; i++)
        {
            Walk(level.walkerIterations[i]);
        }

        Smooth();
        AddWallsAndFloors();
        AddEnemies();
        Decorate();
    }

    void AddEnemies()
    {
        for (int i = 0; i < level.enemies.Length; i++)
        {
            EnemySpawn d = level.enemies[i];

            for (int k = 0; k < d.count; k++)
            {
                if (floorTiles.Count == 0) { Debug.LogWarning("No more floor tiles!!"); return; }

                var index = Random.Range(0, floorTiles.Count);
                var pos = floorTiles[index];

                if (Vector2.Distance(player.transform.position, pos) < minEnemySpawnDist)
                {
                    for (int j = 0; j < floorTiles.Count; j++)
                    {
                        if (Vector2.Distance(player.transform.position, floorTiles[j]) >= minEnemySpawnDist)
                        {
                            index = j;
                            pos = floorTiles[index];
                            break;
                        }
                    }
                }

                floorTiles.RemoveAt(index);

                var ene = Instantiate(d.enemyPrefab, new Vector3(pos.x + 0.5f, pos.y + 0.5f), Quaternion.identity);
                ene.GetComponent<Damageable>().Died += levelEnder.EnemyDied;
            }
        }
    }

    bool WithinMap(int x, int y) => (x < level.levelWidth - 1 && y < level.levelHeight - 1 && x > 0 && y > 0);

    void Walk(int iterations)
    {
        walkerPos = new Vector2Int(level.levelWidth / 2, level.levelHeight / 2);

        for (int i = 0; i < iterations; i++)
        {
            if (RandSign1() == -1 && RandSign1() == -1)
                walkerSize = 1;
            else
                walkerSize = 2;

            int dir = RandSign1();

            Vector2Int movement = Vector2Int.zero;

            if (RandSign1() == -1)
                movement.x = dir;
            else
                movement.y = dir;

            if (!WithinMap(walkerPos.x + movement.x, walkerPos.y + movement.y))
                continue;

            walkerPos += movement;

            for (int x = 0; x < walkerSize; x++)
                for (int y = 0; y < walkerSize; y++)
                {
                    Vector2Int pos = new Vector2Int(walkerPos.x + x, walkerPos.y + y);

                    if (!WithinMap(pos.x, pos.y))
                        continue;

                    if (map[pos.x, pos.y] == 0)
                    {
                        floorTiles.Add(new Vector2Int(pos.x, pos.y));
                        map[pos.x, pos.y] = 1;
                    }
                }
        }
    }

    int alliesSpawned;

    void AddWallsAndFloors()
    {
        for (int x = 0; x < level.levelWidth; x++)
        {
            for (int y = 0; y < level.levelHeight; y++)
            {
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), level.wallTile);
                }
                else if (map[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x + .5f, y + .5f);

                    if (player == null)
                    {
                        player = Instantiate(playerPrefab, pos, Quaternion.identity);

                        if (Game.I.PlayerData != null)
                            SpawnAlly(player.gameObject, Game.I.PlayerData);
                    }
                    else if (alliesSpawned < Game.I.Allies.Count)
                    {
                        AllyData data = Game.I.Allies[alliesSpawned];
                        GameObject ally = Instantiate(data.GetPrefab, pos, Quaternion.identity);
                        SpawnAlly(ally, data);

                        alliesSpawned++;
                    }

                    tilemap.SetTile(new Vector3Int(x, y), level.floorTile);
                }

            }
        }
    }

    void SpawnAlly(GameObject instance, AllyData data)
    {
        instance.GetComponent<Damageable>().Health = data.health;
        instance.GetComponent<ItemHolder>().ChangeItem(new GunStack(data.weapon, 1));
    }

    void Decorate()
    {
        for (int x = 0; x < level.levelWidth; x++)
        {
            for (int y = 0; y < level.levelHeight; y++)
            {
                if (map[x, y] == 1)
                    if (Random.Range(0, 90) == 0)
                        decorTilemap.SetTile(new Vector3Int(x, y, 0), level.decorTiles[Random.Range(0, level.decorTiles.Length)]);
            }
        }
    }

    int RandSign1()
    {
        if (Random.Range(0, 2) == 0)
            return 1;
        return -1;
    }

    void Smooth()
    {
        for (int x = 0; x < level.levelWidth; x++)
        {
            for (int y = 0; y < level.levelHeight; y++)
            {
                if (map[x, y] == 0 && WithinMap(x, y))
                {
                    int immediateWalls = 0;

                    if (map[x + 1, y] == 0)
                        immediateWalls++;
                    if (map[x - 1, y] == 0)
                        immediateWalls++;
                    if (map[x, y + 1] == 0)
                        immediateWalls++;
                    if (map[x, y - 1] == 0)
                        immediateWalls++;

                    if (immediateWalls <= 2)
                        map[x, y] = 1;
                }
            }
        }
    }
}






//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class LevelGenerator : MonoBehaviour
//{
//    [SerializeField] int mainRoomSize;
//    [SerializeField] int levelWidth;
//    [SerializeField] int levelHeight;
//    [SerializeField] Tilemap tilemap;
//    [SerializeField] Tile groundTile;
//    [SerializeField] Tile rocksTile;
//    [SerializeField] Tile wallTile;
//    List<Vector2Int> floors;
//    int[,] map;

//    void Awake()
//    {
//        map = new int[levelWidth, levelHeight];

//        Generate();
//        Smooth();
//        FillTiles();
//    }

//    void Generate()
//    {
//        for (int x = 0; x < levelWidth; x++)
//        {
//            for (int y = 0; y < levelHeight; y++)
//            {
//                map[x, y] = 1;
//            }
//        }

//        for (int x = 0; x < mainRoomSize; x++)
//        {
//            for (int y = 0; y < mainRoomSize; y++)
//            {
//                if (Random.Range(0f, 10) <= 5.5f)
//                    map[levelWidth / 4 + x, levelHeight / 4 + y] = 0;
//            }
//        }
//    }

//    void Smooth()
//    {
//        for (int x = 0; x < levelWidth; x++)
//        {
//            for (int y = 0; y < levelHeight; y++)
//            {
//                int landTileCount = 0;

//                if (x > 0)
//                {
//                    // 0 0 0
//                    // $ 0 0
//                    // 0 0 0
//                    if (map[x - 1, y] == 1)
//                        landTileCount++;

//                    if (y > 0)
//                    {
//                        // 0 0 0
//                        // 0 0 0
//                        // $ 0 0
//                        if (map[x - 1, y - 1] == 1)
//                            landTileCount++;
//                    }

//                    if (y < levelHeight - 1)
//                    {
//                        // $ 0 0
//                        // 0 0 0
//                        // 0 0 0
//                        if (map[x - 1, y + 1] == 1)
//                            landTileCount++;
//                    }
//                }


//                if (x < levelWidth - 1)
//                {

//                    if (y > 0)
//                    {
//                        // 0 0 0
//                        // 0 0 0
//                        // 0 0 $
//                        if (map[x + 1, y - 1] == 1)
//                            landTileCount++;
//                    }

//                    // 0 0 0
//                    // 0 0 $
//                    // 0 0 0
//                    if (map[x + 1, y] == 1)
//                    {
//                        landTileCount++;
//                    }

//                    if (y < levelHeight - 1)
//                    {
//                        // 0 0 $
//                        // 0 0 0
//                        // 0 0 0
//                        if (map[x + 1, y + 1] == 1)
//                            landTileCount++;
//                    }
//                }

//                if (y > 0)
//                {
//                    // 0s 0 0
//                    // 0 0 0
//                    // 0 $ 0
//                    if (map[x, y - 1] == 1)
//                        landTileCount++;
//                }

//                if (y < levelHeight - 1)
//                {
//                    // 0 $ 0
//                    // 0 0 0
//                    // 0 0 0
//                    if (map[x, y + 1] == 1)
//                        landTileCount++;
//                }

//                if (landTileCount > 4)
//                    map[x, y] = 1;
//                else if (landTileCount < 4)
//                    map[x, y] = 0;
//            }
//        }
//    }

//    void CutOutEmptyCells()
//    {
//        for (int x = 0; x < levelWidth; x++)
//        {
//            for (int y = 0; y < levelHeight; y++)
//            {
//                if (map[x, y] == 0)
//                    floors.Add(new Vector2Int(x, y));
//            }
//        }

//        for (int i = 0; i < floors.Count; i++)
//        {
//            Vector2Int f = floors[i];

//            if (map[f.x + 1, f.y] == 1)
//            {

//            }
//        }
//    }

//    void FillTiles()
//    {
//        for (int x = 0; x < levelWidth; x++)
//            for (int y = 0; y < levelHeight; y++)
//            {
//                if (map[x, y] == 0)
//                    tilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
//                else
//                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
//            }
//    }
//}
