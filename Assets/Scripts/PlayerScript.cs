using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator ani;
    private float horizontal;
    public float speed = 2f;

    // Use this for initialization
    void Awake()
    {
        //ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        //transform.Translate(0, 0, horizontal * -speed * Time.deltaTime);
        ani.SetFloat("horizontal", horizontal);
    }
}