using UnityEngine;
using System.Collections;

public class ControllerGUI : MonoBehaviour {

    public GameObject[] objectArray;
    public GameObject camera;
    public GameObject RobotBase;

    private bool openMenu = false;
    private GameObject currentObject;
    private ArrayList robotComponents = new ArrayList();

	// Use this for initialization
	void Start () {
        robotComponents.Add(RobotBase);
	}
	
	// Update is called once per frame
	void Update () {
        if (currentObject != null)
        {
            MoveTheObject();
        }

	    if(Input.GetKeyDown(KeyCode.Q)){
			openMenu = true;
		}
		if(Input.GetKeyUp(KeyCode.Q)){
			openMenu = false;
		}
	}

    void OnGUI()
    {
        if (openMenu)
        {
            int boxL = 120; //Horizontal lenght of the menu

            // Make a background box
            GUI.Box(new Rect(10, 10, 120, 30 + 30 * objectArray.Length), "Spawn Menu");

            for (int i = 0; i < objectArray.Length; i++)
            {
                if (GUI.Button(new Rect(20, 40 + (30 * i), boxL - 20, 20), "" + objectArray[i].name))
                {
                    Debug.Log("placing" + objectArray[i].name);
                    rezItem(objectArray[i]);
                }
            }
        }

        if (Input.GetButton("Submit"))
        {
            foreach (GameObject com in robotComponents)
            {
                com.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    void rezItem(GameObject rezObject)
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        //Debug.DrawRay (transform.position, transform.forward*rayLenght, Color.green, 10, true);

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 10, true);

            if (hit.collider)
            {
                currentObject = (GameObject)Instantiate(rezObject, hit.point, Quaternion.identity);
                currentObject.transform.rotation.Set(0, 0, 0, 0);
            }
        }
    }

    void MoveTheObject()
    {
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("SpawnCollide");

        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            if (Input.GetButton("Fire1"))
            {
                currentObject.layer = LayerMask.NameToLayer("SpawnCollide");

                //FixedJoint joint = currentObject.AddComponent<FixedJoint>();
                ConfigurableJoint joint = currentObject.AddComponent<ConfigurableJoint>();

                joint.connectedBody = hit.rigidbody;
                
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                joint.projectionMode = JointProjectionMode.PositionAndRotation;
                joint.projectionAngle = 0;
                joint.projectionDistance = 0;

                robotComponents.Add(currentObject);


                currentObject = null;
            }
            else
            {
                Vector3 target = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                //currentObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse ScrollWheel") * 100);

                Vector3 offset = new Vector3(0, currentObject.GetComponent<Collider>().bounds.size.y / 2, 0);
                if (Input.GetButton("Shift"))
                {
                    //offset = new Vector3(1f - (hit.point.x % 1), currentObject.GetComponent<Collider>().bounds.size.y / 2, 1f - (hit.point.z % 1));
                    target += new Vector3(0.5f - (hit.point.x % 1), hit.point.y, 0.5f - (hit.point.z % 1));

                }

                currentObject.transform.position = target + offset;
            }
        }
    }
}
