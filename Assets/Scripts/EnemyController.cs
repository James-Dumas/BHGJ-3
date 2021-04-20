using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 100f;
    public float ShootTimer = 1f;
    public float Spread = 0f;
    public int MaxHealth = 20;
    public int KillValue = 5;
    public GameObject Projectile;
    public LayerMask SightMask;

    private Rigidbody2D rigidbody;
    private GameObject player;
    private bool hasLineOfSight;
    private float lastShootTime;
    private Vector2 movement;
    private int health;
    private float beatTime;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        movement = -Vector2.up;
        lastShootTime = Time.time;
        health = MaxHealth;
    }

    void Update()
    {
        Vector2 towardsPlayer = Vector2.up;
        if(player != null)
        {
            towardsPlayer = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, towardsPlayer, 1000f, SightMask);
            hasLineOfSight = hit.collider.gameObject == player;
        }
        else
        {
            hasLineOfSight = false;
        }

        if(hasLineOfSight)
        {
            float lookAngle = Vector2.SignedAngle(Vector2.up, towardsPlayer);
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            if(Time.time - lastShootTime > ShootTimer)
            {
                Shoot(towardsPlayer.normalized);
            }
        }
        else
        {
            float lookAngle = Vector2.SignedAngle(Vector2.up, movement);
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        }

        if(towardsPlayer.magnitude < 4)
        {
            movement = -towardsPlayer.normalized;
        }
        else if(towardsPlayer.magnitude > 6)
        {
            movement = 0.7f * towardsPlayer.normalized;
        }
        else
        {
            movement = 0.4f * Vector2.Perpendicular(towardsPlayer).normalized;
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movement * Speed * Time.fixedDeltaTime;
    }

    private void Shoot(Vector2 direction)
    {
        lastShootTime = Time.time;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Instantiate(Projectile, transform.position + new Vector3(direction.x, direction.y, 0f) * 0.5f, Quaternion.Euler(0f, 0f, angle));
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            player.GetComponent<PlayerController>().Money += KillValue;
            Destroy(this.gameObject);
        }
    }
}
