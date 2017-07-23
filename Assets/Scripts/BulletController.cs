using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed = 10.0f;
    public float lifeTime = 4.0f;

    void Start()
    {
        DestroyGameObject();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject, lifeTime);
    }

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}

		if (other.gameObject.tag == "Enemy") {
			Destroy (other.gameObject);
		}
	}

}
