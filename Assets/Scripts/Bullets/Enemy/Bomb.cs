using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private MeshRenderer mesh;
    public SphereCollider explosionCollider;
    public Properties properties;

    void awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag =="Env")
        {
            explosionCollider.enabled = true;
            mesh.enabled = false;
            Destroy(gameObject, properties.b_lifeTime);
        }

        if(other.gameObject.tag =="Player")
        {
            Destroy(other.gameObject);
            Destroy(gameObject, properties.b_lifeTime);
        }
    }
	
}
