using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDefault : Bullet
{
    protected override void Move()
    {
        switch (gameObject.tag)
        {
            case "PlayerBullet":
                if (GameManager.instance.cameraState == State.SIDESCROLL)
                {
                    if (isRight)
                    {
                        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime, Space.World);
                    }
                    else if (isLeft)
                    {
                        transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime, Space.World);
                    }
                    else if (isCenter)
                    {
                        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.World);
                    }
                }
                else if (GameManager.instance.cameraState == State.TOPDOWN)
                {
                    transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime, Space.Self);
                }
                break;
            case "EnemyBullet":
                transform.Translate(-Vector3.right * bulletSpeed * Time.deltaTime, Space.World);
                break;
        }
    }
}
