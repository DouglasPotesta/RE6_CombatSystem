using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        private Vector3 currentVel;
        public float smoothTime = 0.1f;

        private void FixedUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVel, smoothTime) ;
        }
    }
}
