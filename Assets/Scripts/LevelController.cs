using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public EnemyWave[] EnemyWaves;
    public GameObject[] SpawnLocations;
    public LayerMask SpawnBlockLayerMask;

    private float lastWaveTime;
    private int currentWave;
    private Queue<GameObject> spawnQueue;
    private Collider2D[] res = new Collider2D[1];
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        lastWaveTime = Time.time;
        currentWave = 0;
        spawnQueue = new Queue<GameObject>();
        player = Instantiate(PlayerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
