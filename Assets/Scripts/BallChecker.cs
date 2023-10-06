using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallChecker : MonoBehaviour {

	public GameObject door1;
	public GameObject door2;
	private List<GameObject> ballsEntered = new List<GameObject>();

	bool CheckInArray(GameObject obj){
		foreach(GameObject item in ballsEntered){
			if(item == obj){
				return true;
			}
		}
		return false;
	}

	public bool CheckBalls(){
		int CorrectCount = 0;
		foreach (GameObject ball in ballsEntered) {
			Transform order = ball.transform.Find ("OrderNumber");
			if (order && order.GetChild (0).name == ballsEntered.IndexOf (ball).ToString ()) {
				CorrectCount += 1;
			} else {
				return false;
			}
		}
		if (CorrectCount == 3) {
			return true;
		}
		return false;
	}

	void OnTriggerEnter(Collider hit){
		if (!CheckInArray (hit.gameObject)) {
			ballsEntered.Add (hit.gameObject);
			if (ballsEntered.Count >= 5) {
				door1.SetActive (true);
			}
		}
	}

	public void ResetBalls(){
		ballsEntered = new List<GameObject> ();
	}
}
