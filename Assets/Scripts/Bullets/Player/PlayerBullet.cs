using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : NormalBullet
{
    //private bool? isRight = null;
    //private bool? isCenter = null;

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //}

    protected override void Awake()
    {
        base.Awake();
        speed = Register.instance.propertiesPlayer.bulletSpeed;
        destructionMargin = Register.instance.propertiesPlayer.bulletDestructionMargin;
    }

    protected override void Update()
    {
        base.Update();
        DisableGameobject();
    }

    
    protected override void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

}
