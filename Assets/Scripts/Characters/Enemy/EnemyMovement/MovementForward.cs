using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementForward : EnemyMovement {

    private float destructionMargin;
    private PropertiesForward properties;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        properties = Register.instance.propertiesForward;
        speed = properties.xSpeed;
        destructionMargin = properties.destructionMargin;
    }

    public override void MoveSidescroll(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public override void MoveTopdown(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
