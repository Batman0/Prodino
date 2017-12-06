using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBullet : BaseBullet
{
    protected override void ChangeGameState(GameMode currentState)
    {
        //if (GameManager.instance.transitionIsRunning)
        {
            if (currentState == GameMode.TOPDOWN)
            {
               // if (!sideCollider.enabled)
                {
                    topCollider.enabled = true;
                    sideCollider.enabled = false;
                }
            }
            else
            {
                //if (!topCollider.enabled)
                {
                    sideCollider.enabled = true;
                    topCollider.enabled = false;
                    if (sidescrollRotation.HasValue)
                    {
                        transform.rotation = sidescrollRotation.Value;
                    }
                }
            }
        }
    }
}
