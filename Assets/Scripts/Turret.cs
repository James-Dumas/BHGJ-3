using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float ShootTimer = 0.5f;
    public float Spread = 0f;
    public GameObject Projectile;
    public LayerMask SightMask;

    private List<GameObject> targetingEnemies;
    private float nextShootTime;

    public float BeatTime { get; set; }

    void Start()
    {
        targetingEnemies = new List<GameObject>();
        nextShootTime = BeatTime;
    }

    void Update()
    {
        GameObject nearestVisibleEnemy = null;
        float nearestVisibleEnemyDist = Single.PositiveInfinity;
        Vector2 towardsNearestEnemy = Vector2.up;
        foreach(GameObject enemy in targetingEnemies)
        {
            if(enemy != null)
            {
                Vector2 towardsEnemy = enemy.transform.position - transform.position;
                bool hasLineOfSight = Physics2D.Raycast(transform.position, towardsEnemy, 1000f, SightMask).collider.gameObject == enemy;

                if(hasLineOfSight && towardsEnemy.magnitude < nearestVisibleEnemyDist)
                {
                    nearestVisibleEnemy = enemy;
                    nearestVisibleEnemyDist = towardsEnemy.magnitude;
                    towardsNearestEnemy = towardsEnemy;
                }
            }
        }

        if(Time.time > nextShootTime)
        {
            nextShootTime += ShootTimer;
            if(nearestVisibleEnemy != null)
            {
                Shoot(towardsNearestEnemy.normalized);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        targetingEnemies.Add(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        targetingEnemies.Remove(col.gameObject);
    }

    private void Shoot(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Instantiate(Projectile, transform.position + new Vector3(direction.x, direction.y, 0f) * 0.5f, Quaternion.Euler(0f, 0f, angle));
    }
}
