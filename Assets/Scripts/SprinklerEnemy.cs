using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerEnemy : BaseEnemy
{
    private int currentDirection;

    protected override void Update()
    {
        base.Update();

        transform.rotation = Quaternion.identity;

        if(canShoot)
        {
            Vector2 shootDirection;
            if(currentDirection % 2 == 0)
            {
                shootDirection = ((Vector2) adjacents[currentDirection / 2]).normalized;
            }
            else
            {
                shootDirection = ((Vector2) diagonals[currentDirection / 2]).normalized;
            }

            Shoot(shootDirection);
            Shoot(-shootDirection);

            currentDirection = (currentDirection + 1) % 4;
        }
    }
}
