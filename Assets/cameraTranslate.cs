using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
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
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * dampPosSpeed);
        
    }

    private void CompensateForWalls(Vector3 fromObject, Transform toTarget, Vector3 playerPosition, out Vector3 targetPosition)
    {
        Debug.DrawLine(fromObject, toTarget.position, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(playerPosition, toTarget.position, out wallHit, physicsMask))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            targetPosition = new Vector3(wallHit.point.x + wallHit.normal.x / 5, toTarget.position.y + wallHit.normal.y / 5, wallHit.point.z + wallHit.normal.z / 5);

        } else
        {
            targetPosition = toTarget.position;
        }
    }

}
