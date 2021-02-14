using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{



    int RSelect;

    bool flag = false;

    bool isClicked = false;

    int rNum = Random.Range(100, 999);

    private float lastClick = 0;

    int WinTime = 0;

    private void Start()
    {
        Init();
    }


    void Init()
    {
        RSelect = Random.Range(1, 22);
    }

    void Update()
    {
        Debug.Log(RSelect);
        for (int i = 1; i < 22; i++)
        {


            if (i >= 1 && i <= 7)
            {
                if (RSelect == i && flag == true) continue;

                rNum = Random.Range(100, 999);
                GameObject.Find("Buttons").transform.Find("ButtonsA").transform.Find(i.ToString()).transform.Find("Text").GetComponent<Text>().text = rNum.ToString();
            }

            if (i >= 8 && i <= 14)
            {
                if (RSelect == i && flag == true) continue;

                rNum = Random.Range(100, 999);
                GameObject.Find("Buttons").transform.Find("ButtonsB").transform.Find(i.ToString()).transform.Find("Text").GetComponent<Text>().text = rNum.ToString();
            }

            if (i >= 15 && i <= 23)
            {
                if (RSelect == i && flag == true) continue;

                rNum = Random.Range(100, 999);
                GameObject.Find("Buttons").transform.Find("ButtonsC").transform.Find(i.ToString()).transform.Find("Text").GetComponent<Text>().text = rNum.ToString();
            }

            if (i == 21) flag = true;

        }

        
        if (RSelect >= 1 && RSelect <= 7 && isClicked == false)
        {
            if (lastClick > (Time.time - 1f)) return;
            lastClick = Time.time;
            Button answer = GameObject.Find("Buttons").transform.Find("ButtonsA").transform.Find(RSelect.ToString()).GetComponent<Button>();
            answer.onClick.AddListener(Win);
        }

        if (RSelect >= 8 && RSelect <= 14 && isClicked == false)
        {
            if (lastClick > (Time.time - 1f)) return;
            lastClick = Time.time;
            Button answer = GameObject.Find("Buttons").transform.Find("ButtonsB").transform.Find(RSelect.ToString()).GetComponent<Button>();
            answer.onClick.AddListener(Win);
        }

        if (RSelect >= 15 && RSelect <= 23 && isClicked == false)
        {
            if (lastClick > (Time.time - 1f)) return;
            lastClick = Time.time;
            Button answer = GameObject.Find("Buttons").transform.Find("ButtonsC").transform.Find(RSelect.ToString()).GetComponent<Button>();
            answer.onClick.AddListener(Win);
        }

        isClicked = false;
    }

    void Win()
    {

        isClicked = true;

        WinTime++;
        
        if(WinTime < 3)
        {

            Debug.Log("You chose the correct button!");

            GameObject.Find("Counter").transform.GetComponent<TMPro.TextMeshProUGUI>().text = WinTime + "/3";

            Init();

        }
        else EndMG();

    }

    void EndMG()
    {
        //laser disabled

        WinTime = 0;

        gameObject.SetActive(false);
    }

    

    }
