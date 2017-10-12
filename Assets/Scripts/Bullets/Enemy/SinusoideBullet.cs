using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : BaseBullet
{
    protected override void Start()
    {
        base.Start();
        pos = transform.position;
        playerTransform = Register.instance.player.transform;
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void Move()
    {
        pos += transform.right * Time.deltaTime * Register.instance.propertiesDoubleAiming.bulletSpeed;

        if(gameObject.tag == "EnemyBulletInverse")
        {
            transform.position = pos + direction * (Mathf.Pow(Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed), -1f)) * Register.instance.propertiesDoubleAiming.arcSin;
        }
        else
        {
            transform.position = pos + direction * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin;
        }
        //transform.position = new Vector3(Register.instance.player.transform.position.x * Register.instance.propertiesDoubleAiming.bulletSpeed * Time.deltaTime,transform.position.y,(Register.instance.propertiesDoubleAiming.arcSin * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed)));
    }
}
