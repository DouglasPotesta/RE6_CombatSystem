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
                player.charRotate.y = y/4;
                player.transform.Rotate(player.charRotate);
            }
            // we preserve the existing y part of the current velocity.

            v.y = player.rig.velocity.y;

            player.rig.velocity = v;
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

        player.anim.SetBool("Sprint", false);
        player.currentState = player.combatState;
    }

    public void ToGround()
    {
        player.InputTransitionCheck();
        player.StartCoroutine(player.CamTransition(player.camGround, player.cam));
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
}
