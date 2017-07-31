 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    SIDESCROLL,
    TOPDOWN
}

public class CameraController : MonoBehaviour
{
    public float timeScaleValueLerping = 0.0f;
    public float timeScaleValueNotLerping = 1.0f;
    private Vector3 lerp;
    private Quaternion slerp;
    public Transform topDownCameraPosition;
    public Transform sideScrollCameraPosition;
    /// <summary>
    /// The lerp speed. Increase to make it faster, decrease to make it slower.
    /// </summary>
    [Range(0.001f, 1.0f)]
    public float lerpSpeed = 1.0f;
    public float lerpDistance = 0.01f;


    // Update is called once per frame
    void Update ()
    {
		if( GameManager.instance.isLerpingCamera)
        { 
            StartCoroutine("LerpCamera");
        }
	}

    IEnumerator LerpCamera()
    {
        Time.timeScale = timeScaleValueLerping;
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                while (Vector3.Distance(transform.position,topDownCameraPosition.position)>= lerpDistance)
                {
                    lerp = Vector3.Lerp(transform.position, topDownCameraPosition.position, lerpSpeed);
                    transform.position = lerp;
                    slerp = Quaternion.Slerp(transform.rotation, topDownCameraPosition.rotation, lerpSpeed);
                    transform.rotation = slerp;
                    yield return null;
                }
                transform.position = topDownCameraPosition.position;
                GameManager.instance.cameraState = State.TOPDOWN;
                break;

            case State.TOPDOWN:
                while (Vector3.Distance(transform.position,sideScrollCameraPosition.position) >= lerpDistance)
                {
                    lerp = Vector3.Lerp(transform.position, sideScrollCameraPosition.position, lerpSpeed);
                    transform.position = lerp;
                    slerp = Quaternion.Slerp(transform.rotation, sideScrollCameraPosition.rotation, lerpSpeed);
                    transform.rotation = slerp;
                    yield return null;
                }
                transform.position = sideScrollCameraPosition.position;
                GameManager.instance.cameraState = State.SIDESCROLL;
                break;
        }
        Time.timeScale = timeScaleValueNotLerping;
        GameManager.instance.isLerpingCamera = false;
    }
}
