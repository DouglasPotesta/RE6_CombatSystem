using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamStats : MonoBehaviour {

    public float dampSpeed = 1000;
    public float MouseSensitivity = 1;
    public float transitionSpeed = 10;
    public float FOV = 60;
    public bool snap = false;
    public Transform lookTarget;
    public float blur;
    public float yMin;
    public float yMax;
    public bool xLocked =false;

    void Start()
    {
        if(lookTarget == null) { transform.GetChild(0); }
    }
    /// <summary>
    /// Pushes the camera out from the wall based on what layermask you pass it
    /// </summary>
    /// <param name="fromObject"></param>
    /// <param name="toTarget"></param>
    /// <param name="playerPosition"></param>
    /// <param name="targetPosition"></param>
    public void CompensateForWalls(Vector3 fromObject, Vector3 toTarget, Vector3 playerPosition, out Vector3 targetPosition, LayerMask physicsMask)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(playerPosition, toTarget, out wallHit, physicsMask))
        {
            //Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            targetPosition = Vector3.Lerp(transform.position, new Vector3(wallHit.point.x + wallHit.normal.x / 10, toTarget.y + wallHit.normal.y / 10, wallHit.point.z + wallHit.normal.z / 10), 0.8f);

        }
        else
        {
            targetPosition = Vector3.Lerp(transform.position, toTarget, 0.8f);
        }
    }
}
