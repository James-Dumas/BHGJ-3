using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 100f;
    public float ShootTimer = 1f;
    public float Spread = 0f;
    public GameObject Projectile;
    public LayerMask SightMask;

    private Rigidbody2D rigidbody;
    private GameObject player;
    private bool hasLineOfSight;
    private float lastShootTime;
    private Vector2 movement;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        movement = -Vector2.up;
    }

    void Update()
    {
        Vector2 towardsPlayer = player.transform.position - transform.position;

        if(player != null)
        {
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
                Shoot();
            }
        }
        else
        {
            float lookAngle = Vector2.SignedAngle(Vector2.up, movement);
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movement * Speed * Time.fixedDeltaTime;
    }

    private void Shoot()
    {
        lastShootTime = Time.time;
        Instantiate(Projectile, transform.position + transform.forward * 0.5f, transform.rotation * Quaternion.Euler(0f, 0f, (Random.value - 0.5f) * Spread));
    }
}
