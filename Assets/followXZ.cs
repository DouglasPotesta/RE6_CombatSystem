using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followXZ : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);


    private void LateUpdate()
    {
        transform.position = new Vector3 (target.position.x, transform.position.y, target.position.z) + offset;
    }
}
