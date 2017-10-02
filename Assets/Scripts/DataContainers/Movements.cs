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
public static class Movements
{
    public static void StraightMove(Transform transform, bool isRight, float speed, float destructionMargin, ref bool destroy)
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (isRight)
        {
            if (transform.position.x <= Register.instance.xMin - destructionMargin)
            {
                destroy = true;
            }
        }
        else
        {
            if (transform.position.x >= Register.instance.xMax + destructionMargin)
            {
                destroy = true;
            }
        }
    }

    public static void CircularMove(Transform transform, float speed, bool isRight, float radius, Vector3 originalPos, ref float lifeTime, ref bool destroy)
    {
        //switch (GameManager.instance.currentGameMode)
        //{
        //    case GameMode.SIDESCROLL:
        if (isRight)
        {
            transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
        }
        else
        {
            transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
        }
        //        break;
        //    case GameMode.TOPDOWN:
        //        if (isRight)
        //        {
        //            transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, transform.position.y, radius * Mathf.Sin(Time.time * speed) + originalPos.y);
        //        }
        //        else
        //        {
        //            transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, transform.position.y, radius * Mathf.Sin(Time.time * speed) + originalPos.y);
        //        }
        //        break;
        //}

        if (lifeTime > 0.0f)
        {
            lifeTime -= Time.deltaTime;
        }
        else if (lifeTime <= 0.0f && !destroy)
        {
            destroy = true;
        }
    }

    public static void SquareMove(ref int index, float speed, float waitingTime, ref float waitingTimer, Transform[] targets, Transform transform, ref bool destroy)
    {
        //switch (GameManager.instance.currentGameMode)
        //{
        //    case GameMode.SIDESCROLL:
        if (transform.position != targets[index].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[index].position, speed * Time.deltaTime);
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
                    destroy = true;
                }
            }
        }
        //        break;
        //    case GameMode.TOPDOWN:
        //        if (transform.position != new Vector3(targets[index].position.x, 0, targets[index].position.z))
        //        {
        //            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targets[index].position.x, 0, targets[index].position.z), speed * Time.deltaTime);
        //        }
        //        else
        //        {
        //            if (waitingTimer < waitingTime && index < targets.Length - 1)
        //            {
        //                waitingTimer += Time.deltaTime;
        //            }
        //            else
        //            {
        //                if (index < targets.Length - 1)
        //                {
        //                    index++;
        //                    waitingTimer = 0.0f;
        //                }
        //                else
        //                {
        //                    destroy = true;
        //                }
        //            }
        //        }
        //        break;
        //}
    }

    public static void Move(MovementType movementType, Transform transform, bool isRight, Properties properties, Vector3 originalPos, ref int targetIndex, ref float lifeTime, ref float waitingTimer, ref bool toDestroy)
    {
        switch (movementType)
        {
            case MovementType.STRAIGHT:
                StraightMove(transform, isRight, properties.st_Speed, properties.st_DestructionMargin, ref toDestroy);
                break;
            case MovementType.CIRCULAR:
                CircularMove(transform, properties.c_Speed, isRight, properties.c_Radius, originalPos, ref lifeTime, ref toDestroy);
                break;
            case MovementType.SQUARE:
                if (isRight)
                {
                    SquareMove(ref targetIndex, properties.sq_Speed, properties.sq_WaitingTime, ref waitingTimer, properties.sq_RightTargets, transform, ref toDestroy);
                }
                else
                {
                    SquareMove(ref targetIndex, properties.sq_Speed, properties.sq_WaitingTime, ref waitingTimer, properties.sq_LeftTargets, transform, ref toDestroy);
                }
                break;
        }
    }
}
