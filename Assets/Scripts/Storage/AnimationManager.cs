using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    private Animator playerAnimator;
    
    
    void Awake()
    {
        instance = this;
        playerAnimator = Register.instance.player.GetComponent<Animator>();
    }

	public void GetAnimation(string name,bool modType)
    {
        playerAnimator.SetBool(name, modType);
    }

    public void GetAnimation(string name, float direction)
    {
        playerAnimator.SetFloat(name, direction);
    }
}
