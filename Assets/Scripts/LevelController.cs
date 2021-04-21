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
    public Tilemap FullWallTilemap;
    public Tilemap HalfWallTilemap;
    public Tilemap ShieldTilemap;

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
                    BaseEnemy newEnemy = Instantiate(spawnQueue.Dequeue(), spawnLocation.transform.position, spawnLocation.transform.rotation).GetComponent<BaseEnemy>();
                    newEnemy.FullWallTilemap = FullWallTilemap;
                    newEnemy.HalfWallTilemap = HalfWallTilemap;
                }
            }
        }

        if(Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Color c = ShieldTilemap.color;
        c.a = 0.6f + 0.1f * (float) Math.Sin(0.5 * Math.PI * Time.time);
        ShieldTilemap.color = c;
    }
}
