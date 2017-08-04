 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    SIDESCROLL,
    TOPDOWN
}

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
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.instance.cameraTransitionIsRunning)
        {
            GameManager.instance.cameraTransitionIsRunning = true;
            Register.instance.canStartEnemyTransition = true;
            Time.timeScale = timeScaleValueLerping;
            StartCoroutine("LerpCamera");
        }
	}

    IEnumerator LerpCamera()
    {
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
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
                if (GameManager.instance.cameraState != State.TOPDOWN)
                {
                    GameManager.instance.cameraState = State.TOPDOWN;
                }
                break;

            case State.TOPDOWN:
                while (Vector3.Distance(cameras.transform.position, sideScrollCameraPosition.position) >= lerpDistance)
                {
                    Debug.Log("Sono una mignotta");
                    lerp = Vector3.Lerp(cameras.transform.position, sideScrollCameraPosition.position, lerpSpeed);
                    cameras.transform.position = lerp;
                    slerp = Quaternion.Slerp(cameras.transform.rotation, sideScrollCameraPosition.rotation, lerpSpeed);
                    cameras.transform.rotation = slerp;
                    yield return null;
                }
                cameras.transform.position = sideScrollCameraPosition.position;
                cameras.transform.rotation = sideScrollCameraPosition.rotation;
                if (GameManager.instance.cameraState != State.SIDESCROLL)
                {
                    GameManager.instance.cameraState = State.SIDESCROLL;
                }
                break;
        }
        Register.instance.canEndEnemyTransition = true;
        Time.timeScale = timeScaleValueNotLerping;
        GameManager.instance.cameraTransitionIsRunning = false;
        yield return null;
    }
}
