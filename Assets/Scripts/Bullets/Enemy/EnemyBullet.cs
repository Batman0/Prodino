using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{

    protected override void Move()
    {
        //if(this.gameObject.layer.ToString()=="Left")
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        }
        /*else
        {
            transform.Translate(Vector3.right * -enemyProperties.D_bulletSpeed * Time.deltaTime, Space.Self);
        }*/
    }

}
