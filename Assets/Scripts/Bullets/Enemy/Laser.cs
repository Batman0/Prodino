using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BaseBullet
{

    private void Start()
    {
        Destroy(gameObject, Register.instance.propertiesLaserDiagonal.shootingTime);
    }

    //private void Update()
    //{
    //    Extend();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //private void Extend()
    //{
    //    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + Register.instance.properties.l_Speed * Time.deltaTime);
    //}

}
