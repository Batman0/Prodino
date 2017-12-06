using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalAimingBullet : NormalBullet
{

    protected override void Move()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

}
