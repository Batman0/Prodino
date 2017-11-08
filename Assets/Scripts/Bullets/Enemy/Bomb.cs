using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    public CapsuleCollider explosionCollider;
    public Rigidbody rb;

    private void Start()
    {
        speed = Register.instance.propertiesBombDrop.bombFallSpeed;
        lifeTime = Register.instance.propertiesBombDrop.bombLifeTime;
    }

    void Update()
    {
        rb.AddForce(Vector3.down * speed, ForceMode.Acceleration);
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
            explosionCollider.enabled = false;
        }

        StartCoroutine("Destroy", lifeTime);
    }
	
    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        explosionCollider.enabled = false;
    }
}
