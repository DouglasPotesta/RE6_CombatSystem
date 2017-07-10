using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Combat : ICharacterState
{


    //private float dampVelocity = 0;
    //private float refRotate;
    private bool isPressed = false;
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
            player.LookTowards(player.transform, player.camState.TargetCamStats.lookTarget.position, 10);
            if (Input.GetAxis("Shoot") > 0.5f && !IsAiming)
            {
                //TODO implement a quickshot
            }
            else if (!IsAiming)//  Initializes aiming
            {
                AimStart();
            } else if(Input.GetAxis("Shoot") > 0.5f)
            {
                player.anim.SetBool("Fire", true);
            } else
            {
                player.anim.SetBool("Fire", false);
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
            if (Input.GetAxis("Shoot") > 0.1f)
            {
                player.anim.SetTrigger("Melee");
            }
        }
        //Debug.Log(Mathf.Abs(Input.GetAxis("DHorizontal")));
        if (Mathf.Abs(Input.GetAxis("DHorizontal")) >0.5f)
        {

            if (!isPressed)
            {
                SwitchWeapon();

                isPressed = true;
            }
        }
        else
        {
            isPressed = false;
        }
    }

    private void AimEnd()
    {
        player.camState.TargetCamStats = player.camDefault;
        /*
        player.cam.posTarget = player.camDefault;
        player.cam.lookTarget = player.camDefault.transform.GetChild(0);
        player.cam.dOF.focalTransform = player.cam.lookTarget;
        player.cam.dOF.enabled = false;
        */
        player.anim.SetBool("Aim", false);
        IsAiming = false;
    }

    private void AimStart()
    {
        
        player.anim.SetBool("Aim", true);
        player.camState.TargetCamStats = player.camAim;
        /*
        player.camState.ToAim();
        player.cam.lookTarget = player.camAim.transform.GetChild(0);
        player.cam.dOF.focalTransform = player.cam.lookTarget;
        player.cam.dOF.enabled = true;
        player.cam.dOF.focalSize = 0.4f;
        player.cam.dOF.aperture = 0.507f;
        */
        IsAiming = true;
    }

    public void SwitchWeapon()
    {
        player.anim.SetLayerWeight(player.anim.GetLayerIndex(player.weapons.weaponEquiped.LayerName), 0);
        player.weapons.Choice += Input.GetAxis("DHorizontal") > 0 ? 1 : -1;
        player.anim.SetLayerWeight(player.anim.GetLayerIndex(player.weapons.weaponEquiped.LayerName), 1);
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
        player.camState.TargetCamStats = player.camGround;
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
           
            v.y = player.navAgent.velocity.y;
            
            player.navAgent.velocity = v;
        }
    }



    public void ToRun()
    {
        player.InputTransitionCheck();
        player.camState.TargetCamStats = player.camRun;
        player.currentState = player.runState;
    }

    public void OnAnimatorIK(AvatarIKGoal NonDomHand)
    {
        player.anim.SetIKPositionWeight(NonDomHand, 1);
        player.anim.SetIKRotationWeight(NonDomHand, 1);
        player.anim.SetIKPosition(NonDomHand, player.weapons.weaponEquiped.NDHandIKTrans.position);
        player.anim.SetIKRotation(NonDomHand, player.weapons.weaponEquiped.NDHandIKTrans.rotation);
    }
}
