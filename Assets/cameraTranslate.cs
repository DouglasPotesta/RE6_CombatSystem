using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
/// <summary>
/// Controls the main camera's translation movement.
/// </summary>
public class cameraTranslate : MonoBehaviour {

    public CamStats posTarget;
    public Transform lookTarget;
    public Transform player;
    public float dampRotSpeed;
    public float dampPosSpeed;
    public Vector3 TargetPosition;
    public DepthOfField dOF;
    public LayerMask physicsMask;
    void FixedUpdate()
    {
        if (lookTarget == null)
        {
            lookTarget = posTarget.transform.GetChild(0);
        }
        
        CompensateForWalls(transform.position, posTarget.transform, player.position, out  TargetPosition);
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.fixedDeltaTime * dampPosSpeed);
        
    }
    /// <summary>
    /// Pushes the camera out from the wall based on what layermask you pass it
    /// </summary>
    /// <param name="fromObject"></param>
    /// <param name="toTarget"></param>
    /// <param name="playerPosition"></param>
    /// <param name="targetPosition"></param>
    private void CompensateForWalls(Vector3 fromObject, Transform toTarget, Vector3 playerPosition, out Vector3 targetPosition)
    {
        Debug.DrawLine(fromObject, toTarget.position, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(playerPosition, toTarget.position, out wallHit, physicsMask))
        {
            //Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            targetPosition = Vector3.Lerp(transform.position, new Vector3(wallHit.point.x + wallHit.normal.x / 10, toTarget.position.y + wallHit.normal.y / 10, wallHit.point.z + wallHit.normal.z / 10), 0.8f);

        } else
        {
            targetPosition = Vector3.Lerp(transform.position, toTarget.position, 0.8f);
        }
    }

}
