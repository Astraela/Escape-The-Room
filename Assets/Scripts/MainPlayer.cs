using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;

public class MainPlayer : MonoBehaviour {

	private GameObject mainCamera;
    private IntroScript IS;
	// Use this for initialization
	void Start () {
		mainCamera = this.transform.GetChild (0).gameObject;
        IS = GameObject.Find("Intro").GetComponent<IntroScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftControl)) {
			mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3 (0, 0, 0), 0.1f);
			GetComponent<FirstPersonController> ().m_WalkSpeed = 2.5f;
			GetComponent<FirstPersonController> ().m_RunSpeed = 2.5f;
		}
		else {
			mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3 (0, 0.8f, 0), 0.1f);
			GetComponent<FirstPersonController> ().m_WalkSpeed = 5;
			GetComponent<FirstPersonController> ().m_RunSpeed = 5;
		}
	}

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.transform.CompareTag("Exit"))
        {
            GameObject.Find("TvNoise").GetComponent<VideoPlayer>().renderMode = VideoRenderMode.CameraNearPlane;
            GameObject.Find("TvNoise").GetComponent<VideoPlayer>().targetCamera = gameObject.transform.Find("FirstPersonCharacter").GetComponent<Camera>();
            GameObject.Find("TvNoise").GetComponent<VideoPlayer>().Play();
            GameObject.Find("TimerText").GetComponent<alarmscript>().stoptimer();
            StartCoroutine(wait(3));
        }
    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        IS.M.transform.position = new Vector3(0.852f, 1.479f, -1.651f);
        IS.M.transform.rotation = new Quaternion(0,180,0,0);
        //IS.M.transform.rotation = IS.CTVP.transform.rotation;
        //IS.M.transform.position = IS.CTVP.transform.position;
        IS.M.SetActive(true);
        GameObject.Find("TvNoise").GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;

        IS.TvB.SetActive(true);
        IS.winsc();
        gameObject.SetActive(false);
    }
}
