using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnoidHead : MonoBehaviour
{


    AracnoidEnemy aracnoid;
    Material headMaterial;
    public float colorBlinkTime = 0.3f;

    void Awake()
    {
        aracnoid = transform.parent.GetComponent<AracnoidEnemy>();
        headMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlayerBullet" && GameManager.instance.currentGameMode == GameMode.SIDESCROLL && aracnoid.state == AracnoidEnemy.AracnoidState.wounded)
        {
            coll.gameObject.SetActive(false);
            aracnoid.GetDamage();
            headMaterial.color = Color.red;
            StartCoroutine(ResetColor());
        }
    }

    public IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(colorBlinkTime);
        StopAllCoroutines();
        if(aracnoid.state == AracnoidEnemy.AracnoidState.wounded)
        {
            headMaterial.color = Color.green;
        }
        else
        {
            headMaterial.color = Color.black;
        }
    }

}
