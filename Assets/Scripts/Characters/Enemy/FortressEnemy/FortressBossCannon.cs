using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossCannon : MonoBehaviour
{

    bool isShooting;
    public void Shoot(FortressBullet bullet, float speed)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.Speed = speed;
    }
}
