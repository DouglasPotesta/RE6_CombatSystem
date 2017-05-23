using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grounded : ICharacterState
{

    private StatePatternController player;

    public State_Grounded(StatePatternController controller)
    {
        player = controller;
    }

    public void Update()
    {
        //player.SetAnimatorLocomotion();
        if (Input.GetAxis("Aim") < 0.5f || (Input.GetButtonDown("Run") && player.anim.GetFloat("YDirection") > 0.5f))
        {
            ToCombat();
        }
        if (Input.GetButtonDown("Run"))
        {
            player.anim.SetBool("Sprint", true);
        }
        if (Input.GetButtonUp("Run"))
        {
            player.anim.SetBool("Sprint", false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }

    public void OnTriggerStay(Collider other)
    {
        throw new NotImplementedException();
    }

    public void SwitchWeapon(WeaponBehaviour weapon)
    {
        throw new NotImplementedException();
    }

    public void ToCasual()
    {
        throw new NotImplementedException();
    }

    public void ToCombat()
    {
        player.InputTransitionCheck();
        player.currentState = player.combatState;
    }

    public void ToGround()
    {
        throw new NotImplementedException();
    }

    public void ToHurt()
    {
        throw new NotImplementedException();
    }

    public void ToInteraction()
    {
        throw new NotImplementedException();
    }

    public void ToQuickTime()
    {
        throw new NotImplementedException();
    }

    public void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 v = (player.anim.deltaPosition) / Time.deltaTime;
                Vector3 camStraight = player.cam.transform.forward;
                camStraight.y = 0;
            float x = Vector3.Cross(player.transform.forward, camStraight).y > 0? 1:-1 ;
            // TODO implement look sensitivity changs depending on the state using the script animation layer
            player.transform.Rotate (new Vector3(0,(Vector3.Angle(player.transform.forward, camStraight)*Time.deltaTime*x),0)) ;
            // we preserve the existing y part of the current velocity.
            v.y = player.rig.velocity.y;
            v += (player.transform.right * player.velocity.x + player.transform.forward*player.velocity.y);
            v.y = player.rig.velocity.y;
            player.rig.velocity = v;
        }
    }

    public void ToRun()
    {
        throw new NotImplementedException();
    }
}
