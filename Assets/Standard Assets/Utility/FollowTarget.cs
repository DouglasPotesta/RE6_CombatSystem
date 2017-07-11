using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public bool smoothYValue = false;

        public float smoothTime = 1;

        private float velocity;

        private void LateUpdate()
        {
            if (smoothYValue)
            {
                transform.position = new Vector3(
                    target.position.x + offset.x, 
                    Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref velocity, smoothTime), 
                    target.position.z + offset.z);
            } else
            {
                transform.position = target.position + offset;
            }
            
        }
    }
}
