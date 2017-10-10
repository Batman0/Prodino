using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : BaseBullet
{

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void Move()
    {
        transform.position= new Vector3(Vector3.MoveTowards(transform.position, Register.instance.player.transform.position, 0.5f).x,transform.position.y,Register.instance.propertiesDoubleAiming.arcSin * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed));
    }
}
