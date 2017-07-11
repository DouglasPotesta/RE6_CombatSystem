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

    public void SwitchWeapon()
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

            v.y = player.navAgent.velocity.y;
            v += (player.transform.right * player.velocity.x + player.transform.forward * player.velocity.y) + new Vector3 (v.x, 0, v.z);
            v.y = player.navAgent.velocity.y;
            if (player.speed > 0.1f) // when the player is not pivoting he 
            {
                float y;
                y = player.direction * 360 * Time.deltaTime;
                player.charRotate.y = Input.GetAxis("CameraX") * 180 / 4 * Time.deltaTime;
                player.transform.Rotate(player.charRotate);
                player.navAgent.velocity = v/3;
            } else
            {
                float y;
                y = player.direction * 360 * Time.deltaTime;
                player.charRotate.y = Input.GetAxis("CameraX") * 180 / 2 * Time.deltaTime;
                player.transform.Rotate(player.charRotate);
                player.navAgent.velocity = v;
            }


        }
    }

    public void ToRun()
    {
        throw new NotImplementedException();
    }

    public void OnAnimatorIK(AvatarIKGoal NonDomHand)
    {
        
        player.anim.SetIKPositionWeight(NonDomHand, 1);
        player.anim.SetIKRotationWeight(NonDomHand, 1);
        player.anim.SetIKPosition(NonDomHand, player.weapons.weaponEquiped.NDHandIKTrans.position);
        player.anim.SetIKRotation(NonDomHand, player.weapons.weaponEquiped.NDHandIKTrans.rotation);
    }
}
