using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternController : MonoBehaviour {

    public cameraTranslate cam;
    public CamStats camAim;
    public CamStats camRun;
    public CamStats camGround;
    public CamStats camDefault;
    public Collider col;
    public Animator anim;
    public Rigidbody rig;
    public Transform camTarget;

    public WeaponInventory weapons;

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
    public NavMeshHit yHolder;
    public enum areaMaskBinaries { EVERYTHING = 1, NOTWALKABLE = 2, JUMP = 4, GROUNDABLE = 8 }


    public areaMaskBinaries[] areaMask;
    public int areaMaskInt { get { int x =0;  foreach (areaMaskBinaries ms in areaMask) { x += (int)ms; } return x; }}

    public float directionDampTime = 0.1f;

    public float speedDampTime = 0.1f;

    public class Zombie : MonoBehaviour { }
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
    }


    void Start () {
        rig = GetComponent<Rigidbody>();
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

    // Update is called once per frame
    void Update () {
        velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");
        //if (velocity.sqrMagnitude > 0.1)
            StickToWorldSpace(transform, cam.transform, ref direction, ref speed, ref charAngle, ref moveDirection, velocity.x, velocity.y, isPivoting);
        print(currentState);
        currentState.Update();
        SetAnimatorLocomotion();
    }

    void Grab (GameObject other)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("EnvironmentInteraction"), 1.0f);
        anim.SetFloat("Height", other.transform.position.y - transform.position.y);
        anim.SetTrigger("Grab");
    }

    public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, ref Vector3 MoveDirection, float directionx, float directiony, bool IsPivoting)
    {
        Vector3 rootDirection = root.forward;
        Vector3 stickDirection = new Vector3(directionx, 0, directiony);
        Vector3 CameraDirection = new Vector3(camera.forward.x, 0, camera.forward.z).normalized;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
        MoveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
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
    public void FixedUpdate()
    {
        NavMesh.SamplePosition(transform.position, out yHolder, 0.5f, areaMaskInt);
        transform.position = yHolder.position - Vector3.up*0.03f;//Vector3.Lerp(transform.position, yHolder.position, 0.01f); // new Vector3(transform.position.x, yHolder.position.y, transform.position.z);

    }

    public IEnumerator CamTransition(CamStats camStat, cameraTranslate cam) // remove this for a callable function in update
    {
        cam.dampPosSpeed = camStat.transitionSpeed;
        cam.posTarget = camStat;
        cam.lookTarget = camStat.transform.GetChild(0);
        cam.dampRotSpeed = 10; // TODO fix this so that way the rotation of the camera is consistent throughout the transition
        float x = 0;
        while (cam.dampPosSpeed < camStat.dampSpeed)
        {
            x += Time.deltaTime*1.5f;
            cam.dampPosSpeed = Mathf.Lerp(camStat.transitionSpeed, camStat.dampSpeed, 1);
            cam.dampRotSpeed = Mathf.Lerp(cam.dampRotSpeed, camStat.dampRot, x);
            yield return new WaitForEndOfFrame();
        }
        cam.dampPosSpeed = camStat.dampSpeed;
        cam.dampRotSpeed = camStat.dampRot;
    }

    public static void SlerpForMe(Transform looker, Vector3 target, float damping)
    {
        var rotation = Quaternion.LookRotation(target - looker.position);
        looker.transform.rotation = Quaternion.Slerp(looker.transform.rotation, rotation, Time.deltaTime * damping);
    }
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
