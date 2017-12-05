 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CamerasController : MonoBehaviour
{
    public const float timeScaleValueLerping = 0.0f;
    public const float timeScaleValueNotLerping = 1.0f;
    public const float anim_timeRange = 0.95f;
    public float transitionDuration;
    public GameObject cameras;
    public Animation anim_animation;
    private GameMode currentMode;
    
    void Update()
    {
        SetCurrentMode(GameManager.instance.currentGameMode);
    }

    IEnumerator LerpCamera()
    {
        switch (currentMode)
        {
            case GameMode.SIDESCROLL:
                anim_animation["CameraMovementSideToTop"].weight = 1;
                anim_animation["CameraMovementSideToTop"].enabled = true;
                while (anim_animation["CameraMovementSideToTop"].normalizedTime < anim_timeRange)
                {
                    anim_animation["CameraMovementSideToTop"].normalizedTime += Time.fixedDeltaTime / transitionDuration;
                    yield return null;
                }
                //if (GameManager.instance.currentGameMode != GameMode.TOPDOWN)
                //{
                //    GameManager.instance.currentGameMode = GameMode.TOPDOWN;
                //}
                break;

            case GameMode.TOPDOWN:
                anim_animation["CameraMovementTopToSide"].weight = 1;
                anim_animation["CameraMovementTopToSide"].enabled = true;
                while (anim_animation["CameraMovementTopToSide"].time < anim_timeRange)
                {
                    anim_animation["CameraMovementTopToSide"].normalizedTime += Time.fixedDeltaTime / transitionDuration;
                    yield return null;
                }
                //if (GameManager.instance.currentGameMode != GameMode.SIDESCROLL)
                //{
                //    GameManager.instance.currentGameMode = GameMode.SIDESCROLL;
                //}
                break;
        }
        EndTransition();
        yield return null;
    }

    public void EndTransition()
    {
        Register.instance.canEndTransitions = true;
        Time.timeScale = timeScaleValueNotLerping;
        GameManager.instance.transitionIsRunning = false;
    }

    public void StartTransition()
    {

        if (!GameManager.instance.transitionIsRunning)
        {
            GameManager.instance.transitionIsRunning = true;
            Register.instance.canStartTransitions = true;
            Time.timeScale = timeScaleValueLerping;
            StartCoroutine(LerpCamera());
        }
    }

    public void SetCurrentMode(GameMode _currentMode)
    {
        currentMode = _currentMode;
    }
}
