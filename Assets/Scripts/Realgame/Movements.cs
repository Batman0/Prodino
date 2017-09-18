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
        if (isRight)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }

        if (isRight)
        {
            if (transform.position.x <= GameManager.instance.leftBound.x - destructionMargin)
            {
                destroy = true;
            }
        }
        else
        {
            if (transform.position.x >= GameManager.instance.rightBound.x + destructionMargin)
            {
                destroy = true;
            }
        }
    }

    public static void CircularMove(Transform transform, float speed, bool isRight, float radius, Vector3 originalPos, ref float lifeTime, ref bool destroy)
    {
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                if (isRight)
                {
                    transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, transform.position.z);
                }
                break;
            case GameMode.TOPDOWN:
                if (isRight)
                {
                    transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, transform.position.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
                }
                else
                {
                    transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, transform.position.y, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
                }
                break;
        }

        if (lifeTime > 0.0f)
        {
            lifeTime -= Time.deltaTime;
        }
        else if(lifeTime <= 0.0f && !destroy)
        {
            destroy = true;
        }
    }

    public static void SquareMove(ref int index, float speed, float waitingTime, ref float waitingTimer, Transform[] targets, Transform transform, ref bool destroy)
    {
        if (transform.position != targets[index].position)
        {
            switch (GameManager.instance.currentGameMode)
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
                    destroy = true;
                }
            }
        }
    }
}
