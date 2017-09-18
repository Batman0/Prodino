using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseBullet
{
    protected bool isRight;
    protected bool isCenter;

    private void Awake()
    {
        AssignDirection();
    }

    protected void AssignDirection()
    {
        if (transform.position.x > GameManager.instance.player.transform.position.x)
        {
            isRight = true;
            isCenter = false;
        }
        else if (transform.position.x < GameManager.instance.player.transform.position.x)
        {
            isRight = false;
            isCenter = false;
        }
        else if (transform.position.x == GameManager.instance.player.transform.position.x)
        {
            isRight = false;
            isCenter = true;
        }
    }

    protected override void Move()
    {
        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            if (isRight && !isCenter)
            {
                transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime, Space.World);
            }
            else if (!isRight && !isCenter)
            {
                transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime, Space.World);
            }
            else if (!isRight && isCenter)
            {
                transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.World);
            }
        }
        else if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
        }
    }
}
