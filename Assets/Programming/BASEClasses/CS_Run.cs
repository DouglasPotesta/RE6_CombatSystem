using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Run : ICameraState
{
    private StatePatternController player;
    private CamStats stats;
    private Vector3 TargetPosition;
    //private Vector3 LookPosition;

    public CS_Run(StatePatternController controller, CamStats stat)
    {
        player = controller;
        stats = stat;
    }

    public void FixedUpdate()
    {
        stats.CompensateForWalls(player.cam.transform.position, stats.transform.position, player.transform.position, out TargetPosition, player.CameraCollisionLayer);
        player.cam.transform.position = Vector3.Lerp(player.cam.transform.position, TargetPosition, Time.fixedDeltaTime * stats.dampSpeed);
        StatePatternController.SlerpForMe(player.cam.transform, stats.lookTarget.position, stats.dampSpeed, Time.fixedDeltaTime);
    }

    public void LateUpdate()
    {
        player.yRot += Input.GetAxis("CameraX") * Time.deltaTime * 20 * GameManager.XSENSITIVITY * GameManager.sensitivityModifier;
        player.xRot = Mathf.Clamp(Input.GetAxis("CameraY") * Time.deltaTime * 20 * GameManager.YSENSITIVITY * GameManager.sensitivityModifier + player.xRot, -90, 90);
        //LookPosition = Vector3.Lerp(, minZ.position, (xRot + 90) / 180);
        //StatePatternController.SlerpForMe(player.CamPivot, new Vector3(LookPosition.x, Mathf.Clamp(LookPosition.y, stats.yMin, stats.yMax), LookPosition.z), stats.dampSpeed, Time.deltaTime);
        player.CamPivot.eulerAngles = new Vector3(Mathf.Clamp(player.xRot * 10, stats.yMin, stats.yMax), player.yRot * 10, player.CamPivot.eulerAngles.z);

    }

    public void Update()
    {
        //throw new NotImplementedException();
    }

    public void ToAim()
    {
        player.StartCoroutine(player.CamTransition(stats, player.camAim, player.CAimState, player.cam));
    }

    public void ToBasic()
    {
        player.StartCoroutine(player.CamTransition(stats, player.camDefault, player.CBasicState, player.cam));
    }

    public void ToGround()
    {
        player.StartCoroutine(player.CamTransition(stats, player.camGround, player.CGroundState, player.cam));
    }

    public void ToRun()
    {
        player.StartCoroutine(player.CamTransition(stats, player.camRun, player.CRunState, player.cam));
    }

    public CamStats GetStats()
    {
        return stats;
    }
}
