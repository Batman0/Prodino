using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public CapsuleCollider explosionCollider;
    public Rigidbody rb;

    void Update()
    {
        rb.AddForce(Vector3.down * Register.instance.propertiesBombDrop.bombFallSpeed,ForceMode.Acceleration);
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

        StartCoroutine("Destroy", Register.instance.propertiesBombDrop.bombLifeTime);
    }
	
    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
