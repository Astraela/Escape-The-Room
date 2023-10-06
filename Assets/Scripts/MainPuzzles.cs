using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;

public class MainPuzzles : MonoBehaviour {

	public GameObject key1;
	public GameObject key2;
	public GameObject key3;
	public GameObject key4;
	public GameObject key5;

	public GameObject player;
	public GameObject crosshair;
	public GameObject codePopup;
	private int ShapesCollected = 0;
	private bool popupOpen = false;
	private GameObject currentPuzzle;
	public GameObject ballChecker;
	private bool isResettingBalls = false;
	private List<GameObject> allBooks = new List<GameObject> ();
	private bool bookCaseOpen = false;
	private bool BallsComplete = false;
	private bool wcDoorOpen = false;
	public GameObject colorCode;
	private bool TvOn = false;
	private int ConesCollected = 0;
	private bool allConesCollected = false;
	private int MMCubeCount = 0;
	private bool CollectedMMCubes = false;
    public GameObject blocc;

	public void HandlePuzzleInteraction(GameObject puzzle, GameObject holding){
		currentPuzzle = puzzle;
		if (puzzle.name == "ShapeBox") {
			ShapeBox (puzzle, holding);
        } else if (puzzle.name.Contains("MMCube")) {
            ChangeMMCube(puzzle);
        } else if (puzzle.name == "Safe" && !popupOpen) {
			OpenPopUp ();
		} else if (puzzle.name == "BallResetButton" && !isResettingBalls) {
			StartCoroutine (BallResetButton ());
		} else if (puzzle.transform.parent.name == "Book") {
			MainBook ();
		} else if (puzzle.name == "WcDoorOpen" && !wcDoorOpen && GameObject.Find("Mastermind").GetComponent<MasterMind>().solved && GameObject.Find("Mastermind").GetComponent<MasterMind>().isolved) {
			StartCoroutine (WcDoorOpen ());
		} else if (puzzle.name == "TvObject") {
			HandleTvRemote (holding);
		} else if (puzzle.name == "Mastermind") {
			PlaceCube (puzzle, holding);
		}
	}

	// PUZZLE ONE

	private void ShapeBox(GameObject curPuzz, GameObject curObj){
		if (curObj) {
			Transform spot = curPuzz.transform.Find (curObj.name);
			if(spot){
				player.GetComponent<Inventory> ().UnselectedObject ();
				player.GetComponent<Inventory> ().DeleteObject (curObj);

				curObj.transform.position = spot.position;
				curObj.tag = "Untagged";
				curObj.SetActive (true);

				ShapesCollected += 1;
				if (ShapesCollected >= 4) {
                    GameObject.Find("TimerText").GetComponent<alarmscript>().puzzle1 = true;
					Transform lade = curPuzz.transform.Find ("Lade");
					key1.SetActive (true);
					StartCoroutine (MoveLade (lade));
				}
			}
		}
	}

	IEnumerator MoveLade(Transform l){
		for(int i = 0; i < 20; i++){
			l.localPosition += new Vector3(0.025f,0,0);
			yield return new WaitForSeconds(0);
		}

	}

	// PUZZLE TWO

	public bool getPopupState(){
		return popupOpen;
	}

	public void EnterButton(){
		StartCoroutine (ClosePopup());
	}

	private void OpenPopUp(){
		popupOpen = true;
		codePopup.SetActive (true);

		player.GetComponent<FirstPersonController> ().m_MouseLook.lockCursor = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		crosshair.SetActive (false);

		player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 0;
		player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 0;
	}

	IEnumerator ClosePopup(){
		GameObject inputField = codePopup.transform.Find ("InputField").gameObject;
		string inputtedCode = inputField.transform.Find("CodeInput").GetComponent<Text> ().text;
		string mainCode = currentPuzzle.transform.GetChild (0).GetChild (0).name;
		if (inputtedCode == mainCode) {
			inputField.GetComponent<Image> ().color = new Color (0,255,0,255);
			key2.SetActive (true);
			currentPuzzle.transform.Find ("Door").gameObject.SetActive (false);
			currentPuzzle.GetComponent<BoxCollider> ().enabled = false;
            GameObject.Find("TimerText").GetComponent<alarmscript>().puzzle2 = true;
        } else {
			inputField.GetComponent<Image> ().color = new Color (255,0,0,255);
		}

		yield return new WaitForSeconds (1);

		codePopup.SetActive (false);
		inputField.GetComponent<Image> ().color = new Color (255,255,255,255);
		inputField.GetComponent<InputField> ().Select ();
		inputField.GetComponent<InputField> ().text = "";

		player.GetComponent<FirstPersonController> ().m_MouseLook.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		crosshair.SetActive (true);

		player.GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 2;
		player.GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 2;

		popupOpen = false;
	}

	// PUZZLE THREE

	IEnumerator BallResetButton(){
		isResettingBalls = true;
		ballChecker.GetComponent<BallChecker> ().door1.SetActive (true);
		ballChecker.GetComponent<BallChecker> ().door2.SetActive (false);

		if (ballChecker.GetComponent<BallChecker> ().CheckBalls () && !BallsComplete) {
			BallsComplete = true;
			key3.SetActive (true);
            GameObject.Find("TimerText").GetComponent<alarmscript>().puzzle3 = true;
        }
		ballChecker.GetComponent<BallChecker> ().ResetBalls ();

		yield return new WaitForSeconds (2);
		ballChecker.GetComponent<BallChecker> ().door1.SetActive (false);
		ballChecker.GetComponent<BallChecker> ().door2.SetActive (true);
		isResettingBalls = false;
	}

	// PUZZLE FOUR

	private bool CheckBooks(){
		int bookCount = 0;
		foreach (GameObject item in allBooks) {
			if (item.name == "SpecialBook" && item.transform.GetChild(0).name == allBooks.IndexOf(item).ToString()) {
				bookCount += 1;
			} else {
				return false;
			}
		}
		if (bookCount == 4) {
			return true;
		}
		return false;
	}

	private void MainBook(){
		GameObject MainBookCase = currentPuzzle.transform.parent.parent.parent.gameObject;
		Vector3 localPos = currentPuzzle.transform.parent.localPosition;
		if (localPos.z >= 0) {
			localPos = new Vector3 (localPos.x, localPos.y, -0.0625f);
			allBooks.Add (currentPuzzle);
		} else {
			localPos = new Vector3 (localPos.x, localPos.y, 0);
			allBooks.Remove (currentPuzzle);
		}
		currentPuzzle.transform.parent.localPosition = localPos;

		if (CheckBooks() && !bookCaseOpen)
        {
            GameObject.Find("TimerText").GetComponent<alarmscript>().puzzle4 = true;
            bookCaseOpen = true;
			key4.SetActive (true);
			StartCoroutine (OpenBookCase(MainBookCase));
		}
	}

	private void HandleTvRemote(GameObject holding){
		if (holding && holding.name == "Remote" && !TvOn) {
			TvOn = true;
			colorCode.SetActive (true);
            GameObject.Find("TvNoise").GetComponent<VideoPlayer>().Play();
            GameObject.Find("TvNoise").GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;
            blocc.SetActive(true);
            StartCoroutine (TurnTvOff ());
		}
	}

	IEnumerator OpenBookCase(GameObject mbc){
		for(int i = 0; i < 40; i++){
			mbc.transform.eulerAngles += new Vector3 (0, -1, 0);
			yield return new WaitForSeconds(0);
		}
	}

	IEnumerator TurnTvOff(){
		yield return new WaitForSeconds (5);
		colorCode.SetActive (false);
		TvOn = false;
        GameObject.Find("TvNoise").GetComponent<VideoPlayer>().Stop();
        blocc.SetActive(false);
    }

	// PUZZLE FIVE

	IEnumerator WcDoorOpen()
    {
        GameObject.Find("TimerText").GetComponent<alarmscript>().puzzle5 = true;
        wcDoorOpen = true;
		GameObject rotatePoint = GameObject.Find ("WcDoorRotate");

		for(int i = 0; i < 80; i++){
			rotatePoint.transform.eulerAngles += new Vector3 (0,1.5f,0);
			yield return new WaitForSeconds (0.01f);
		}
	}

	public void CollectCone(){
		ConesCollected += 1;
		if (ConesCollected >= 3 && !allConesCollected) {
			allConesCollected = true;
            GameObject.Find("Mastermind").GetComponent<MasterMind>().isolved = true;

        }
	}

	void PlaceCube(GameObject curPuzzle, GameObject curObj){
		if (curObj && curObj.name.Contains("MMCube")) {
			string curNumber = curObj.name.Substring (6,1);
			GameObject curSpot = curPuzzle.transform.Find ("CubeSpot" + curNumber).gameObject;

			player.GetComponent<Inventory> ().UnselectedObject ();
			player.GetComponent<Inventory> ().DeleteObject (curObj);

            curObj.transform.position = curSpot.transform.position;
			curObj.tag = "Interactable";
			curObj.SetActive (true);

			MMCubeCount += 1;
            GameObject.Find("Mastermind").GetComponent<MasterMind>().addobject(curObj);
            if (MMCubeCount >= 4 && !CollectedMMCubes) {
				CollectedMMCubes = true;
				curPuzzle.tag = "Untagged";
			}
		}
	}

	void ChangeMMCube(GameObject cube){
        GameObject.Find("Mastermind").GetComponent<MasterMind>().ClickEvent(cube);
	}
}
