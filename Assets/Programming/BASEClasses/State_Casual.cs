using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Casual : ICharacterState {

    private float dampVelocity = 0;

    private StatePatternController player;
    public State_Casual(StatePatternController controller)
    {
        player = controller;
    }

    public void Update()
    {

        //player.SetAnimatorLocomotion();
        if (player.aimCool)
        {
            player.LookTowards(player.transform, player.camTarget.position, 10);
        }
        if (Input.GetAxis("Aim")> 0.5f && !player.aimCool)
        {
            player.aimCool = true;
            player.anim.SetBool("Aim", true);
            player.cam.posTarget = player.camAim;
            player.cam.lookTarget = player.camAim.transform.GetChild(0);
            player.cam.dOF.focalTransform = player.cam.lookTarget;
            player.cam.dOF.enabled = true;
            player.cam.dOF.focalSize = 0.4f;
            player.cam.dOF.aperture = 0.507f;
            if (Input.GetAxis("Shoot") < 0.5f)
            {
                //QuickShot
                player.meleeCool = false;
            }

        } else if (Input.GetAxis("Shoot")>0.5f  && !player.meleeCool)
        {
            player.meleeCool = true;
            player.anim.SetBool("Fire", true);

        }
        if (Input.GetAxis("Shoot") < 0.5f)
        {
            player.meleeCool = false;
            player.anim.SetBool("Fire", false);
        }
        if (Input.GetAxis("Aim") < 0.5f)
        {
            player.cam.posTarget = player.camDefault;
            player.cam.lookTarget = player.camDefault.transform.GetChild(0);
            player.cam.dOF.focalTransform = player.cam.lookTarget;
            player.cam.dOF.enabled = false;
            player.aimCool = false;
            player.anim.SetBool("Aim", false);
        }

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

    public void ToGround()
    {
        throw new NotImplementedException();
    }

    public void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 v = (player.anim.deltaPosition) / Time.deltaTime;
            if (player.speed > 0.1f)
            {
                player.transform.Rotate(0, player.direction * 360 * Time.deltaTime, 0);
            }
            // we preserve the existing y part of the current velocity.
            v.y = player.rig.velocity.y;
            player.rig.velocity = v;
        }
    }

    public void ToRun()
    {
        throw new NotImplementedException();
    }
}
