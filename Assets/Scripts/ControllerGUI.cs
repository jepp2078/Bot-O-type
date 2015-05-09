using UnityEngine;
using System.Collections;

public class ControllerGUI : MonoBehaviour {

    public GameObject[] objectArray;
    public GameObject camera;
    public GameObject[] basePlates;
    public GameObject robotBase;
    public GameObject table;
    public Material highLightMat;

    private bool openMenu = false;
    private bool baseplateSet = false;
    private ArrayList robotComponents = new ArrayList();
    
    public GameObject currentObject;
    public GameObject highLightedObject;
    private ArrayList oldMat = new ArrayList();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (currentObject != null)
        {
            MoveTheObject();
        }

        /*
	    if(Input.GetKeyDown(KeyCode.Q)){
			openMenu = true;
		}
		if(Input.GetKeyUp(KeyCode.Q)){
			openMenu = false;
		}
        */

        if (Input.GetButtonDown("Fire1") && currentObject == null)
        {
            if(highLightedObject != null){
                Material[] oldMatA = (Material[])oldMat.ToArray(typeof(Material));

                for (int i = 0; i < oldMatA.Length; i++)
                    highLightedObject.GetComponentsInChildren<Renderer>()[i].material = oldMatA[i];
            }
            
            highLightedObject = null;
            highLight();
        }
	}

    void OnGUI()
    {
        if(!baseplateSet){ //runs if theres no baseplate for this player yet, will spawn a new baseplate over the table
            int boxL = 160; //Horizontal lenght of the menu
            GUI.Box(new Rect(10, 10, boxL, 30 + 30 * basePlates.Length), "Spawn Menu");

            for (int i = 0; i < basePlates.Length; i++)
            {
                if (GUI.Button(new Rect(20, 40 + (30 * i), boxL - 20, 20), "" + basePlates[i].name))
                {
                    GUI.Box(new Rect(10, 10, boxL, 30 + 30 * basePlates.Length), "Spawn Menu");
                    GameObject tempBasePlate = (GameObject)PhotonNetwork.Instantiate(basePlates[i].name, table.transform.FindChild("TableSurface").position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, 0);
                    robotComponents.Add(tempBasePlate);
                    baseplateSet = true;
                }
            }
            return;
        }
        //if (openMenu)
        if(true)
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

    void highLight()
    {
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("SpawnCollide");

        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 1f, true);

        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            highLightedObject = hit.rigidbody.gameObject;
            oldMat = new ArrayList();
            foreach(Renderer render in highLightedObject.GetComponentsInChildren<Renderer>()){
                oldMat.Add(render.material);
                render.material = highLightMat;
            }
            //oldMat = highLightedObject.GetComponentsInChildren<Renderer>().material;
            //highLightedObject.GetComponent<Renderer>().material = highLightMat;
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
                //currentObject = (GameObject)Instantiate(rezObject, hit.point, Quaternion.identity);
                Debug.Log(rezObject.name);
                currentObject = (GameObject)PhotonNetwork.Instantiate(rezObject.name, hit.point, Quaternion.identity, 0);

                currentObject.transform.rotation.Set(0, 0, 0, 0);
            }
        }
    }

    void MoveTheObject()
    {
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("SpawnCollide");

        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 1f, true);

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
                Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1f, true);

                Vector3 target = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                if (Input.GetButtonDown("RotateC"))
                {
                    currentObject.transform.Rotate(Vector3.up, 45f);

                }
                else if (Input.GetButtonDown("RotateCCW"))
                {
                    currentObject.transform.Rotate(Vector3.up, -45f);

                }

                //fucking dragons

                //Vector3 offset = currentObject.transform.position - currentObject.GetComponent<Collider>().bounds.;
                //Vector3 offset = currentObject.transform.position - currentObject.GetComponent<Collider>().ClosestPointOnBounds(hit.point);
                //Vector3 offset = new Vector3(0, currentObject.GetComponent<Collider>().bounds.size.y / 2, 0);
                
                /*
                Ray boundRay = new Ray(currentObject.GetComponent<Collider>().bounds.center, 
                    currentObject.GetComponent<Collider>().bounds.ClosestPoint(hit.point) - currentObject.GetComponent<Collider>().bounds.center); //calculates a ray from object to hit though the nearest point of the boundray

                Debug.DrawRay(boundRay.origin, boundRay.direction, Color.magenta, 0.2f, true);
                RaycastHit boundRayHit;
                Vector3 offset = Vector3.zero;

                if (Physics.Raycast(boundRay, out boundRayHit, 1000.0f, layerMask))
                {
                    offset = currentObject.GetComponent<Collider>().bounds.center - boundRayHit.point;
                }
                */
                Vector3 offset = new Vector3(0, currentObject.GetComponent<Collider>().bounds.size.y / 2, 0);
                offset += (currentObject.transform.position - currentObject.GetComponent<Collider>().bounds.center);
                if (Input.GetButton("Shift")) //temp snap thing, not really working
                {
                    //offset = new Vector3(1f - (hit.point.x % 1), currentObject.GetComponent<Collider>().bounds.size.y / 2, 1f - (hit.point.z % 1));
                   
                    Vector3 hitPontNormalized = hit.point.normalized; //the normalized is to check if we're negative in which case invert the 0.05f offset
                    target += new Vector3(0.05f - (hit.point.x % 0.1f), 0, 0.05f - (hit.point.z % 0.1f));

                }

                currentObject.transform.position = target + offset;
            }
        }
    }
}
