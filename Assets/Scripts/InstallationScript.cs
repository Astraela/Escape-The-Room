using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationScript : MonoBehaviour {
	public GameObject camera;
	public GameObject puzzleManager;
	private bool isPlaced = false;

	void OnTriggerEnter(Collider hit){
		if(hit.gameObject.name == "InstallationObject"){
			GameObject hitObject = hit.gameObject;
			Vector3 rotation = hitObject.transform.localEulerAngles;
			float xDiff = Mathf.Abs (rotation.x - transform.localEulerAngles.x);
			float zDiff = Mathf.Abs (rotation.z - transform.localEulerAngles.z);
			if (xDiff < 20 && zDiff < 20 && camera.GetComponent<ObjectHolding>().IsHolding() && !isPlaced) {
				isPlaced = true;
				puzzleManager.GetComponent<MainPuzzles>().CollectCone();
				camera.GetComponent<ObjectHolding> ().SetObject (camera.GetComponent<ObjectHolding> ().GetHoldingObject ());
				hitObject.tag = "Untagged";
				hitObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				hitObject.transform.position = transform.position;
				hitObject.transform.eulerAngles = transform.eulerAngles;
			}
		}
	}
}
