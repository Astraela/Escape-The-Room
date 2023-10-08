using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ObjectHolding : MonoBehaviour {

	private GameObject currentObject;
	private GameObject player;
    //private Vector3 rot = new Vector3(0, 20, 0);
	private float playerY = 0;
    private Vector3 rot = new Vector3 (0, 20, 0);
	private Vector3 objectRotation = new Vector3 (0, 0, 0);
	private Vector3 mouseVelocity = new Vector3 (0, 0, 0);
	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject;
	}
    
	// Update is called once per frame
	void Update () {

		if (transform.parent.GetComponent<Inventory> ().GetInventoryState () && currentObject) {
			currentObject.GetComponent<Rigidbody> ().useGravity = true;
            currentObject.GetComponent<Rigidbody>().drag = 0;
            currentObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentObject = null;
		}
		if (currentObject) {
            //currentObject.GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * 2);
            //currentObject.transform.position = Vector3.Lerp (currentObject.transform.position, transform.position + transform.forward * 2, 0.2f);
			float extraY = (playerY + transform.eulerAngles.y) + objectRotation.y;
			//currentObject.transform.eulerAngles = new Vector3 (currentObject.transform.eulerAngles.x, extraY, currentObject.transform.eulerAngles.z);

            currentObject.GetComponent<Rigidbody>().AddForce(((transform.position + transform.forward * 2) - currentObject.transform.position) * 200);

            print(Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                //currentObject.transform.rotation = Quaternion.Slerp(currentObject.transform.rotation, Quaternion.Euler(currentObject.transform.rotation.x, currentObject.transform.rotation.y, currentObject.transform.rotation.z), Time.deltaTime * 10f);

				currentObject.transform.Rotate (rot);

            }else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
				currentObject.transform.Rotate (-rot);
            }
        }
        
		if (Input.GetMouseButton (2) && currentObject) {
			float mouseX = Input.GetAxis ("Mouse X");
			float mouseY = Input.GetAxis ("Mouse Y");

			player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 0;
			player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 0;

			if (mouseX != 0) {
				mouseVelocity += new Vector3 (0, -mouseX * 10, 0);
			}
			if (mouseY != 0) {
				mouseVelocity += new Vector3 (mouseY * 10, 0, 0);
			}
			currentObject.transform.eulerAngles = transform.eulerAngles + mouseVelocity;
		} else if(currentObject) {
			//currentObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x + mouseVelocity.x, transform.eulerAngles.y + mouseVelocity.y, transform.eulerAngles.z);

			//player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 2;
			//player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 2;
		}
	}

	public void SetObject(GameObject obj){
		if (!currentObject) {
			currentObject = obj;
			if (obj) {
				playerY = transform.eulerAngles.y;
				objectRotation = obj.transform.eulerAngles;
				mouseVelocity = new Vector3 (0, 0, 0);
				obj.GetComponent<Rigidbody> ().useGravity = false;
                obj.GetComponent<Rigidbody>().drag = 13;
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            }
		}
		else {
			player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 2;
			player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 2;
			currentObject.GetComponent<Rigidbody> ().useGravity = true;
            currentObject.GetComponent<Rigidbody>().drag = 0;
            currentObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentObject = null;
		}
	}
    public void ThrowObject(GameObject obj)
    {
        if (currentObject)
        {
			player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 2;
			player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 2;
            currentObject.GetComponent<Rigidbody>().useGravity = true;
            currentObject.GetComponent<Rigidbody>().drag = 0;
            currentObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentObject.GetComponent<Rigidbody>().AddForce((transform.forward * 4)*200);
            currentObject = null;
        }
    }
    public bool IsHolding()
    {
        if (currentObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public GameObject GetHoldingObject(){
		return currentObject;
	}
}
