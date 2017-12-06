using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBullet : NormalBullet
{
    

    protected override void OnEnable()
    {
        direction = transform.forward;
        base.OnEnable();
    }

    protected override void Move()
    {
       transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

}
