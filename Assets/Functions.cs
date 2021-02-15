using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextList
{
    public float AtTime;
    [TextArea]
    public string Text;
    public bool ClearBefore;

    [HideInInspector]
    public bool HasWritten;
}

public class Functions : MonoBehaviour
{
    public List<TextList> Texts = new List<TextList>();

    [HideInInspector]
    public GameManager GameManager;

    private float _startTime;

    private float getTime
    {
        get
        {
            return Time.time - _startTime;
        }
    }

    private void Awake()
    {
        Instance = this;

        _startTime = Time.time;
    }

    private IEnumerator Start()
    {
        float time = getTime;

        var waitForSecs = new WaitForSeconds(0.1f);

        if (Texts.Count > 0)
        {
            while (time <= Texts[Texts.Count - 1].AtTime)
            {
                foreach (var text in Texts)
                {
                    if (text.AtTime < time && !text.HasWritten)
                    {
                        if (text.ClearBefore)
                        {
                            ConsolePanel.Instance.Clear();
                        }

                        ConsolePanel.Instance.Write(text.Text);

                        text.HasWritten = true;
                    }
                }

                time = getTime;

                yield return waitForSecs;
            }
        }
    }

    public void ResetGame()
    {
        GameManager.ResetGame();

        GameCanvas.Instance.HackerPanel.SetActive(true);
    }

    //

    private static Functions _instance;

    public static Functions Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Functions>();
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
}
