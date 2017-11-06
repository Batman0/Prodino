using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLaser : BaseBullet
{

    private float xMax;
    private float fadeTime;

    private void Awake()
    {
        Debug.Log("SSSSSS");
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

    protected override void ChangePerspective()
    {
        //Debug.Log("SSSS");
        if (GameManager.instance.transitionIsRunning)
        {
            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideCollider.enabled)
                {
                    topCollider.enabled = false;
                    sideCollider.enabled = true;
                    if (sidescrollRotation.HasValue)
                    {
                        transform.rotation = sidescrollRotation.Value;
                    }
                }
            }
            else
            {
                if (!topCollider.enabled)
                {
                    sideCollider.enabled = false;
                    topCollider.enabled = true;
                }
            }
        }
    }

    private void Extend()
    {
        //Debug.Log("xMax: " + xMax);
        //Debug.Log("Mathf.Abs(transform.position.x): " + Mathf.Abs(transform.position.x));
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
