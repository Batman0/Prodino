using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : BaseBullet
{

    private float xStartPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = transform.position.x - Register.instance.player.transform.position.x >= 0 ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
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
            //transform.position = (transform.position + direction.normalized * Register.instance.propertiesDoubleAiming.bulletSpeed) + transform.right * -Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin;
            transform.position = new Vector3(Register.instance.propertiesDoubleAiming.xBulletSpeed * direction.x * Time.deltaTime + transform.position.x, transform.position.y, -Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.zBulletSpeed) * Register.instance.propertiesDoubleAiming.bulletAmplitude);
        }
        else
        {
            //transform.position = new Vector3(xDirection.x * Register.instance.propertiesDoubleAiming.bulletSpeed, transform.position.y, (transform.right * Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.bulletSpeed) * Register.instance.propertiesDoubleAiming.arcSin).z);
            transform.position = new Vector3(Register.instance.propertiesDoubleAiming.xBulletSpeed * direction.x * Time.deltaTime + transform.position.x, transform.position.y, Mathf.Sin(Time.time * Register.instance.propertiesDoubleAiming.zBulletSpeed) * Register.instance.propertiesDoubleAiming.bulletAmplitude);
        }
    }
}
