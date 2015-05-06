using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour {

    public float lookSpeed, moveSpeed, zoomSpeed;
    public GameObject camera, cameraPitch;

    private float rotationX = 0, rotationY = 0;
    private float rotationCenterX = 0, rotationCenterY = 0;
    private float zoomPos = -10;
    private Vector3 cameraPos = new Vector3();


	// Use this for initialization
	void Start () {
        zoomPos = camera.transform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        zoomPos += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        
        if(Input.GetButton("Fire1")){
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            cameraPitch.transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);
        }

        if (Input.GetButton("Fire2")){
            cameraPos = new Vector3(moveSpeed * Input.GetAxis("Mouse X"), 0, moveSpeed * Input.GetAxis("Mouse Y"));

            transform.position += -transform.forward * moveSpeed * Input.GetAxis("Mouse Y");
            transform.position += -transform.right * moveSpeed * Input.GetAxis("Mouse X");
        }

        camera.transform.localPosition = new Vector3(0, 0, zoomPos);
	}
}
