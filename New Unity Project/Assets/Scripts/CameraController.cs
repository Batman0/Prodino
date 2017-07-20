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
    private Vector3 lerp;
    public Transform TopDownCameraPosition;
    public Transform SideScrollCameraPosition;
    public float lerpSpeed = 1.0f;
    public float lerpDistance = 0.01f;
    private bool isLerpingCamera = false;
    public CameraState myState;

    void Start()
    {
        myState = CameraState.SIDESCROLL;
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space) && !isLerpingCamera)
        {
            isLerpingCamera = true;
            StartCoroutine("LerpCamera");
        }
	}

    IEnumerator LerpCamera()
    {
        switch(myState)
        {
            case CameraState.SIDESCROLL:
                while(Vector3.Distance(transform.position,TopDownCameraPosition.position)>= lerpDistance)
                {
                    lerp = Vector3.Lerp(transform.position, TopDownCameraPosition.position, lerpSpeed);
                    transform.position = lerp;
                    yield return null;
                }
                break;
            case CameraState.TOPDOWN:
                break;
        }

    }
}
