using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 100f;

    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float lookAngle = Vector2.SignedAngle(Vector2.up, mousePos - (Vector2) transform.position);
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1) * Speed * Time.deltaTime;
    }
}
