using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBehaviour : EnemyBehaviour {

    private float destructionMargin;
    private PropertiesForward properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesForward;
        speed = properties.xSpeed;
        destructionMargin = properties.destructionMargin;
    }

    public override void Move()
    {
        enemyInstance.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemyInstance.isRight)
        {
            if (enemyInstance.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemyInstance.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                enemyInstance.gameObject.SetActive(false);
            }
        }
    }

    public override void Shoot()
    {
        return;
    }
}
