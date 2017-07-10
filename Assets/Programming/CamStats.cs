using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamStats : MonoBehaviour {

    /// <summary>
    /// How fast the target position will move to the Cam stats position
    /// </summary>
    public float transitionLength = 0.5f;


    public float MouseSensitivity = 1;
    //public float transitionSpeed = 10;
    public float FOV = 60;
    public Transform lookTarget;
    public bool isDepthOfField = false;
    public float focalSize = 0;

    public float aperture = 0;



    /// <summary>
    /// Used during transitioning for effect
    /// </summary>
    public bool shouldCameraShake = false;
    public AnimationCurve cameraShake;
    public float yMin;
    public float yMax;
    public bool xLocked =false;

    void Start()
    {
        if(lookTarget == null) { transform.GetChild(0); }
    }



}
