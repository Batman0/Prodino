using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : BaseBullet
{
    protected override void Start()
    {
        base.Start();
        xDirection = Register.instance.player.transform.position;
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void Move()
    {
        //xDirection += direction * Time.deltaTime * Register.instance.propertiesDoubleAiming.bulletSpeed;
        // to do Da cambiare il seno con un'approssimazione con lerp tra un punto più alto ad un punto più basso
        if(gameObject.tag == "EnemyBulletInverse")
        {
            //transform.position = new Vector3(xDirection.x * Register.instance.propertiesDoubleAiming.bulletSpeed, transform.position.y, (transform.right * -Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin).z);
            transform.position = (xDirection * Register.instance.propertiesDoubleAiming.bulletSpeed) + transform.right * -Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin;
        }
        else
        {
            //transform.position = new Vector3(xDirection.x * Register.instance.propertiesDoubleAiming.bulletSpeed, transform.position.y, (transform.right * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin).z);
            transform.position = (xDirection * Register.instance.propertiesDoubleAiming.bulletSpeed) + transform.right * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin;
        }
    }
}
