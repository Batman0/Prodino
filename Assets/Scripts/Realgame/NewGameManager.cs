using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SIDESCROLL,
    TOPDOWN
}
public class NewGameManager : MonoBehaviour
{
    public static NewGameManager instance;
    public GameMode currentGameMode;

    private void Awake()
    {
        instance = this;
    }
}
