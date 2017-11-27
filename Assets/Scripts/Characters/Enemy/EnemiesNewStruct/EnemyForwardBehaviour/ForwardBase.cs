using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBase : EnemyForward
{
    private new PropertiesForward property;
   
    public override void InitEnemy()
    {
        base.InitEnemy();
        enemyLives = property.lives;
        speed = property.xSpeed;
        destructionMargin = property.destructionMargin;
        enemyLives = property.lives;
    }

    public override void SetProperty(ScriptableObject _property)
    {
        property = (PropertiesForward)_property;
    }
}
