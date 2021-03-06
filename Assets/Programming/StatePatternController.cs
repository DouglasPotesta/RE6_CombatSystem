﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternController : MonoBehaviour {

    [Header("Camera")]
    public Camera cam;
    public Transform CamPivot;

    public CameraController camState;

    public CamStats camAim;
    public CamStats camRun;
    public CamStats camGround;
    public CamStats camDefault;
    [HideInInspector]
    public CamStats TransitionStats;
    public string CamStateDebugString = "null";

    public  float yRot;
    public float xRot;



    public LayerMask CameraCollisionLayer;

    [Header("Components")]
    public Collider col;
    public Animator anim;
    public NavMeshAgent navAgent;

    [Header("Movement Parameters")]
    public float directionDampTime = 0.1f;
    public float speedDampTime = 0.1f;


    public enum areaMaskBinaries { EVERYTHING = 1, NOTWALKABLE = 2, JUMP = 4, GROUNDABLE = 8 }
    [Header("NavMesh Limits")]
    public areaMaskBinaries[] areaMask;
    public int areaMaskInt { get { int x = 0; foreach (areaMaskBinaries ms in areaMask) { x += (int)ms; } return x; } }

    [Header("Weapons")]
    public WeaponInventory weapons;

    [Header("Feedback (Read Only)")]
    public bool isRight = true;
    public float direction;
    public bool meleeCool;
    public bool aimCool;
    public float charAngle;
    public float speed;
    [HideInInspector]
    public bool isPivoting = false;
    [HideInInspector]
    public Vector3 charRotate;
    public Vector3 moveDirection;
    public float dampVelocity;
    public Vector3 velocity;
    public float ATTACKRANGE = 1;
    //public NavMeshHit yHolder;




    public class Zombie : MonoBehaviour { }
    [Header("Enemy Interaction")]
    public LayerMask zombieLayerMask;
    public GameObject enemy;

    public ICharacterState currentState;
    public State_Hurt hurtState;
    public State_Casual casualState;
    public State_Grounded groundedState;
    public State_Run runState;
    public State_Combat combatState;
    public State_Interaction interactionState;
    

    void Awake ()
    {
        hurtState = new State_Hurt(this);
        casualState = new State_Casual(this);
        groundedState = new State_Grounded(this);
        combatState = new State_Combat(this);
        interactionState = new State_Interaction(this);
        runState = new State_Run(this);
        camState.TargetCamStats = camDefault;
    }


    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        currentState = combatState;


        StartCoroutine(EnemyCheck());
	}

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    void Update () {
        velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");
        StickToWorldSpace(transform, cam.transform, ref direction, ref speed, ref charAngle, velocity.x, velocity.y, isPivoting);
        //print(currentState);
        currentState.Update();
        SetAnimatorLocomotion();
    }


    /// <summary>
    /// Used for grabbing items and breaking item boxes
    /// </summary>
    /// <param name="other"></param>
    void Grab (GameObject other) // need to implement 
    {
        anim.SetLayerWeight(anim.GetLayerIndex("EnvironmentInteraction"), 1.0f);
        anim.SetFloat("Height", other.transform.position.y - transform.position.y);
        anim.SetTrigger("Grab");
    }
    /// <summary>
    /// Converts the stick movement into a single direction float based on the direction the character is facing relative to the camera. 0 being forward -0.5f being left, 0.5f being right. -1 and 1 are both back. 
    /// </summary>
    /// <param name="root">character's root transform</param>
    /// <param name="camera">camera's transform</param>
    /// <param name="directionOut"></param>
    /// <param name="speedOut"> the out parameter for speed</param>
    /// <param name="angleOut">the out parameter for direction</param>
    /// <param name="directionx">The x axis of the stick</param>
    /// <param name="directiony">The y axis of the stick</param>
    /// <param name="IsPivoting"> Whether the character is pivoting</param>
    public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, float directionx, float directiony, bool IsPivoting)
    {
        Vector3 rootDirection = root.forward;
        Vector3 stickDirection = new Vector3(directionx, 0, directiony);
        Vector3 CameraDirection = new Vector3(camera.forward.x, 0, camera.forward.z).normalized;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
        Vector3 MoveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(MoveDirection, rootDirection);
        //stickDirection = moveDirection;
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), stickDirection, Color.blue, 0.1f);
        float angleRootToMove = Vector3.Angle (rootDirection, MoveDirection) * (axisSign.y >= 0 ? -1f : 1f);
        if (!IsPivoting)
        {
            angleOut = angleRootToMove;
        }
        angleRootToMove /= 180;
        directionOut = angleRootToMove;
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), MoveDirection, Color.red, 0.1f);
    }
    public void OnAnimatorMove()
    {
        currentState.OnAnimatorMove();
    }

    public void OnAnimatorIK()
    {
        // TODO remove this for proper animations
        currentState.OnAnimatorIK(isRight ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand);
    }

    /// <summary>
    /// a basic slerp function for making targets smoothly look at other targets
    /// </summary>
    /// <param name="looker"></param>
    /// <param name="target"></param>
    /// <param name="damping"></param>
    public static void SlerpForMe(Transform looker, Vector3 target, float damping, float deltaTime)
    {
        var rotation = Quaternion.LookRotation(target - looker.position);
        looker.transform.rotation = Quaternion.Slerp(looker.transform.rotation, rotation, deltaTime * damping);
    }
    /// <summary>
    /// a slerp function for making a target look in a direction based only on the x and z position of the target
    /// </summary>
    /// <param name="looker"></param>
    /// <param name="target"></param>
    /// <param name="damping"></param>
    public void LookTowards(Transform looker, Vector3 target, float damping)
    {
        target.y = 0;

        var rotation = Quaternion.LookRotation(target - new Vector3(looker.position.x, 0, looker.position.z));
        looker.transform.rotation = Quaternion.Slerp(looker.transform.rotation, rotation, Time.deltaTime * damping);
    }
    
    public void SetAnimatorLocomotion()
    {
            speed = Mathf.SmoothDamp(speed, velocity.magnitude > 1 ? 1 : velocity.magnitude, ref dampVelocity, 0.1f);
        
        anim.SetFloat("XDirection", Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);
            anim.SetFloat("YDirection", Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);
        if (!isPivoting)
        {
            anim.SetFloat("Speed", speed > 0.1f ? speed : 0, speedDampTime , Time.deltaTime);
            anim.SetFloat("Direction", direction, 0.1f, Time.deltaTime);
        }
    }

    IEnumerator EnemyCheck() // Sets the closest enemy target for melee combat
    {
        RaycastHit[] hits;
        while (true)
        {
             hits = Physics.SphereCastAll(transform.position, 1, Vector3.forward, 1, zombieLayerMask);

            foreach(RaycastHit hit in hits)
            {
                if(enemy != null)
                enemy = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position) < Vector3.Distance(enemy.transform.position, transform.position) ? hit.collider.gameObject: enemy;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    /// <summary>
    /// Checks what input the user is currently doing to adequately reassign bools in the animator and state during transitions
    /// </summary>
    public void InputTransitionCheck()
    {
        // Firing and Melee bools
        if (Input.GetAxis("Aim") > 0.5f)
        {
            anim.SetBool("Aim", true);
            if (Input.GetAxis("Shoot") > 0.5f)
            {
                anim.SetBool("Fire", true);
            } else
            {
                anim.SetBool("Fire", false);
            }
        } else
        {
            anim.SetBool("Aim", false);

            if (Input.GetAxis("Shoot") > 0.5f)
            {
                anim.SetTrigger("Melee");
            }
        }
        if (Input.GetButton("Run"))
        {
            anim.SetBool("Sprint", true);
        }else
        {
            anim.SetBool("Sprint", false);
        }
        if (Input.GetButton("Run"))
        {
            anim.SetBool("Sprint", true);
        }
        else
        {
            anim.SetBool("Sprint", false);
        }
    }
}
