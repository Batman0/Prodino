using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossBarrier : MonoBehaviour
{

    FortressBossEnemy fortress;

    void Awake()
    {
        fortress = GetComponentInParent<FortressBossEnemy>();
    }

    void OnTriggerEnter(Collider coll)
    {
        //IF MORSO 
        if (coll.gameObject.layer == Register.instance.PlayerLayer && GameManager.instance.currentGameMode == GameMode.SIDESCROLL && fortress.State == FortressBossEnemy.FortressState.ProtectingCore && GameManager.instance.playerState== PlayerController.PlayerState.Attacking)
        {
            fortress.BreakBarrier();
            gameObject.SetActive(false);
        }
    }
}
