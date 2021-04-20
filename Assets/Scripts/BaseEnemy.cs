using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public float Speed = 100f;
    public float ShootTimer = 1f;
    public float Spread = 0f;
    public int MaxHealth = 20;
    public int KillValue = 5;
    public GameObject Projectile;
    public LayerMask SightMask;

    protected Rigidbody2D rigidbody;
    protected GameObject player;
    protected float lastShootTime;
    protected Vector2 movement;
    protected int health;
    protected float beatTime;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        movement = -Vector2.up;
        lastShootTime = Time.time;
        health = MaxHealth;
    }

    protected virtual void Update()
    {
        
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
        if(health == 0)
        {
            player.GetComponent<PlayerController>().Money += KillValue;
            Destroy(this.gameObject);
        }
    }
}
