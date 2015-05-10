using UnityEngine;
using System.Collections;

public class RotateTowardsObject : MonoBehaviour {

    public GameObject TargetObject;
    public GameObject LinkedObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(this.transform.position - TargetObject.transform.position);
        transform.position = LinkedObject.transform.position + new Vector3(0f, 0.3f, 0f);
        GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        GetComponent<LineRenderer>().SetPosition(1, LinkedObject.transform.position);
	}
}
