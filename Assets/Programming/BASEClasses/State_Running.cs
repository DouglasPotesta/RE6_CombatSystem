using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Run : ICharacterState
{
    private StatePatternController player;

    public State_Run(StatePatternController controller)
    {
        player = controller;
    }

    public void Update()
    {
        //TODO implement camera movement sliding mechanic and slowing to a stop

        if (Input.GetButtonUp("Run"))
        {
            ToCombat();
        }
        if (Input.GetAxis("Shoot") > 0.5f)
        {
            player.anim.SetTrigger("Melee");
        }
        if (Input.GetAxis("Aim") > 0.5f)
        {
            player.anim.SetBool("Aim", true);
            ToGround();
        }
        if (player.speed < 0.1f)
        {

            player.anim.SetFloat("Speed", 0);
            ToCombat();
        }
    }

    public void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 v = (player.anim.deltaPosition) / Time.deltaTime;
            if (player.speed > 0.1f) // when the player is not pivoting he 
            {
                float y;
                    y = player.direction * 360 * Time.deltaTime;
                player.charRotate.y = y / 4;
                player.transform.Rotate(player.charRotate);
            }
            // we preserve the existing y part of the current velocity.

            v.y = player.navAgent.velocity.y;

            player.navAgent.velocity = v;
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
        player.camState.ToBasic();
        player.anim.SetBool("Sprint", false);
        player.currentState = player.combatState;
    }

    public void ToGround()
    {
        player.InputTransitionCheck();
        player.camState.ToGround();
        player.currentState = player.groundedState;
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
