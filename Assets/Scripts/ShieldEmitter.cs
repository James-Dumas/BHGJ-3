using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShieldEmitter : MonoBehaviour
{
    public int ShieldHealth = 5;
    public float RechargeTime = 5f;
    public Tile ShieldTile;
    public Tilemap Tilemap;

    private int health;
    private bool active;
    private float rechargeStartTime;
    private Vector3Int tilePos;

    void Start()
    {
        health = 0;
        rechargeStartTime = Time.time;
        tilePos = Tilemap.WorldToCell(transform.position);
    }

    void Update()
    {
        if(Time.time - rechargeStartTime > RechargeTime)
        {
            if(health < ShieldHealth)
            {
                health = ShieldHealth;
            }

            if(!active)
            {
                active = true;
                GetComponent<Collider2D>().enabled = true;
                Tilemap.SetTile(tilePos, ShieldTile);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        health--;
        rechargeStartTime = Time.time;
        if(health == 0)
        {
            active = false;
            GetComponent<Collider2D>().enabled = false;
            Tilemap.SetTile(tilePos, null);
        }
    }
}
