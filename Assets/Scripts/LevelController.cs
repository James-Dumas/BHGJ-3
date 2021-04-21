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
    public Text Countdown;
    public Tilemap FullWallTilemap;
    public Tilemap HalfWallTilemap;
    public Tilemap ShieldTilemap;
    public GameObject PreStartOverlay;
    public GameObject InGameUI;

    private bool started;
    private float startTime;
    private float nextWaveTime;
    private float nextSecondTime;
    private int countdownValue;
    private int currentWave;
    private Queue<GameObject> spawnQueue;
    private Collider2D[] res = new Collider2D[1];

    void Start()
    {
        started = false;
        spawnQueue = new Queue<GameObject>();
    }

    void Update()
    {
        if(started)
        {
            WaveDisplay.text = $"Wave {(currentWave > 0 ? currentWave.ToString() : "--")}/{EnemyWaves.Length}";
            MoneyDisplay.text = $"${Player.Money}";

            if(Time.time > nextWaveTime && currentWave < EnemyWaves.Length)
            {
                nextWaveTime += 10f;
                foreach(GameObject enemyObj in EnemyWaves[currentWave].Enemies)
                {
                    spawnQueue.Enqueue(enemyObj);
                }

                currentWave++;
            }

            if(Countdown.transform.localScale.x > 1)
            {
                Countdown.transform.localScale -= new Vector3(4f, 4f, 0f) * Time.deltaTime;
            }
            else if(Countdown.transform.localScale.x < 1)
            {
                Countdown.transform.localScale = new Vector3(1f, 1f, 0);
            }

            if(Time.time > nextSecondTime)
            {
                nextSecondTime += 1f;
                countdownValue--;
                if(countdownValue == 0)
                {
                    countdownValue = 10;
                }

                Countdown.text = countdownValue.ToString();
                Countdown.transform.localScale = new Vector3(1.4f, 1.4f, 0f);
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
                        newEnemy.BeatTime = nextSecondTime;
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

        if(!started && Input.GetButtonDown("Start Level"))
        {
            StartLevel();
        }
    }

    public void StartLevel()
    {
        started = true;
        startTime = Time.time + 0.1f;
        nextWaveTime = startTime + 10f;
        nextSecondTime = startTime + 1f;
        countdownValue = 10;
        currentWave = 0;
        Player.Money = StartingMoney;
        Player.BeatTime = startTime;
        Player.LevelStarted = true;
        PreStartOverlay.SetActive(false);
        InGameUI.SetActive(true);
    }
}
