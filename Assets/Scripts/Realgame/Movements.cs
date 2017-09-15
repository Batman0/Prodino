using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    STRAIGHT,
    CIRCULAR,
    SQUARE,
    DIAGONAL
}
public class Movements : MonoBehaviour
{


    public void SquareMove(ref int index, float speed, float waitingTime, ref float waitingTimer, Transform[] targets, Transform transform)
    {
        if (transform.position != targets[index].position)
        {
            switch (NewGameManager.instance.currentGameMode)
            {
                case GameMode.SIDESCROLL:
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targets[index].position.x, targets[index].position.y, 0), speed * Time.deltaTime);
                    break;
                case GameMode.TOPDOWN:
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targets[index].position.x, 0, targets[index].position.z), speed * Time.deltaTime);
                    break;
            }
        }
        else
        {
            if (waitingTimer < waitingTime && index < targets.Length - 1)
            {
                waitingTimer += Time.deltaTime;
            }
            else
            {
                if (index < targets.Length - 1)
                {
                    index++;
                    waitingTimer = 0.0f;
                }
                else
                {
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}
