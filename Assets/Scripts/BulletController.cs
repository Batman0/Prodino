﻿using System.Collections;
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

    void DestroyGameObject()
    {
        Destroy(gameObject, lifeTime);
    }

	void OnTriggerEnter(Collider other){
		if (this.gameObject.tag == "EnemyBullet" && other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}

		if (this.gameObject.tag == "PlyerBullet" && other.gameObject.tag == "Enemy") {
			Destroy (other.gameObject);
		}
	}

}
