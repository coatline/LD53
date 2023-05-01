using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Level ", menuName = "Level")]

public class Level : ScriptableObject
{
    public int order;

    public EnemySpawn[] enemies;

    public TileBase[] decorTiles;
    public TileBase floorTile;
    public TileBase wallTile;

    public int[] walkerIterations;
    public int levelWidth;
    public int levelHeight;

    public Vector2Int borderSize;

    public int TotalEnemies
    {
        get
        {
            int totalEnemies = 0;

            for (int i = 0; i < enemies.Length; i++)
                totalEnemies += enemies[i].count;

            return totalEnemies;
        }
    }
}

[System.Serializable]
public class EnemySpawn
{
    public AiController enemyPrefab;
    public int count;
}