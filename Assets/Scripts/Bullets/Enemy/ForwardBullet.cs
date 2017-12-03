using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBullet : NormalBullet
{
    //public PropertiesForwardShooter property;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    speed = property.bulletSpeed;
    //    destructionMargin = property.bulletDestructionMargin;
    //}

    protected override void OnEnable()
    {
        direction = transform.forward;
        base.OnEnable();
    }

    //protected override void Update()
    //{
    //    base.Update();
    //    DisableGameobject();
    //}

    protected override void Move()
    {
       transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

}
