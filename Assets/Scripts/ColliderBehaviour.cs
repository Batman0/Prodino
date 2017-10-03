using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBehaviour : MonoBehaviour
{

    public Collider collider;
    public delegate void OntriggerEnterBehaviour(Collider other);
    public OntriggerEnterBehaviour behaviour;

    public void OnTriggerEnter(Collider other)
    {
        behaviour(other);
    }

}
