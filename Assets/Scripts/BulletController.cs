using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed;
    public int Damage;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(this.gameObject);

        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Damage(Damage);
        }

        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().Damage(Damage);
        }
    }
}
