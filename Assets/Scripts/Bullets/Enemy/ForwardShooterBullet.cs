using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShooterBullet : BaseBullet
{

    protected override void Update()
    {
        base.Update();
        DestroyGameobject(Register.instance.propertiesForwardShooter.bulletDestructionMargin);
    }

    protected override void Move()
    {
       transform.Translate(Vector3.forward * Register.instance.propertiesForwardShooter.bulletSpeed * Time.deltaTime, Space.Self);
    }

}
