using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highighting : MonoBehaviour {

	public Material outline;
	public GameObject crosshair;
	public Sprite defaultCrosshair;
	public Sprite graspingHand;
	public Sprite inventoryIcon;
	public Sprite interactableIcon;
	public GameObject puzzleManager;
	Camera mainCamera;
	GameObject currentObject;
	Material currentMaterial;
	string currentTag;

	// Use this for initialization
	void Start () {
		mainCamera = this.GetComponent<Camera> ();
		currentTag = "Untagged";
	}
	
	// Update is called once per frame
	void Update () {
		crosshair.transform.position = Input.mousePosition;

		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if ((hit.transform.CompareTag ("Holdable") || hit.transform.CompareTag("Pickupable") || hit.transform.CompareTag("Interactable") || hit.transform.CompareTag("Key") || hit.transform.CompareTag("Lock")) && !transform.parent.GetComponent<Inventory>().GetInventoryState() && !GetComponent<ObjectHolding>().IsHolding() && !puzzleManager.GetComponent<MainPuzzles>().getPopupState() && hit.distance <= 3) {
				if (currentObject != hit.transform.gameObject) {
					if (currentObject) {
						//currentObject.GetComponent<MeshRenderer> ().material = currentMaterial;
                        currentObject.GetComponent<OutlineScript>().color = 1;
                        currentObject = null;
						currentMaterial = null;
						currentTag = "Untagged";

						crosshair.GetComponent<Image> ().sprite = defaultCrosshair;
						crosshair.GetComponent<RectTransform> ().sizeDelta = new Vector2 (10,10);
					}

					currentObject = hit.transform.gameObject;
					currentMaterial = currentObject.GetComponent<MeshRenderer> ().material;
					currentTag = hit.transform.tag;
					//currentObject.GetComponent<MeshRenderer> ().material = outline;
                    currentObject.GetComponent<OutlineScript>().color = 0;
					outline.mainTexture = currentMaterial.mainTexture;
					outline.mainTextureScale = currentMaterial.mainTextureScale;

					if (hit.transform.CompareTag ("Holdable")) {
						crosshair.GetComponent<Image> ().sprite = graspingHand;
						crosshair.GetComponent<RectTransform> ().sizeDelta = new Vector2 (40, 25);
					} else if (hit.transform.CompareTag ("Pickupable") || hit.transform.CompareTag ("Key")) {
						crosshair.GetComponent<Image> ().sprite = inventoryIcon;
						crosshair.GetComponent<RectTransform> ().sizeDelta = new Vector2 (40, 40);
					} else if (hit.transform.CompareTag ("Interactable")) {
						crosshair.GetComponent<Image> ().sprite = interactableIcon;
						crosshair.GetComponent<RectTransform> ().sizeDelta = new Vector2 (40, 40);
					}
				}
			}
			else {
				if (currentObject) {
					//currentObject.GetComponent<MeshRenderer> ().material = currentMaterial;
                    currentObject.GetComponent<OutlineScript>().color = 1;
                    currentObject = null;
					currentMaterial = null;
					if (currentTag == "Pickupable" || currentTag == "Interactable" || currentTag == "Lock" || currentTag == "Key") {
						currentTag = "Untagged";
					}

					crosshair.GetComponent<Image> ().sprite = defaultCrosshair;
					crosshair.GetComponent<RectTransform> ().sizeDelta = new Vector2 (10,10);
				}
			}
		}
		if(Input.GetKeyUp(KeyCode.Mouse0)){
			if (currentTag == "Holdable") {
				GetComponent<ObjectHolding> ().SetObject (currentObject);
			} else if (currentTag == "Pickupable" || currentTag == "Key") {
				currentTag = "Untagged";
				transform.parent.GetComponent<Inventory> ().PickUp (currentObject);
			} else if (currentTag == "Lock") {
				GameObject.Find ("ExitManager").GetComponent<ExitScript> ().HandleInteraction (transform.parent.gameObject, currentObject);
			} else if (currentTag == "Interactable") {
				GameObject.Find ("PuzzleManager").GetComponent<MainPuzzles> ().HandlePuzzleInteraction (currentObject, transform.parent.GetComponent<Inventory>().GetHoldingObject());
			}
		}
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
			if (currentTag == "Holdable") {
				GetComponent<ObjectHolding> ().ThrowObject (currentObject);
			}
        }
	}
}
