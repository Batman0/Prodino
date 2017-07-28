using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10.0f;
    public float lifeTime = 4.0f;

    void Start()
    {
      
        DestroyGameObject();
    }

    void FixedUpdate()
    {
        switch (this.gameObject.tag)
        {
            case "PlayerBullet":
                transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
                break;
            case "EnemyBullet":
                transform.Translate(-Vector3.right * bulletSpeed * Time.deltaTime);
                break;
        }
        
    }

    void DestroyGameObject()
    {
        Destroy(gameObject, lifeTime);
    }

	void OnTriggerEnter(Collider other){
		if (this.gameObject.tag == "EnemyBullet" && other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}

		if (this.gameObject.tag == "PlayerBullet" && other.gameObject.tag == "Enemy") {
			Destroy (other.gameObject);
		}
	}

}
