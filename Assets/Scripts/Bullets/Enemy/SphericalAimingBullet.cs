using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalAimingBullet : NormalBullet
{

    protected override void Awake()
    {
        base.Awake();
        speed = Register.instance.propertiesSphericalAiming.bulletSpeed;
        destructionMargin = Register.instance.propertiesSphericalAiming.bulletDestructionMargin;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        Move();
        DisableGameobject();
    }

    protected override void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

}
