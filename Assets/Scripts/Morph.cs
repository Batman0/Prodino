using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morph : MonoBehaviour
{
    private Animator ani;

	// Use this for initialization
	void Awake ()
    {
        ani = GetComponent<Animator>();
	}
    void Start()
    {
        ani.SetBool("IsFlying", false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * -10 * Time.deltaTime);
        }

		if(Input.GetKeyDown(KeyCode.Space))
        {
            if(ani.GetBool("IsFlying")==false)
            {
                ani.SetBool("IsFlying", true);
            }
            else
            {
                ani.SetBool("IsFlying", false);
            }
        }
	}
}
