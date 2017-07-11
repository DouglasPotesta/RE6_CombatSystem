using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    private Camera cam;
    public Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
            }
            return cam;
        }
    }

    public Transform CamPivot;

    public DepthOfField dOF;

    public LayerMask wallMask;

    public Transform playerFocalSpot;

    public CamStats TargetCamStats
    {
        get { return targetCamStats; }

        set
        {
            timeInTransition = 0;
            targetCamStats = value;
        }
    }

    private CamStats targetCamStats;

    /// <summary>
    /// the Camera's desired position "Optimal"
    /// </summary>
    public Vector3 targetPosition;

    /// <summary>
    /// The Safe Position;
    /// </summary>
    public Vector3 wallPosition;

    public float FOV = 60;
    public bool snap = false;
    public Vector3 lookTargetPosition;
    public float blur;

    public float yMin;
    public float yMax;
    public bool xLocked = false;

    public float CameraSlerpDamp = 0.1f;
    public float xRot = 0;
    public float yRot = 0;

    public float timeInTransition = 0;

    public float transitionTimeFrame { get { return timeInTransition / targetCamStats.transitionLength; } }

    /// <summary>
    /// Returns a safe position so the fromObject can always see the playerPosition
    /// </summary>
    /// <param name="fromObject">The camera's current position</param>
    /// <param name="toTarget">the camera's main target objects position</param>
    /// <param name="playerPosition">Where the player is </param>
    public Vector3 CompensateForWalls(Vector3 fromObject, Vector3 toTarget, Vector3 playerPosition, LayerMask physicsMask)
    {
        // TODO Keep balancing snappiness of this, 
        // TODO add raycast for accounting for player heading, so that the camera can push relative to the direction
        // example: player is running around a spiralling stair case leaning left, the camera will raycast with an offset to the left, 
        // checking to see if it can see to the players left, and push out towards the right if it can't. This behaviour is lower to priority
        // than the main line of sight raycast
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(playerPosition, toTarget, out wallHit, physicsMask))
        {
            //Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            //Debug.Log("Compensating for: " + wallHit.collider.gameObject.name);
            return Vector3.Lerp(transform.position, new Vector3(wallHit.point.x + wallHit.normal.x / 10, toTarget.y + wallHit.normal.y / 10, wallHit.point.z + wallHit.normal.z / 10), 0.8f);

        }
        else
        {
            return Vector3.Lerp(transform.position, toTarget, 0.8f);
        }
    }


    /// <summary>
    /// Returns an offset to shake the camera by
    /// </summary>
    /// <param name="timeFrame0To1"></param>
    /// <returns></returns>
    public Vector3 Shake(float timeFrame0To1)
    {
        float shakeValue = targetCamStats.cameraShake.Evaluate(timeFrame0To1);
        return new Vector3(Random.Range(shakeValue, -shakeValue), Random.Range(shakeValue, -shakeValue), 0)/10;
    }

    public void CameraTranslation(float deltaTime)
    {

        targetPosition = Vector3.Lerp(targetPosition, TargetCamStats.transform.position, transitionTimeFrame);

        wallPosition = CompensateForWalls(Cam.transform.position, targetPosition, playerFocalSpot.position, wallMask);

        Cam.transform.position = wallPosition +
            (targetCamStats.shouldCameraShake ? Shake(transitionTimeFrame) : Vector3.zero);

        lookTargetPosition = Vector3.Lerp(lookTargetPosition, targetCamStats.lookTarget.position, transitionTimeFrame);

    }
    // TODO implement locking for xLocked running the actual character rotation should be handled in the statepatterncontroller, but the camera pivot orienting itself to the player should be done here.
    public void CameraSpecials()
    {
        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FOV, transitionTimeFrame);

        DepthOfFieldLerping();
    }

    public void DepthOfFieldLerping()
    {
        if (!dOF.enabled)
        {
            if (!targetCamStats.isDepthOfField)
            {
                return;
            }else
            {
                dOF.enabled = true;
                dOF.focalTransform = targetCamStats.lookTarget;
                dOF.aperture = 0;
                dOF.focalSize = 0;
            }
        } else if (!targetCamStats.isDepthOfField)
        {
            dOF.aperture = Mathf.Lerp(dOF.aperture, 0, transitionTimeFrame);
            dOF.focalSize = Mathf.Lerp(dOF.focalSize, 0, transitionTimeFrame);
            dOF.enabled = transitionTimeFrame < 0.999f;
        } else
        {
            dOF.aperture = Mathf.Lerp(dOF.aperture, targetCamStats.aperture, transitionTimeFrame);
            dOF.focalSize = Mathf.Lerp(dOF.focalSize, targetCamStats.focalSize, transitionTimeFrame);
        }
    }

    float yRotVelocity = 0;
    public void CameraPivotRotation(float deltaTime)
    {
        xRot = Mathf.Clamp(Input.GetAxis("CameraY") *
                deltaTime *
                20 *
                GameManager.YSENSITIVITY *
                GameManager.sensitivityModifier +
                xRot,
                yMin, yMax);
        if (targetCamStats.xLocked)
        {
            yRot = Mathf.SmoothDampAngle(yRot, playerFocalSpot.eulerAngles.y, ref yRotVelocity, targetCamStats.transitionLength);
        }
        else
        {
            yRot += Input.GetAxis("CameraX") *
                deltaTime *
                20 *
                GameManager.XSENSITIVITY *
                GameManager.sensitivityModifier;
        }

        CamPivot.eulerAngles = new Vector3(xRot, yRot, CamPivot.eulerAngles.z);

        yMax = Mathf.Lerp(yMax, targetCamStats.yMax, transitionTimeFrame);
        yMin = Mathf.Lerp(yMin, targetCamStats.yMin, transitionTimeFrame);

        GameManager.sensitivityModifier = Mathf.Lerp(
            GameManager.sensitivityModifier, 
            targetCamStats.MouseSensitivity, 
            transitionTimeFrame
            );
    }


    public void Start()
    {
        targetPosition = TargetCamStats.transform.position;
    }

    public void LateUpdate()
    {
        CameraSpecials();
        CameraPivotRotation(Time.deltaTime);
        Cam.transform.LookAt(lookTargetPosition);
    }

    public void Update()
    {
        timeInTransition = Mathf.Clamp01(timeInTransition + Time.deltaTime);
        CameraTranslation(Time.deltaTime);
    }

    public void FixedUpdate()
    {

        
        
    }

}
