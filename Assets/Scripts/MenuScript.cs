using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void click(string btn)
    {
        if (btn == "resume")
        {
            GameObject.Find("TimerText").GetComponent<alarmscript>().close();
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}
