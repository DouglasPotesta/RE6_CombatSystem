using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Combat : ICharacterState
{


    private float dampVelocity = 0;
    private float refRotate;
    private StatePatternController player;
    private bool IsAiming = false;
    public State_Combat(StatePatternController controller)
    {
        player = controller;
    }

    public void Update()
    {

        //player.SetAnimatorLocomotion();
        if (Input.GetButton("Run"))
        {
            ToRun();
        }
        else
        {

        }
        if (Input.GetAxis ("Aim") > 0.5f) // If player is trying to aim
        {
            player.LookTowards(player.transform, player.camTarget.position, 10);
            if (Input.GetAxis("Shoot") > 0.5f && !IsAiming)
            {
                //TODO implement a quickshot
            }
            else if (!IsAiming)//  Initializes aiming
            {
                AimStart();
            }

            if (Input.GetButtonDown("Run") && player.speed > 0.8f) // Diving in a direction
            {
                ToGround();
            }
        } else // when the player is not trying to aim
        {
            if (IsAiming)
            {
                AimEnd();
            }
            if (Input.GetAxis("Shoot") > 0.5f)
            {
                player.anim.SetTrigger("Melee");
            }
        }
    }

    private void AimEnd()
    {
        player.StartCoroutine(player.CamTransition(player.camDefault, player.cam));
        player.cam.posTarget = player.camDefault;
        player.cam.lookTarget = player.camDefault.transform.GetChild(0);
        player.cam.dOF.focalTransform = player.cam.lookTarget;
        player.cam.dOF.enabled = false;
        player.anim.SetBool("Aim", false);
        IsAiming = false;
    }

    private void AimStart()
    {
        player.anim.SetBool("Aim", true);
        player.StartCoroutine(player.CamTransition(player.camAim, player.cam));
        player.cam.lookTarget = player.camAim.transform.GetChild(0);
        player.cam.dOF.focalTransform = player.cam.lookTarget;
        player.cam.dOF.enabled = true;
        player.cam.dOF.focalSize = 0.4f;
        player.cam.dOF.aperture = 0.507f;
        IsAiming = true;
    }

    public void SwitchWeapon(WeaponBehaviour weapon)
    {
        player.weapons.choice += Input.GetAxis("DHorizontal") > 0 ? 1 : -1;
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Reload"))
        {
            //TODO IntegratePickupObject not here, but in the actual statePatternController script
        }
    }

    public void ToCasual()
    {
        throw new NotImplementedException();
    }

    public void ToCombat()
    {
        throw new NotImplementedException();
    }

    public void ToGround()
    {
        player.InputTransitionCheck();
        player.StartCoroutine(player.CamTransition(player.camGround, player.cam));
        player.currentState = player.groundedState;
        //player.StartCoroutine(TransitionTo()); Coroutine implementation 
    }

    IEnumerator TransitionTo()
    {
        while(true){

            yield return new WaitForSeconds(1);
        }
        
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
            if (player.speed > 0.1f && !player.isPivoting)
            {
                float y;
                if (player.isPivoting)
                {
                    y = player.anim.GetFloat("Direction") * 360 * Time.deltaTime;
                }
                else
                {
                    y = IsAiming ?
                        Mathf.Atan2(player.velocity.y, player.velocity.x) * Mathf.Rad2Deg * Time.deltaTime :
                    player.direction * 360 * Time.deltaTime;
                }
            
                player.charRotate.y = y;
                player.transform.Rotate(player.charRotate);
            }
            // we preserve the existing y part of the current velocity.
           
            v.y = player.rig.velocity.y;
            
            player.rig.velocity = v;
        }
    }

    public void ToRun()
    {
        player.InputTransitionCheck();
        player.currentState = player.runState;
    }
}
