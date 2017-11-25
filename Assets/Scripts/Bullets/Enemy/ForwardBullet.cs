using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBullet : NormalBullet
{

    protected override void Awake()
    {
        base.Awake();
        //speed = Register.instance.propertiesForwardShooter.bulletSpeed;
        //destructionMargin = Register.instance.propertiesForwardShooter.bulletDestructionMargin;
    }

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
