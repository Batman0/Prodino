using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidWeakSpot : MonoBehaviour
{

    AracnoidEnemy aracnoid;
    Material headMaterial;
    Material weakSpotMaterial;
    float blinkTime = 0.25f;
    void Awake()
    {
        Transform father = transform.parent;
        aracnoid = father.parent.GetComponent<AracnoidEnemy>();
        headMaterial = father.GetComponent<MeshRenderer>().material;
        weakSpotMaterial = GetComponent<MeshRenderer>().material;
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlayerBullet" && GameManager.instance.currentGameMode == GameMode.TOPDOWN && aracnoid.state== AracnoidEnemy.AracnoidState.vulnerable)
        {
            coll.gameObject.SetActive(false);
            aracnoid.WeakSpotHit();
            headMaterial.color = Color.green;
        }
    }

    public void WeakSpotRecovery()
    {
        headMaterial.color = Color.black;
    }

    public void StartBlink()
    {
        StartCoroutine(Blink(true));
    }

    IEnumerator Blink(bool on)
    {
        yield return new WaitForSeconds(blinkTime);
        StopAllCoroutines();
        if (aracnoid.state==AracnoidEnemy.AracnoidState.vulnerable)
        {
            if (on)
            {
                weakSpotMaterial.color = Color.red;
                StartCoroutine(Blink(false));
            }
            else
            {
                weakSpotMaterial.color = Color.cyan;
                StartCoroutine(Blink(true));
            }
        }

    }
}
