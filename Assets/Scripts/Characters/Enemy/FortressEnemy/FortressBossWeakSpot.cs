using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossWeakSpot : MonoBehaviour {


    FortressBossEnemy fortress;
    Material myMaterial;
    bool hit;

    void Awake()
    {
        fortress = GetComponentInParent<FortressBossEnemy>();
        myMaterial = GetComponent<MeshRenderer>().material;
    }

	void Start () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        if (!hit)
        {
            if (coll.tag == "PlayerBullet" && GameManager.instance.currentGameMode == GameMode.TOPDOWN && fortress.State == FortressBossEnemy.FortressState.Vulnerable)
            {
                hit = true;
                fortress.WeakSpotHit();
                myMaterial.color = Color.green;
            }
        }
    }

	void Update () {
        if (fortress.State == FortressBossEnemy.FortressState.Vulnerable)
        {
            if (hit)
            {
                myMaterial.color = Color.red;
            }
            else
            {
                myMaterial.color = Color.green;
            }
        }
        else
        {
            hit = false;
            myMaterial.color = Color.gray;
        }
	}
    void Recover()
    {
        hit = false;
    }
}
