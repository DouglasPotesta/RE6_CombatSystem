using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatternController : MonoBehaviour {

    public cameraTranslate cam;
    public Transform camAim;
    public Transform camRun;
    public Transform camGround;
    public Transform camDefault;
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



    public float directionDampTime = 0.1f;

    public ICharacterState currentState;
    public State_Hurt hurtState;
    public State_Casual casualState;
    public State_Grounded groundedState;
    public State_Combat combatState;
    public State_Interaction interactionState;
    

    void Awake ()
    {
        hurtState = new State_Hurt(this);
        casualState = new State_Casual(this);
        groundedState = new State_Grounded(this);
        combatState = new State_Combat(this);
        interactionState = new State_Interaction(this);
    }


    void Start () {
        rig = GetComponent<Rigidbody>();
        currentState = combatState;
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
        currentState.Update();
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
        SetAnimatorLocomotion();
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
            anim.SetFloat("Direction", direction, 0.1f, Time.deltaTime);
            anim.SetFloat("XDirection", Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);
            anim.SetFloat("YDirection", Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);
        if (!isPivoting)
        {
            anim.SetFloat("Speed", velocity.magnitude > 1 ? 1 : velocity.magnitude, 0.1f, Time.deltaTime);
        }
    }
}
