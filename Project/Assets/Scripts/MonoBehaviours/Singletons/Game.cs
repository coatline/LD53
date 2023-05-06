using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class Game : Singleton<Game>
{
    public List<AllyData> Allies { get; private set; }
    public AllyData PlayerData { get; private set; }

    Level[] levels;
    int level;

    protected override void Awake()
    {
        base.Awake();

        int count = DataLibrary.I.Levels.Length;

        Allies = new List<AllyData>();
        levels = new Level[count];

        for (int i = 0; i < count; i++)
        {
            Level level = DataLibrary.I.Levels[i];
            levels[level.order] = level;
        }
    }

    public Level CurrentLevel()
    {
        if (level > levels.Length - 1)
        {
            SceneManager.LoadScene("Win");
            return null;
        }

        return levels[level];
    }

    /// <param name="playerHealth"></param>
    /// <param name="allies">Include player</param>
    public void CompletedLevel(List<AllyData> allies, AllyData playerData)
    {
        this.PlayerData = playerData;
        this.Allies = allies;
        level++;
    }
    public void Restart()
    {
        PlayerData = null;
        Allies.Clear();
        level = 0;
    }
}


public class AllyData
{
    public string prefabName;
    public float health;
    public Item weapon;
    public bool isPlayer;

    public AllyData(GameObject prefab, bool isPlayer = false)
    {
        this.prefabName = prefab.name;
        this.isPlayer = isPlayer;
        health = prefab.GetComponent<Damageable>().Health;
        weapon = prefab.GetComponent<ItemHolder>().Item;
    }

    public GameObject GetPrefab => DataLibrary.I.Allies[prefabName.Replace("(Clone)", "")].gameObject;
}