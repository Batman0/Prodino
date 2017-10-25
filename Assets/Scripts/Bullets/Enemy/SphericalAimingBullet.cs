using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalAimingBullet : BaseBullet
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();
        DestroyGameobject(Register.instance.propertiesSphericalAiming.bulletDestructionMargin);
    }

    protected override void Move()
    {
        transform.Translate(direction * Register.instance.propertiesSphericalAiming.bulletSpeed * Time.deltaTime, Space.World);
    }

}
