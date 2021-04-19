using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProximityMine : MonoBehaviour
{
    public float PlacementCooldown;
    public float TriggerRadius;
    public float DamageRadius;
    public float MaxDamage;
    public float MinDamage;
    public Tilemap Tilemap;

    private float placementTime;
    private bool isTriggered;
    private List<GameObject> nearbyEnemies;
    private float beatTime;

    void Start()
    {
        placementTime = Time.time;
        isTriggered = false;
        nearbyEnemies = new List<GameObject>();
    }

    void Update()
    {
        foreach(GameObject enemy in nearbyEnemies)
        {
            if((enemy.transform.position - transform.position).magnitude < TriggerRadius)
            {
                isTriggered = true;
            }
        }

        if(isTriggered)
        {
            // my main goal
            foreach(GameObject enemy in nearbyEnemies)
            {
                float dist = (enemy.transform.position - transform.position).magnitude;
                if(dist < DamageRadius)
                {
                    int damage = (int) Math.Ceiling(Math.Min(MaxDamage, Math.Max(MinDamage, (DamageRadius - dist) / DamageRadius * (MaxDamage - MinDamage) + MinDamage)));
                    enemy.GetComponent<EnemyController>().Damage(damage);
                }
            }

            Tilemap.SetTile(Tilemap.WorldToCell(transform.position), null);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        nearbyEnemies.Add(col.gameObject);
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        nearbyEnemies.Remove(col.gameObject);
    }
}
