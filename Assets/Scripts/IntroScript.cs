using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroScript : MonoBehaviour
{
    
    public VideoPlayer vp;
    public Image i;
    public Image d;
    public Text dt;
    public Button db;
    public Animation pa;
    public GameObject F;
    public GameObject M;
    public GameObject C;
    public GameObject CTVP;
    private GameObject TvN;
    public GameObject TvB;
    private GameObject Lasthit;
    private GameObject MDown;
    public GameObject TVUI;
    public GameObject TVEND;
    public GameObject TVHS;
    public bool onmenu = false;
    private bool texto = false;
    private bool finished = false;
    private string[] dialog = { "Welcome to this Escape Room!", "This Escape Room is to gather information in order to make an actual Escape Room.", "In the game you will have to find 5 keys which will unlock the exit.", "You can perform various interactions with objects, some objects you can pick up, rotate and move, some you can put in your inventory and some objects have unique interactions.", "By hovering over objects you can interact with them in various ways. If the cursor changes into a grasping hand you can hold and move it. If the cursor changes into a box with an arrow you can put it in your inventory. If it's a hand pointing you can interact with it in various ways like using an object from the inventory on it.","You can walk around using W,A,S,D and crouch using leftctrl and of course look around by moving your mouse.","You can open your inventory by pressing tab and you can select an item by clicking on its.","You can exit the game by pressing the escape button." };
    int count = -1;
    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        vp.loopPointReached += videoend;
        i.color = new Color(0, 0, 0, 1);
        CTVP = GameObject.Find("CameraTVPos");
        TvN = GameObject.Find("TvNoise");
        TvB = GameObject.Find("blocc");
        TvB.SetActive(false);
        TVUI = GameObject.Find("TVUI");
        TVUI.SetActive(false);
        TvN.GetComponent<VideoPlayer>().Prepare();
        Physics.IgnoreLayerCollision(10, 9);
    }
    void Update()
    {
        if (i.color == new Color(0, 0, 0, 0))
        {
            i.enabled = false;
            d.gameObject.SetActive(true);
            nextdialog();
            i.color = new Color(1, 1, 1, 0);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            nextdialog();
        }

        if (onmenu)
        {

            RaycastHit2D hit = Physics2D.GetRayIntersection(M.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition));
            if (hit && hit != Lasthit)
            {
                hit.transform.localScale = new Vector3(0.99f, 0.99f, 1);
                Lasthit = hit.transform.gameObject;

            }
            else if (!hit && Lasthit && Lasthit != MDown)
            {
                Lasthit.transform.localScale = new Vector3(1, 1, 1);
                Lasthit = null;
            }
            if (hit && Input.GetKeyDown(KeyCode.Mouse0))
            {
                MDown = hit.transform.gameObject;
                hit.transform.localScale = new Vector3(0.98f, 0.98f, 1);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && MDown)
            {
                MDown.transform.localScale = new Vector3(1, 1, 1);
                if (hit && hit.transform.gameObject == MDown)
                {
                    switch (MDown.name)
                    {
                        case "Start":
                            TVUI.SetActive(false);
                            StartCoroutine("MoveCamera2");
                            onmenu = false;
                            break;

                        case "Exit":
                            Application.Quit();
                            break;

                        case "Retry":
                            SceneManager.LoadScene("Game");
                            break;
                        case "Back":
                            if (finished)
                                SceneManager.LoadScene("Game");
                            StartCoroutine(ToMenu());
                            break;
                        case "HighScores":
                            StartCoroutine(ToHS());
                            break;
                    }
                }
                MDown = null;
            }
        }

    }
    public void nextdialog()
    {
        try
        {

            if (vp.isPlaying)
                return;
            if (texto)
            {
                texto = false;
                return;
            }
            if (count == dialog.Length - 1)
            {
                TvB.SetActive(true);
                TVUI.SetActive(true);
                TvN.GetComponent<VideoPlayer>().Play();
                StartCoroutine("MoveCamera1");
                d.gameObject.SetActive(false);
                count += 1;
                //M.transform.position = Vector3.MoveTowards(GameObject.Find("Main Camera").transform.position, CTVP.transform.position,0.5f);

                //M.SetActive(false);
                //C.SetActive(true);
                //F.SetActive(true);
                return;
            }
            else if (count < dialog.Length - 1)
            {
                count += 1;
                StartCoroutine(textfunction());
            }

        }
        catch (System.Exception)
        {

        }
    }
    void videoend(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        pa.wrapMode = WrapMode.Once;
        pa.Play();
    }
    public void winsc()
    {
        try
        {
            GameObject.Find("Colors").SetActive(false);
        }
        catch (System.Exception)
        {
        }
        StartCoroutine(MoveCamera3());
    }
    public void endsc()
    {
        try
        {
            GameObject.Find("Colors").SetActive(false);
        }
        catch (System.Exception)
        {
        }
        StartCoroutine(MoveCamera4());
    }
    IEnumerator textfunction()
    {

        dt.text = "";
        texto = true;
        for (int i = 0; i < dialog[count].Length; i++)
        {
            if (!texto)
                break;
            dt.text = dt.text + dialog[count][i];
            yield return new WaitForSeconds(0.005f);
            if (!texto)
                break;
        }
        dt.text = dialog[count];
        texto = false;
        if (count == dialog.Length - 1)
        {

            db.GetComponentInChildren<Text>().text = "E";
        }
    }
    IEnumerator MoveCamera1()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime / 4;
            M.transform.position = Vector3.Lerp(M.transform.position, CTVP.transform.position, timeSinceStarted);
            M.transform.rotation = Quaternion.Lerp(M.transform.rotation, CTVP.transform.rotation, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (M.transform.position == CTVP.transform.position)
            {
                break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
        TvN.GetComponent<VideoPlayer>().Stop();
        TvB.SetActive(false);
        onmenu = true;
        yield break;
    }
    IEnumerator MoveCamera2()
    {

        float timeSinceStarted = 0f;
        while (true)
        {

            timeSinceStarted += Time.deltaTime / 4;
            M.transform.position = Vector3.Lerp(M.transform.position, F.transform.Find("FirstPersonCharacter").transform.position, timeSinceStarted);
            M.transform.rotation = Quaternion.Lerp(M.transform.rotation, F.transform.Find("FirstPersonCharacter").transform.rotation, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (M.transform.position == F.transform.Find("FirstPersonCharacter").transform.position)
            {
                break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
        M.SetActive(false);
        C.SetActive(true);
        F.SetActive(true);
        GameObject.Find("TimerText").GetComponent<alarmscript>().starttimer();
        yield break;
    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    IEnumerator MoveCamera3()
    {
        finished = true;
        string time = GameObject.Find("TimerText").GetComponent<TextMesh>().text;
        string tm = time[0].ToString() + time[1].ToString();
        string ts = time[3].ToString() + time[4].ToString();
        string m = (15 - Convert.ToInt32(tm)).ToString();
        string s = (60 - Convert.ToInt32(ts)).ToString();
        Debug.Log(m + ":" + s);
        if (s != "0")
        {
            m = (Convert.ToInt32(m) - 1).ToString();
        }
        while (s.Length < 2)
        {
            s = "0" + s;
        }
        while (m.Length < 2)
        {
            m = "0" + m;
        }
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime / 4;
            M.transform.position = Vector3.Lerp(M.transform.position, CTVP.transform.position, timeSinceStarted);
            M.transform.rotation = Quaternion.Lerp(M.transform.rotation, CTVP.transform.rotation, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (M.transform.position == CTVP.transform.position)
            {
                break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
        TvN.GetComponent<VideoPlayer>().Stop();
        TvB.SetActive(false);
        TVUI.SetActive(false);
        TVHS.SetActive(true);
        if (PlayerPrefs.HasKey("HSs")  && PlayerPrefs.HasKey("HSm") )
        {
            if ((Convert.ToInt32(m) < PlayerPrefs.GetInt("HSm")) ||(Convert.ToInt32(m) <= PlayerPrefs.GetInt("HSm") && Convert.ToInt32(s) < PlayerPrefs.GetInt("HSs")))
            {
                TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = "New Highscore!!!!";
                PlayerPrefs.SetInt("HSm", Convert.ToInt32(m));
                PlayerPrefs.SetInt("HSs", Convert.ToInt32(s));
            }
            else
            {
                if (PlayerPrefs.GetInt("HSs").ToString().Length < 2 && PlayerPrefs.GetInt("HSm").ToString().Length < 2)
                {
                    TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + "0" + PlayerPrefs.GetInt("HSm") + ":0" + PlayerPrefs.GetInt("HSs");
                }
                else if (PlayerPrefs.GetInt("HSs").ToString().Length < 2)
                {
                    TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + PlayerPrefs.GetInt("HSm") + ":0" + PlayerPrefs.GetInt("HSs");
                }
                else if (PlayerPrefs.GetInt("HSm").ToString().Length < 2)
                {
                    TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + "0" + PlayerPrefs.GetInt("HSm") + ":" + PlayerPrefs.GetInt("HSs");
                }
            }
        }
        else if (!PlayerPrefs.HasKey("HSs") && !PlayerPrefs.HasKey("HSm") )
        {
            TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = "New Highscore!!!!";
            PlayerPrefs.SetInt("HSm", Convert.ToInt32(m));
            PlayerPrefs.SetInt("HSs", Convert.ToInt32(s));
        }
            TVHS.transform.Find("YourScore").GetComponent<TextMesh>().text = @"You finished in: 
" + m + ":" + s;
        onmenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        C.transform.Find("Crosshair").gameObject.SetActive(false);
        yield break;
    }
    IEnumerator MoveCamera4()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime / 4;
            M.transform.position = Vector3.Lerp(M.transform.position, CTVP.transform.position, timeSinceStarted);
            M.transform.rotation = Quaternion.Lerp(M.transform.rotation, CTVP.transform.rotation, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (M.transform.position == CTVP.transform.position)
            {
                break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
        TvN.GetComponent<VideoPlayer>().Stop();
        TvB.SetActive(false);
        TVUI.SetActive(false);
        TVEND.SetActive(true);
        onmenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        C.transform.Find("Crosshair").gameObject.SetActive(false);
        yield break;
    }
    IEnumerator ToMenu()
    {
        TvB.SetActive(true);
        TvN.GetComponent<VideoPlayer>().Play();
        TVHS.SetActive(false);
        TVUI.SetActive(true);
        yield return new WaitForSeconds(1);
        TvN.GetComponent<VideoPlayer>().Stop();
        TvB.SetActive(false);
    }
    IEnumerator ToHS()
    {
        TvB.SetActive(true);
        TvN.GetComponent<VideoPlayer>().Play();
        TVHS.SetActive(true);
        TVUI.SetActive(false);
        TVHS.transform.Find("YourScore").GetComponent<TextMesh>().text = "";
        if (PlayerPrefs.HasKey("HSs") && PlayerPrefs.HasKey("HSm"))
        {
            if (PlayerPrefs.GetInt("HSs").ToString().Length < 2 && PlayerPrefs.GetInt("HSm").ToString().Length < 2)
            {
                TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + "0" + PlayerPrefs.GetInt("HSm") + ":0" + PlayerPrefs.GetInt("HSs");
            }
            else if (PlayerPrefs.GetInt("HSs").ToString().Length < 2)
            {
                TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + PlayerPrefs.GetInt("HSm") + ":0" + PlayerPrefs.GetInt("HSs");
            }
            else if (PlayerPrefs.GetInt("HSm").ToString().Length < 2)
            {
                TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = @"Current highscore: 
" + "0" + PlayerPrefs.GetInt("HSm") + ":" + PlayerPrefs.GetInt("HSs");
            }
        }
        else
        {
            TVHS.transform.Find("CHighscore").GetComponent<TextMesh>().text = "There is no current highscore";
        }
            yield return new WaitForSeconds(1);
        TvN.GetComponent<VideoPlayer>().Stop();
        TvB.SetActive(false);
    }

}