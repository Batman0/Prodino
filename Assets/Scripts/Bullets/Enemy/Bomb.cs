using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public SphereCollider explosionCollider;
    public Rigidbody rb;

    void Update()
    {
        rb.AddForce(Vector3.down * Register.instance.properties.bd_BombFallSpeed,ForceMode.Acceleration);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag =="Env")
        {
            explosionCollider.enabled = true;
        }

        if(other.gameObject.tag =="Player")
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject, Register.instance.properties.bd_LifeTime);
    }
	
}
