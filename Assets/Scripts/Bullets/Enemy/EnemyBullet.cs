using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{

    protected override void Move()
    {
       transform.Translate(Vector3.forward * Register.instance.properties.e_Speed * Time.deltaTime, Space.Self);
    }

}
