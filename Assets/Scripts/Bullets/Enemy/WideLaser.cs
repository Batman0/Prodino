using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLaser : BaseBullet
{

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine("Fade", Register.instance.propertiesTrail.fadeTime);
        //gameObject.SetActive(gameObject, Register.instance.propertiesTrail.fadeTime);
    }

    protected override void Update()
    {
        base.Update();
        Extend();
    }

    private void Extend()
    {
        if (transform.localScale.z - Mathf.Abs(transform.position.x) < Register.instance.xMax)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + Register.instance.propertiesTrail.trailSpeed * Time.deltaTime);
        }
    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
