using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MasterMind : MonoBehaviour {
    System.Random rand = new System.Random();
    private List<GameObject> cubelist = new List<GameObject>();
    private List<GameObject> colors = new List<GameObject>();
    private List<string> code = new List<string>();
    public Material gm;
    public Material om;
    public Material nm;
    public bool solved = false;
    public bool isolved = false;
	// Use this for initialization
	void Start () {
        GameObject blue = new GameObject();
        blue.transform.eulerAngles = new Vector3(0, 0, 0);
        blue.name = "blue";
        colors.Add(blue);
        GameObject red = new GameObject();
        red.transform.eulerAngles = new Vector3(180, 0, -90);
        red.name = "red";
        colors.Add(red);
        GameObject yellow = new GameObject();
        yellow.transform.eulerAngles = new Vector3(90, 0, 0);
        yellow.name = "yellow";
        colors.Add(yellow);
        GameObject white = new GameObject();
        white.transform.eulerAngles = new Vector3(180, 0, 90);
        white.name = "white";
        colors.Add(white);
        GameObject black = new GameObject();
        black.transform.eulerAngles = new Vector3(270, 0, 0);
        black.name = "black";
        colors.Add(black);
        GameObject green = new GameObject();
        green.transform.eulerAngles = new Vector3(180, 0, 0);
        green.name = "green";
        colors.Add(green);

        for (int i = 0; i < 4; i++)
        {
            code.Add(new string[] { "red", "blue", "green", "yellow", "black", "white" }[rand.Next(0, 6)]);
            Debug.Log(code[i]);
        }
    }

    private void changecolor(GameObject cube, string color)
    {
        foreach (var item in colors)
        {
            if (item.name != color)
                continue;
            cube.transform.eulerAngles = item.transform.eulerAngles;
        }
    }

    private void CheckCode()
    {
        List<string> copy = code;
        List<GameObject> copy2 = cubelist;
        List<GameObject> found = new List<GameObject>();
        List<string> lights = new List<string>();
        int codecount = 0;
        foreach (var item in copy)
        {
            foreach (var item2 in copy2)
            {
                if (GetColor(item2) == item)
                {
                    
                    if (cubelist.IndexOf(item2) == codecount)
                    {
                        Debug.Log("Object found on same spot");
                        lights.Add("green");
                        found.Add(item2);
                        break;
                    }
                }
            }
            codecount += 1;
        }
        foreach (var item in copy)
        {
            foreach (var item2 in copy2)
            {
                if (GetColor(item2) == item)
                {
                    if (cubelist.IndexOf(item2) == copy.IndexOf(item))
                    {
                        break;
                    }
                    if (!found.Contains(item2))
                    {
                        Debug.Log("Object found on dif spot");
                        lights.Add("orange");
                        found.Add(item2);
                        break;
                    }
                }
            }
        }
        int gc = 0;
        foreach (var item in lights)
        {
            if (item == "green")
            {
                gc += 1;
            }
        }
        if (gc >= 4)
        {
            solved = true;
        }
        List<GameObject> lamps = new List<GameObject>();
        foreach (Transform item in GameObject.Find("Mlights").transform)
        {
            lamps.Add(item.gameObject);
            item.GetComponent<MeshRenderer>().material = nm;
        }
        Debug.Log(lights.Count);
        while (lights.Count > 0)
        {
            foreach (var item in lights)
            {
                if (item == "green")
                {
                    lamps[0].GetComponent<MeshRenderer>().material = gm;
                    lights.Remove(item);
                    lamps.Remove(lamps[0]);
                    break;
                }
                else if(item == "orange")
                {
                    lamps[0].GetComponent<MeshRenderer>().material = om;
                    lights.Remove(item);
                    lamps.Remove(lamps[0]);
                    break;
                }
            }
        }
        
    }

    private string GetColor(GameObject cube)
    {
        foreach (var item in colors)
        {
            if (cube.transform.eulerAngles != item.transform.eulerAngles)
                continue;
            return item.name;
        }
        return "none";
    }

    public void ClickEvent(GameObject cube)
    {
        switch (GetColor(cube))
        {
            case "blue":
                changecolor(cube, "red");
                break;
            case "red":
                changecolor(cube, "yellow");
                break;
            case "yellow":
                changecolor(cube, "black");
                break;
            case "black":
                changecolor(cube, "white");
                break;
            case "white":
                changecolor(cube, "green");
                break;
            case "green":
                changecolor(cube, "blue");
                break;
        }
        CheckCode();
    }
    public void addobject(GameObject cube)
    {
        cubelist.Add(cube);
        CheckCode();
    }
}
