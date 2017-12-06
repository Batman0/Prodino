using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager: MonoBehaviour 
{
    public delegate void GameModeState(GameMode currentState);
    public static event GameModeState changeState;
    

    public static void OnChangeGameMode(GameMode currentState)
    {
        if (changeState != null)
        {
            changeState(currentState);
        }
    }
           
}
