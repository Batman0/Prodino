using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLaser : BaseBullet
{

    private float xMax;
    private float fadeTime;

    private void Awake()
    {
        speed = Register.instance.propertiesTrail.trailSpeed;
        xMax = Register.instance.xMax;
        fadeTime = Register.instance.propertiesTrail.fadeTime;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine("Fade", fadeTime);
        //gameObject.SetActive(gameObject, Register.instance.propertiesTrail.fadeTime);
    }

    protected override void Update()
    {
        base.Update();
        Extend();
    }

    private void Extend()
    {
        if (transform.localScale.z - Mathf.Abs(transform.position.x) < xMax)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed * Time.deltaTime);
        }
    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
