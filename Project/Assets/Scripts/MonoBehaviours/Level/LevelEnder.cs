using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnder : MonoBehaviour
{
    [SerializeField] TMP_Text rForNextText;
    [SerializeField] TMP_Text youLostText;
    [SerializeField] Delivery deliveryPrefab;
    Damageable playerDamageable;
    PlayerInputs player;
    bool playerDead;
    bool canMoveOn;
    int enemies;
    int deaths;

    private void Start()
    {
        player = FindObjectOfType<PlayerInputs>();
        playerDamageable = player.GetComponent<Damageable>();
        playerDamageable.Died += PlayerDied;

        enemies = Game.I.CurrentLevel().TotalEnemies;
    }

    void PlayerDied()
    {
        playerDamageable.Died -= PlayerDied;

        playerDead = true;

        youLostText.gameObject.SetActive(true);
    }

    public void EnemyDied()
    {
        deaths++;

        if (deaths == enemies)
            LevelComplete();
    }

    void LevelComplete()
    {
        // release some confetti and poop your pants
        // Deliver an ally
        Delivery d = Instantiate(deliveryPrefab, new Vector3(-100000, 0, 0), Quaternion.identity);

        Vector2 dropPos = Vector2.zero;

        if (player)
            dropPos = player.transform.position;
        else if (FindObjectOfType<SmartAIController>())
            dropPos = FindObjectOfType<SmartAIController>().transform.position;

        d.DropOn(dropPos);

        if (playerDead)
            youLostText.text += "\nYour allies are better than you and beat the level!";

        StartCoroutine(DelayAllowContinue());
    }

    void Update()
    {
#if UNITY_EDITOR
        // DEBUG Restart
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene("Game");
#endif

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerDead)
            {
                Game.I.Restart();
                SceneManager.LoadScene("Game");
            }
            else if (canMoveOn)
            {
                SaveData();
                SceneManager.LoadScene("Game");
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
            if (playerDead)
                SceneManager.LoadScene("Menu");
    }

    void SaveData()
    {
        SmartAIController[] allies = FindObjectsOfType<SmartAIController>();

        List<AllyData> data = new List<AllyData>();

        for (int i = 0; i < allies.Length; i++)
            data.Add(new AllyData(allies[i].gameObject));

        Game.I.CompletedLevel(data, new AllyData(player.gameObject, true));
    }

    IEnumerator DelayAllowContinue()
    {
        yield return new WaitForSeconds(1.25f);
        canMoveOn = true;

        rForNextText.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (playerDamageable)
            playerDamageable.Died -= PlayerDied;
    }
}
