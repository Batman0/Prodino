using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    private int index;
    private float waitingTimer;
    [HideInInspector]
    public MovementType movementType;
    private Movements movements;
    public EnemyProperties enemyProperties;

    private void Awake()
    {
        movements = new Movements();
        index = 0;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        switch (movementType)
        {
            case MovementType.SQUARE:
                movements.SquareMove(ref index, enemyProperties.Sq_speed, enemyProperties.Sq_waitingTime, ref waitingTimer, enemyProperties.Sq_targets, transform);
                break;
            default:
                Debug.Log("Nothing");
                break;
        }
    }
}
