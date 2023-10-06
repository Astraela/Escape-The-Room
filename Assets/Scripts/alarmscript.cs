using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class alarmscript : MonoBehaviour
{

    private DateTime start = DateTime.Now;
    private bool goingon = false;
    private IntroScript IS;
    public GameObject dialog;
    private bool open = false;
    public bool puzzle1 = false;
    public bool puzzle2 = false;
    public bool puzzle3 = false;
    public bool puzzle4 = false;
    public bool puzzle5 = false;
    private Inventory inv;
    public GameObject fps;
    private int pos = 0;
    private DateTime mp = DateTime.Now;
    private DateTime ms = DateTime.Now;
    private TimeSpan mt;
    public GameObject esc;
    // Use this for initialization
    void Start()
    {
        IS = GameObject.Find("Intro").GetComponent<IntroScript>();
        mt = ms - mp;
        inv = fps.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (esc.activeSelf)
            {
                esc.SetActive(false);
                //ms = DateTime.Now;
                //mt = mt + (ms - mp);
                //goingon = true;
                inv.enable();
            }
            else if(goingon == true)
            {
                esc.SetActive(true);
                //mp = DateTime.Now;
                //goingon = false;
                inv.disable();
            }
        }
        if (open)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                dialog.SetActive(false);
            }
        }
        if (goingon)
        {
            DateTime now = DateTime.Now;
            TimeSpan dif = (now - start) - mt;
            string sec = (59 - dif.Seconds).ToString();
            string min = (14 - dif.Minutes).ToString();
            while (sec.Length < 2)
            {
                sec = "0" + sec;
            }
            while (min.Length < 2)
            {
                min = "0" + min;
            }
            if (sec == "60")
            {
                sec = "00";
            }
            gameObject.GetComponent<TextMesh>().text = min + ":" + sec;
            if (min == "00" && sec == "00")
            {
                goingon = false;
                GameObject.Find("TvNoise").GetComponent<VideoPlayer>().renderMode = VideoRenderMode.CameraNearPlane;
                GameObject.Find("TvNoise").GetComponent<VideoPlayer>().targetCamera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
                GameObject.Find("TvNoise").GetComponent<VideoPlayer>().Play();
                StartCoroutine(wait(3));
            }
            if ((min == "12" && sec == "00") || (min == "09" && sec == "00") || (min == "06" && sec == "00") || (min == "03" && sec == "00"))
            {
                open = true;
                if (!puzzle1)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It seems like you can fit 4 pieces in the box with the drawer";
                    dialog.SetActive(true);
                }
                else if (!puzzle2)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "The drawing made of lines on the wall look like a code. There must be a guide somewhere LIKE UNDER A BOX";
                    dialog.SetActive(true);
                }
                else if (!puzzle3)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It looks like you must throw 3 specific balls into the hole in the wall. The drawing next to it might show the order and what balls.";
                    dialog.SetActive(true);
                }
                else if (!puzzle4)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It looks like you can turn on the TV but you need a remote. Maybe it shows a clue that has something to do with the books. Like an order.";
                    dialog.SetActive(true);
                }
                else if (!puzzle5)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It looks like you need to finish 2 puzzles for the button to work. Placing a cone on the table might help with getting the correct rotation. It seems like you can change the color of the cubes to finish the second one";
                    dialog.SetActive(true);
                }
                else if (pos == 0)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It seems like you can walk through the door";
                    dialog.SetActive(true);
                    pos += 1;
                }
                else if (pos == 1)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "It seems like walking to the end of the white room finishes the game";
                    dialog.SetActive(true);
                    pos += 1;
                }
                else if (pos == 2)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = "Walk through the god damn door and to the end of the god damn white";
                    dialog.SetActive(true);
                    pos += 1;
                }
                else if (pos == 3)
                {
                    dialog.transform.Find("Text").GetComponent<Text>().text = @"Ay 
Fonsi 
DY 
Oh
Oh no, oh no
Oh yeah
Diridiri, dirididi Daddy 
Go
Sí, sabes que ya llevo un rato mirándote 
Tengo que bailar contigo hoy (DY) 
Vi que tu mirada ya estaba llamándome 
Muéstrame el camino que yo voy (Oh)
Tú, tú eres el imán y yo soy el metal 
Me voy acercando y voy armando el plan 
Solo con pensarlo se acelera el pulso (Oh yeah)
Ya, ya me está gustando más de lo normal 
Todos mis sentidos van pidiendo más 
Esto hay que tomarlo sin ningún apuro
Despacito 
Quiero respirar tu cuello despacito 
Deja que te diga cosas al oído 
Para que te acuerdes si no estás conmigo
Despacito 
Quiero desnudarte a besos despacito 
Firmo en las paredes de tu laberinto 
Y hacer de tu cuerpo todo un manuscrito (sube, sube, sube)
(Sube, sube)
Quiero ver…";
                    dialog.SetActive(true);
                    pos += 1;
                }
            }
        }
    }

    public void starttimer()
    {
        start = DateTime.Now;
        goingon = true;

    }
    public void stoptimer()
    {
        goingon = false;
    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        IS.M.transform.position = new Vector3(0.852f, 1.479f, -1.651f);
        IS.M.transform.rotation = new Quaternion(0, 180, 0, 0);
        //IS.M.transform.rotation = IS.CTVP.transform.rotation;
        //IS.M.transform.position = IS.CTVP.transform.position;
        IS.M.SetActive(true);
        GameObject.Find("TvNoise").GetComponent<VideoPlayer>().renderMode = VideoRenderMode.RenderTexture;
        IS.TVEND.SetActive(true);
        IS.TvB.SetActive(true);
        IS.endsc();
        GameObject.Find("FPSController").SetActive(false);
    }
    public void close()
    {
        esc.SetActive(false);
        inv.enable();
    }
}
