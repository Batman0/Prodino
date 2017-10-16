using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardShooterBullet : BaseBullet
{

    protected override void Start()
    {
        direction = transform.forward;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();
        DestroyGameobject(Register.instance.propertiesForwardShooter.bulletDestructionMargin);
    }

    protected override void Move()
    {
       transform.Translate(direction * Register.instance.propertiesForwardShooter.bulletSpeed * Time.deltaTime, Space.World);
    }

}
