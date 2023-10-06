using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public GameObject inventory;
	public GameObject crosshair;
	public GameObject puzzleManager;
	private GameObject selectedObject;
	private Text selectedObjectName;
	private bool inventoryState;
	private List<GameObject> InventoryList;

	// Use this for initialization
	void Start () {
		selectedObjectName = crosshair.transform.GetChild (0).GetComponent<Text> ();
		inventoryState = false;
		InventoryList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			selectedObjectName.text = "";
			selectedObject = null;
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (inventoryState) {
				CloseInventory ();
			}
			else {
				if(!puzzleManager.GetComponent<MainPuzzles>().getPopupState()){
					inventoryState = true;
					inventory.SetActive (true);

					selectedObjectName.text = "";
					selectedObject = null;

					GetComponent<FirstPersonController> ().m_MouseLook.lockCursor = false;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					crosshair.SetActive (false);

					GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 0;
					GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 0;
				}
			}
		}
	}

	private void CloseInventory(){
		inventoryState = false;
		inventory.SetActive (false);

		GetComponent<FirstPersonController> ().m_MouseLook.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		crosshair.SetActive (true);

		GetComponent<FirstPersonController> ().m_MouseLook.XSensitivity = 2;
		GetComponent<FirstPersonController> ().m_MouseLook.YSensitivity = 2;
	}

	public void UnselectedObject(){
		selectedObjectName.text = "";
		selectedObject = null;
	}

	public void PickUp(GameObject obj){
		if (InventoryList.Count < 8) {
			InventoryList.Add (obj);
			obj.SetActive (false);
			foreach (GameObject item in InventoryList) {
				GameObject slot = inventory.transform.Find ("Slot" + InventoryList.IndexOf (item)).gameObject;
				slot.transform.GetChild (0).GetComponent<Text> ().text = item.name;
			}
		}
	}

	public void ClickedSlot(int index){
		if (InventoryList.Count >= (index + 1)) {
			GameObject slot = inventory.transform.Find ("Slot" + index).gameObject;
			GameObject obj = InventoryList [index];
			selectedObjectName.text = obj.name;
			selectedObject = obj;
			CloseInventory ();
		}
	}

	public void DeleteObject(GameObject item){
		InventoryList.Remove (item);
		inventory.transform.Find ("Slot0").GetChild(0).GetComponent<Text>().text = "Empty";
		foreach (GameObject obj in InventoryList) {
			GameObject slot = inventory.transform.Find ("Slot" + InventoryList.IndexOf (obj)).gameObject;
			GameObject slot2 = inventory.transform.Find ("Slot" + (InventoryList.IndexOf (obj) + 1)).gameObject;
			if (obj.name != item.name) {
				slot.transform.GetChild (0).GetComponent<Text> ().text = obj.name;
			} else {
				slot.transform.GetChild (0).GetComponent<Text> ().text = "Empty";
			}
			slot2.transform.GetChild (0).GetComponent<Text> ().text = "Empty";
		}
	}

	public GameObject GetHoldingObject(){
		return selectedObject;
	}

	public bool GetInventoryState(){
		return inventoryState;
	}
    public void disable()
    {
        GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);

        GetComponent<FirstPersonController>().m_MouseLook.XSensitivity = 0;
        GetComponent<FirstPersonController>().m_MouseLook.YSensitivity = 0;
    }
    public void enable()
    {
        GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);

        GetComponent<FirstPersonController>().m_MouseLook.XSensitivity = 2;
        GetComponent<FirstPersonController>().m_MouseLook.YSensitivity = 2;
    }
}
