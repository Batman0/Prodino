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
    //private Vector3 lerp;
    //private Quaternion slerp;
    //public Transform topDownCameraPosition;
    //public Transform sideScrollCameraPosition;
    public GameObject cameras;
    public Animation anim_animation;

    [Header("Input")]
    private Player player;
    private const int playerId = 0;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    void LateUpdate ()
    {
        if (/*Input.GetKeyDown(KeyCode.Space)*/player.GetButtonDown("CameraSwitch") && !GameManager.instance.transitionIsRunning/* && player.currentPlayerState == PlayerController.PlayerState.CanMoveAndShoot*/)
        {
            GameManager.instance.transitionIsRunning = true;
            Register.instance.canStartTransitions = true;
            Time.timeScale = timeScaleValueLerping;
            StartCoroutine(LerpCamera());
        }
	}

    IEnumerator LerpCamera()
    {
        //if (Register.instance.numberOfTransitableObjects > 0)
        //{
        //    Register.instance.bulletsCanRotate = true;
        //}
        //Vector3 playerPos = Register.instance.player.transform.position;
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                //while (Vector3.Distance(cameras.transform.position, topDownCameraPosition.position) >= lerpDistance)
                //{
                //    lerp = Vector3.Lerp(cameras.transform.position, topDownCameraPosition.position, lerpSpeed);
                //    cameras.transform.position = lerp;
                //    slerp = Quaternion.Slerp(cameras.transform.rotation, topDownCameraPosition.rotation, lerpSpeed);
                //    cameras.transform.rotation = slerp;
                //    yield return null;
                //}
                //cameras.transform.position = topDownCameraPosition.position;
                //cameras.transform.rotation = topDownCameraPosition.rotation;
                //anim_movement["CameraMovement"].speed = 1.0f;
                //anim_movement["CameraMovement"].normalizedTime = 0.0f;
                anim_animation["CameraMovementSideToTop"].weight = 1;
                anim_animation["CameraMovementSideToTop"].enabled = true;
                while (anim_animation["CameraMovementSideToTop"].normalizedTime < anim_timeRange)
                {
                    anim_animation["CameraMovementSideToTop"].normalizedTime += Time.fixedDeltaTime / transitionDuration;
                    yield return null;
                }
                if (GameManager.instance.currentGameMode != GameMode.TOPDOWN)
                {
                    GameManager.instance.currentGameMode = GameMode.TOPDOWN;
                }
                break;

            case GameMode.TOPDOWN:
                //while (Vector3.Distance(cameras.transform.position, sideScrollCameraPosition.position) >= lerpDistance)
                //{

                //    lerp = Vector3.Lerp(cameras.transform.position, sideScrollCameraPosition.position, lerpSpeed);
                //    cameras.transform.position = lerp;
                //    slerp = Quaternion.Slerp(cameras.transform.rotation, sideScrollCameraPosition.rotation, lerpSpeed);
                //    cameras.transform.rotation = slerp;
                //    yield return null;
                //}
                //cameras.transform.position = sideScrollCameraPosition.position;
                //cameras.transform.rotation = sideScrollCameraPosition.rotation;
                //anim_movement["CameraMovement"].speed = -1.0f;
                //anim_movement["CameraMovement"].normalizedTime = 1.0f;
                anim_animation["CameraMovementTopToSide"].weight = 1;
                anim_animation["CameraMovementTopToSide"].enabled = true;
                while (anim_animation["CameraMovementTopToSide"].time < anim_timeRange)
                {
                    anim_animation["CameraMovementTopToSide"].normalizedTime += Time.fixedDeltaTime / transitionDuration;
                    yield return null;
                }
                if (GameManager.instance.currentGameMode != GameMode.SIDESCROLL)
                {
                    GameManager.instance.currentGameMode = GameMode.SIDESCROLL;
                }
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
}
