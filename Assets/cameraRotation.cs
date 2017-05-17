using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour {

    private float yrot = 0;
    private float xrot = 0;
    public Transform maxZ;
    public Transform minZ;
    public Vector3 lookPos;
    public float dampSpeed;
    public float horizontalSpeed= 10;
    public float XSENSITIVITY = 1;
    public float YSENSITIVITY = 1;
    // Use this for initialization
    void Update () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        yrot += Input.GetAxis("CameraX")*Time.fixedDeltaTime*20*XSENSITIVITY;
        xrot = Mathf.Clamp(Input.GetAxis("CameraY") * Time.fixedDeltaTime * 20 * YSENSITIVITY + xrot, -90, 90);
        lookPos = Vector3.Lerp(maxZ.position, minZ.position, (xrot+90)/180);
        StatePatternController.SlerpForMe(transform, new Vector3(lookPos.x,lookPos.y, lookPos.z), dampSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yrot*horizontalSpeed, transform.eulerAngles.z);
        //transform.Rotate(new Vector3(0, yrot*Time.deltaTime, 0));
        //var rotation = Quaternion.LookRotation(target - looker.position);
        //looker.transform.rotation = Quaternion.Slerp(looker.transform.rotation, rotation, Time.deltaTime * damping);
    
}
}
