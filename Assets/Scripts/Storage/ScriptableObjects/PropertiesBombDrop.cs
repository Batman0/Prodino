using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesBombDrop : Properties
{
    public float xMovementSpeed;
    public float destructionMargin;
    public float loadingTime;
    public float bombFallSpeed;
    public float lifeTime;
    public GameObject gameObjectPrefab;
    public GameObject bombPrefab;
}
