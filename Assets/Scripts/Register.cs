using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public static Register instance;
    [Header("Enemies")]
    public GameObject[] enemies;

    void Awake()
    {
        instance = this;
    }

}
