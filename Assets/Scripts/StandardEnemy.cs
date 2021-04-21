using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : BaseEnemy
{
    protected override void Update()
    {
        base.Update();

        if(hasLineOfSight)
        {
            float lookAngle = Vector2.SignedAngle(Vector2.up, towardsPlayer);
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
            if(canShoot)
            {
                Shoot(towardsPlayer.normalized);
            }
        }
        else
        {
            float lookAngle = Vector2.SignedAngle(Vector2.up, rigidbody.velocity);
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        }

    }
}
