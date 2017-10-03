 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    public float timeScaleValueLerping = 0.0f;
    public float timeScaleValueNotLerping = 1.0f;
    private Vector3 lerp;
    private Quaternion slerp;
    public Transform topDownCameraPosition;
    public Transform sideScrollCameraPosition;
    public GameObject cameras;
    /// <summary>
    /// The lerp speed. Increase to make it faster, decrease to make it slower.
    /// </summary>
    [Range(0.001f, 1.0f)]
    public float lerpSpeed = 1.0f;
    public float lerpDistance = 0.01f;


    // Update is called once per frame
    void LateUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.instance.transitionIsRunning && Register.instance.player.canShootAndMove)
        {
            GameManager.instance.transitionIsRunning = true;
            Register.instance.canStartTransitions = true;
            Time.timeScale = timeScaleValueLerping;
            StartCoroutine("LerpCamera");
        }
	}

    IEnumerator LerpCamera()
    {
        //if (Register.instance.numberOfTransitableObjects > 0)
        //{
        //    Register.instance.bulletsCanRotate = true;
        //}
        Vector3 playerPos = Register.instance.player.transform.position;
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                while (Vector3.Distance(cameras.transform.position, topDownCameraPosition.position) >= lerpDistance)
                {
                    lerp = Vector3.Lerp(cameras.transform.position, topDownCameraPosition.position, lerpSpeed);
                    cameras.transform.position = lerp;
                    slerp = Quaternion.Slerp(cameras.transform.rotation, topDownCameraPosition.rotation, lerpSpeed);
                    cameras.transform.rotation = slerp;
                    yield return null;
                }
                cameras.transform.position = topDownCameraPosition.position;
                cameras.transform.rotation = topDownCameraPosition.rotation;
                if (GameManager.instance.currentGameMode != GameMode.TOPDOWN)
                {
                    GameManager.instance.currentGameMode = GameMode.TOPDOWN;
                }
                break;

            case GameMode.TOPDOWN:
                while (Vector3.Distance(cameras.transform.position, sideScrollCameraPosition.position) >= lerpDistance)
                {

                    lerp = Vector3.Lerp(cameras.transform.position, sideScrollCameraPosition.position, lerpSpeed);
                    cameras.transform.position = lerp;
                    slerp = Quaternion.Slerp(cameras.transform.rotation, sideScrollCameraPosition.rotation, lerpSpeed);
                    cameras.transform.rotation = slerp;
                    yield return null;
                }
                cameras.transform.position = sideScrollCameraPosition.position;
                cameras.transform.rotation = sideScrollCameraPosition.rotation;
                if (GameManager.instance.currentGameMode != GameMode.SIDESCROLL)
                {
                    GameManager.instance.currentGameMode = GameMode.SIDESCROLL;
                }
                break;
        }
        Register.instance.canEndTransitions = true;
        Time.timeScale = timeScaleValueNotLerping;
        GameManager.instance.transitionIsRunning = false;
        yield return null;
    }
}
