using UnityEngine;
using System.Collections;

public class UIPopup : MonoBehaviour
{
    public GameObject button;

    private GameObject camera;
    private GameObject tempButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void highlighted(GameObject camera)
    {
        this.camera = camera;

        Vector3 tempButtonPos = transform.position + new Vector3(0f, 0.3f, 0f);
        tempButton = (GameObject)Instantiate(button, tempButtonPos, Quaternion.LookRotation(tempButtonPos - camera.transform.position, Vector3.up));
        RotateTowardsObject rotateScript = tempButton.GetComponent<RotateTowardsObject>();
        rotateScript.TargetObject = camera;
        rotateScript.LinkedObject = this.gameObject;
        tempButton.GetComponent<LineRenderer>().SetPosition(0, tempButton.transform.position);
        tempButton.GetComponent<LineRenderer>().SetPosition(1, this.gameObject.transform.position);
    }

    public void unhighlighted()
    {
        Destroy(tempButton);
    }
}
