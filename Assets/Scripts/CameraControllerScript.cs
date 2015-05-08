using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour {

    public float lookSpeed, moveSpeed, zoomSpeed, maxDistance, maxZoom;
    public GameObject camera, cameraPitch, focusObject;

    //temp
    public Vector3 tempCameraPos = Vector3.zero;

    private float rotationX = 0, rotationY = 0;
    private float rotationCenterX = 0, rotationCenterY = 0;
    private float zoomPos = -10, oldZoomPos;

    private Vector3 cameraPos = new Vector3();


	// Use this for initialization
	void Start () {
        zoomPos = camera.transform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        oldZoomPos = camera.transform.localPosition.z;
        //distance = Vector3.Distance(focusObject.transform.position, camera.transform.position);

        float tempZoom = zoomPos + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (-tempZoom > 0 && -tempZoom < maxZoom)
        { 
            zoomPos += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        }

        
        if(Input.GetButton("Fire1")){
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            cameraPitch.transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);
        }

        if (Input.GetButton("Fire2"))
        {
            cameraPos = new Vector3(moveSpeed * Input.GetAxis("Mouse X"), 0, moveSpeed * Input.GetAxis("Mouse Y"));

            tempCameraPos = transform.position + -transform.right * moveSpeed * Input.GetAxis("Mouse X");
            tempCameraPos = tempCameraPos + -transform.forward * moveSpeed * Input.GetAxis("Mouse Y");

            if (tempCameraPos.x <= focusObject.transform.position.x + maxDistance && tempCameraPos.x >= focusObject.transform.position.x - maxDistance)
            {
                transform.position = new Vector3(tempCameraPos.x, transform.position.y, transform.position.z);
            }
            if (tempCameraPos.z <= focusObject.transform.position.z + maxDistance && tempCameraPos.z >= focusObject.transform.position.z - maxDistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, tempCameraPos.z);
            }


        }

        camera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(oldZoomPos,zoomPos,Time.deltaTime*10));
	}
}
