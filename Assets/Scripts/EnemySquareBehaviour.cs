using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareBehaviour : EnemyBehaviour
{

    public float speed = 3.0f;
    public float squareSideLenght;
    private float SquareSideCount;
    private Vector3 moveVector;

    void Move(Vector3 moveVector)
    {
        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        SquareSideCount += moveVector.magnitude * speed * Time.deltaTime;
    }

   /* void ChangeMoveVector()
    {
        if(SquareSideCount >= squareSideLenght)
        {
            moveVector
        }
    }*/

}
