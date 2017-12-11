using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossCore : MonoBehaviour {

    FortressBossEnemy fortress;

    void Awake()
    {
        fortress = GetComponentInParent<FortressBossEnemy>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlayerBullet" && GameManager.instance.currentGameMode == GameMode.SIDESCROLL && fortress.State == FortressBossEnemy.FortressState.VulnerableCore)
        {
            fortress.DealDamage();
        }
    }
}
