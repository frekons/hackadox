using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{

    public int ButtonCount = 21;

    public int MaxWinCount = 3;

    public float timeRemaining = 20f;


    int AnswerButtonIndex = 0;

    int WinCounter = 0;


    private UnityAction WinMG;

    private UnityAction LoseMG;


    [SerializeField]
    private Animator anim;


    [SerializeField]
    private List<Button> Buttons;

    [SerializeField]
    private TMPro.TextMeshProUGUI TimerText;

    [SerializeField]
    private GameObject ButtonPrefab;

    [SerializeField]
    private Transform ButtonContainer;

    [SerializeField]
    private TMPro.TextMeshProUGUI WinCounterText;

    public void Update()
    {

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            TimerText.text = timeRemaining.ToString("0.0#");
        }
        else
        {
            LoseMiniGame();
        }

    }

    void Init()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            var go = Instantiate(ButtonPrefab, ButtonContainer);

            var button = go.GetComponent<Button>();

            Buttons.Add(button);

            int _i = i;

            button.onClick.AddListener(()=>
            {
                onButtonClick(_i); 
            });
        }

        AnswerButtonIndex = Random.Range(0, Buttons.Count);

        RandomizerCaller();

    }

    public static void CreateMinigame(int buttonCount, float second, UnityAction onWin, UnityAction onLose)
    {

        var minigame = Resources.Load<GameObject>("Prefabs/Minigame-Canvas");

        MiniGame _minigame = Instantiate(minigame).transform.GetChild(0).GetComponent<MiniGame>();

        _minigame.ButtonCount = buttonCount;

        _minigame.timeRemaining = second;

        _minigame.anim.SetBool("onStart", true);

        _minigame.WinMG = onWin;

        _minigame.LoseMG = onLose;

        _minigame.Init();

    }

    public void WinMiniGame()
    {

        WinMG();

        StartCoroutine(EndMiniGame());

    }

    public void LoseMiniGame()
    {

        LoseMG();

        StartCoroutine(EndMiniGame());

    }

    public void onButtonClick(int i)
    {

        Debug.Log(AnswerButtonIndex+1);

        if (i == AnswerButtonIndex)
        {
            WinCounter++;

            WinCounterText.text = WinCounter + "/" + MaxWinCount;

            Debug.Log("Turret MiniGame: Correct button!");

            DestroyAllButtons();

            if (WinCounter >= MaxWinCount)
            {
                WinMiniGame();
            }
                 
            Init();
        }
        else
        {
            LoseMiniGame();
        }

    }

    IEnumerator EndMiniGame()
    {

        anim.SetBool("onStart", false);

        anim.SetBool("onEnd", true);

        WinCounter = 0;

        yield return new WaitForSeconds(0.3f);

        Destroy(transform.parent.gameObject);

    }

    public void DestroyAllButtons()
    {
        for (int i = Buttons.Count-1; i >= 0; i--)
        {
            Destroy(Buttons[i].gameObject);
        }

        Buttons.Clear();
    }

    void RandomizerCaller()
    {

        Buttons[AnswerButtonIndex].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Random.Range(100, 999).ToString();

        for (int i = 0; i < Buttons.Count; i++)
        {

            if (AnswerButtonIndex == i) continue;

            StartCoroutine(ButtonRandomizer(Buttons[i].transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>()));

        }
        

    }


    IEnumerator ButtonRandomizer(TMPro.TextMeshProUGUI texts)
    {

        var waitForSec = new WaitForSeconds(0.5f);

        while (WinCounter < MaxWinCount && texts != null)
        {

            int rNum = Random.Range(100, 999);

            texts.text = rNum.ToString();

            yield return waitForSec;

        }


    }


}


    

 
    

    
