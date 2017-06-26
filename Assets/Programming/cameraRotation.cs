using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour {

    private float yrot = 0;
    private float xrot = 0;
    public Transform maxZ;
    public float yMax = 80;
    public Transform minZ;
    public float yMin = -80;
    public Vector3 lookPos;
    public float dampSpeed;
    public float horizontalSpeed= 10;
    public float XSENSITIVITY = 1;
    public float YSENSITIVITY = 1;
    public float sensitivityModifier = 1;
    // Use this for initialization
    void Update () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        yrot += Input.GetAxis("CameraX")*Time.deltaTime*20*XSENSITIVITY*sensitivityModifier;
        xrot = Mathf.Clamp(Input.GetAxis("CameraY") * Time.deltaTime * 20 * YSENSITIVITY * sensitivityModifier + xrot, -90, 90);
        lookPos = Vector3.Lerp(maxZ.position, minZ.position, (xrot+90)/180);
        StatePatternController.SlerpForMe(transform, new Vector3(lookPos.x,Mathf.Clamp(lookPos.y, yMin, yMax ), lookPos.z), dampSpeed, Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yrot*horizontalSpeed, transform.eulerAngles.z);
        //transform.Rotate(new Vector3(0, yrot*Time.deltaTime, 0));
        //var rotation = Quaternion.LookRotation(target - looker.position);
        //looker.transform.rotation = Quaternion.Slerp(looker.transform.rotation, rotation, Time.deltaTime * damping);
    
}
}
