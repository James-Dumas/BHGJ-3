using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class LevelController : MonoBehaviour
{
    public int StartingMoney = 0;
    public PlayerController Player;
    public EnemyWave[] EnemyWaves;
    public GameObject[] SpawnLocations;
    public LayerMask SpawnBlockLayerMask;
    public Text WaveDisplay;
    public Text MoneyDisplay;
    public Tile ShieldTile;

    private float lastWaveTime;
    private int currentWave;
    private Queue<GameObject> spawnQueue;
    private Collider2D[] res = new Collider2D[1];

    void Start()
    {
        lastWaveTime = Time.time;
        currentWave = 0;
        spawnQueue = new Queue<GameObject>();
        Player.Money = StartingMoney;
    }

    void Update()
    {
        WaveDisplay.text = $"Wave {(currentWave > 0 ? currentWave.ToString() : "--")}/{EnemyWaves.Length}";
        MoneyDisplay.text = $"${Player.Money}";

        if(Time.time - lastWaveTime > 10f && currentWave < EnemyWaves.Length)
        {
            lastWaveTime = Time.time;
            foreach(GameObject enemyObj in EnemyWaves[currentWave].Enemies)
            {
                spawnQueue.Enqueue(enemyObj);
            }

            currentWave++;
        }

        foreach(GameObject spawnLocation in SpawnLocations)
        {
            if(spawnQueue.Count != 0)
            {
                if(Physics2D.OverlapCircleNonAlloc(spawnLocation.transform.position, 0.5f, res, SpawnBlockLayerMask) == 0)
                {
                    Instantiate(spawnQueue.Dequeue(), spawnLocation.transform.position, spawnLocation.transform.rotation);
                }
            }
        }

        if(Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Color tileColor = ShieldTile.color;
        tileColor.a = 0.65f + 0.1f * (float) Math.Sin(0.5 * Math.PI * Time.time);
        ShieldTile.color = tileColor;
    }
}
