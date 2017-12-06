using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : NormalBullet
{

    protected override void Awake()
    {
        base.Awake();
        myTargetLayer = Register.instance.EnemyLayer;
    }


    protected override void Move()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

}
