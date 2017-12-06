using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingLaserBullet : SpecialBullet
{
    private float xMax;
    [SerializeField]
    private float fadeTime;

    private void Awake()
    {
        xMax = Register.instance.xMax;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

    private void FixedUpdate()
    {
        fadeTime -= Time.fixedDeltaTime;
        if (fadeTime <= 0.0f)
        {
            gameObject.SetActive(false);
        }
        Extend();
    }

    private void Extend()
    {
       
        if (transform.localScale.z - Mathf.Abs(transform.position.x) < xMax)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed * Time.fixedDeltaTime);
        }
    }
}
