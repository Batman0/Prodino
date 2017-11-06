using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : NormalBullet
{
    //private bool? isRight = null;
    //private bool? isCenter = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        //if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        //{
        //    AssignDirection();
        //}
    }

    protected override void Awake()
    {
        base.Awake();
        speed = Register.instance.propertiesPlayer.bulletSpeed;
        destructionMargin = Register.instance.propertiesPlayer.bulletDestructionMargin;
        //Debug.Log(speed);
    }

    protected override void Update()
    {
        base.Update();
        Move();
        DisableGameobject();
    }

    //protected void AssignDirection()
    //{
    //    if (transform.position.x > Register.instance.player.transform.position.x)
    //    {
    //        isRight = true;
    //        isCenter = false;
    //    }
    //    else if (transform.position.x < Register.instance.player.transform.position.x)
    //    {
    //        isRight = false;
    //        isCenter = false;
    //    }
    //    else if (transform.position.x == Register.instance.player.transform.position.x)
    //    {
    //        isRight = false;
    //        isCenter = true;
    //    }
    //}

    //protected override void DisableGameobject()
    //{
    //    if (transform.position.x < Register.instance.xMin - destructionMargin || transform.position.x > Register.instance.xMax + destructionMargin || transform.position.y < Register.instance.yMin - destructionMargin || transform.position.y > Register.instance.yMax + destructionMargin)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}
    protected override void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    //isCenter = null;
    //    //isRight = null;
    //}
}
