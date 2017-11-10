using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBombDrop : EnemyMovement
{

    private float destructionMargin;
    private PropertiesBombDrop properties;
    private float xMin;
    private float xMax;
    private Register register;

    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        register = Register.instance;
        properties = register.propertiesBombDrop;
        speed = properties.xSpeed;
        destructionMargin = properties.destructionMargin;
        xMin = register.xMin;
        xMax = register.xMax;
    }

    public override void Movement(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (enemy.isRight)
        {
            if (enemy.transform.position.x <= xMin - destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            if (enemy.transform.position.x >= xMax + destructionMargin)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    //public override void MoveTopdown(Enemy enemy)
    //{
    //    enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

    //    if (enemy.isRight)
    //    {
    //        if (enemy.transform.position.x <= Register.instance.xMin - destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.transform.position.x >= Register.instance.xMax + destructionMargin)
    //        {
    //            enemy.gameObject.SetActive(false);
    //        }
    //    }
    //}

}
