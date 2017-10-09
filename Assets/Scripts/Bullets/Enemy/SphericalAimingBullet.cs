using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalAimingBullet : BaseBullet
{

    protected override void Update()
    {
        base.Update();
        DestroyGameobject(Register.instance.propertiesSphericalAiming.bulletDestructionMargin);
    }

    protected override void Move()
    {
        transform.Translate(Vector3.forward * Register.instance.propertiesSphericalAiming.bulletSpeed * Time.deltaTime, Space.Self);
    }

}
