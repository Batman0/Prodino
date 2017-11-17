using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossEntry : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float speed = 0.5f;
    float journey = 0;

	void Update ()
    {
        BossEntry();
    }
	
    public void BossEntry()
    {
        float deltaSpeed = Time.deltaTime * speed;
        journey += deltaSpeed;
        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, journey);
    }
}
