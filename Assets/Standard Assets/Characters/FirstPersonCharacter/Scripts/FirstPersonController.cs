using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        public Transform cam;

        
        public Animator anim;
        public Rigidbody rig;
        public float direction;
        public bool meleeCool;
        public bool jump;
        public float charAngle;
        public float speed;
        [Range(0,10)]
        public float rootSpeed;
        public float dampVelocity;
        private Vector3 velref;
        public Vector2 input;
        public Vector3 velocity;
        public Vector3 rootOffsetAMount;
        public Vector3 jumpdir;
        public LayerMask physmask;


        public float directionDampTime = 0.1f;

        // Use this for initialization
        private void Start()
        {

        }


        // Update is called once per frame
        private void Update()
        {
            velocity.x = Input.GetAxis("Horizontal");
            velocity.y = Input.GetAxis("Vertical");
            //if (velocity.sqrMagnitude > 0.1)
            StickToWorldSpace(transform, cam, ref direction, ref speed, ref charAngle, velocity.x, velocity.y);
            SetAnimatorLocomotion();
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down, Color.red, 1);
            if (jump)
            {

                jumpdir.y = Mathf.Max(-7,jumpdir.y + (jumpdir.y * 4 * Time.deltaTime));
                rig.velocity = Vector3.SmoothDamp(rig.velocity, jumpdir + rootSpeed * 4 * Vector3.up, ref velref, 0.05f);
                print(rig.velocity);
                if (Physics.Raycast(new Ray(transform.position, transform.up*-1), 0.2f, physmask) && rig.velocity.y < 0)
                {
                    anim.SetBool("Jump", false);

                }
            }
            else
            {
                
                if (speed > 0.1f && !meleeCool)
                {
                    transform.Rotate(0, direction * 360 * Time.deltaTime, 0);
                }
                if (Input.GetButtonDown("Fire1") && !meleeCool)
                {
                    meleeCool = true;
                    anim.SetTrigger("Attack1");
                }
                if (Input.GetButtonDown("Fire2") && !meleeCool)
                {
                    meleeCool = true;
                    anim.SetTrigger("Attack2");
                }
                if (!meleeCool && !jump)
                {
                    rig.velocity = Vector3.SmoothDamp(rig.velocity, transform.forward * rootSpeed + (Vector3.up*rig.velocity.y), ref velref, dampVelocity);
                }
                Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down, Color.red, 1);
                if (!meleeCool && Input.GetButtonDown("Jump") && Physics.Raycast(transform.position + Vector3.up*0.1f, Vector3.down, 0.5f, physmask))
                {
                    print("yop");
                    anim.SetBool("Jump", true);
                    jumpdir = rig.velocity;
                    jumpdir.y = -0.1f;
                }
                if (!meleeCool && Input.GetButtonDown("Fire3"))
                {
                    print("oid");
                    anim.SetTrigger("Dead");
                }
            }
        }



        private void FixedUpdate()
        {
            
        }


        public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, float directionx, float directiony)
        {
            Vector3 rootDirection = root.forward;
            Vector3 stickDirection = new Vector3(directionx, 0, directiony);

            Vector3 CameraDirection = new Vector3(camera.forward.x, 0, camera.forward.z).normalized;

            Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

            Vector3 moveDirection = referentialShift * stickDirection;
            Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
            //stickDirection = moveDirection;
            Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), stickDirection, Color.blue, 0.1f);

            float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
            angleOut = angleRootToMove;

            angleRootToMove /= 180;
            directionOut = angleRootToMove;
            Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.red, 0.1f);


        }
        public void SetAnimatorLocomotion()
        {
            speed = Mathf.SmoothDamp(speed, velocity.magnitude > 1 ? 1 : velocity.magnitude, ref dampVelocity, 0.1f);
            
            anim.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }

        
    }
}
