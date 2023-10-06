using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour {

	private int LockCount = 5;

	public void HandleInteraction(GameObject plr, GameObject currentLock){
		GameObject holding = plr.GetComponent<Inventory>().GetHoldingObject();
		if (holding && holding.CompareTag("Key")) {
			plr.GetComponent<Inventory> ().UnselectedObject ();
			plr.GetComponent<Inventory> ().DeleteObject (holding);
			currentLock.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			currentLock.tag = "Holdable";
			LockCount -= 1;

			if (LockCount <= 0) {
				Debug.Log ("Exit open!");
                StartCoroutine(dooropen());
			}
		}
	}

    IEnumerator dooropen()
    {
        float timeSinceStarted = 0f;
        int count = 0;
        while (true)
        {
            timeSinceStarted += Time.deltaTime / 2;
            GameObject.Find("InteriorDoorPart").transform.RotateAround(GameObject.Find("DoorPos").transform.position,new Vector3(0,0.1f,0),timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (GameObject.Find("InteriorDoorPart").transform.rotation.y >= 0.9)
            {
                break;
            }

            // Otherwise, continue next frame
            count += 1;
            yield return null;
        }

        yield break;
    }
}
