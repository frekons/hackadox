using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsolePanel : MonoBehaviour
{
    public List<VariablePrefab> VariableList = new List<VariablePrefab>();

    [Header("Fields")]
    [SerializeField]
    [Multiline]
    private string _defaultText = ">\t";

    [Header("Objects")]
    [SerializeField]
    public TextMeshProUGUI _consoleText;

    [SerializeField]
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    [SerializeField]
    private Transform _variablesTransform;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _variablePrefab;

    // ---

    private bool _editingText;


    private void Awake()
    {
        Instance = this;

        _consoleText.text = _defaultText;
    }


    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private IEnumerator ClearIEnumerator(int startIndex, int lastIndex, float waitTime = 0.065f) // startIndex > lastIndex
    {
        while (_editingText)
            yield return _waitForEndOfFrame;

        OnClearStarted();

        _editingText = true;

        var waitForSeconds = new WaitForSecondsRealtime(waitTime);

        for (int i = startIndex; i >= lastIndex; --i)
        {
            var character = _consoleText.text[i];

            _consoleText.text = _consoleText.text.Remove(i);

            OnCharacterRemove(character);

            if (!DiscardedCharacters.Contains(character))
            {
                yield return waitForSeconds;
            }
        }

        _editingText = false;

        OnClearCompleted();
    }

    private IEnumerator WriteIEnumerator(string message, System.Action onWriteEnd = null, float waitTime = 0.065f)
    {
        while (_editingText)
            yield return _waitForEndOfFrame;

        OnWriteStarted();

        _editingText = true;

        var waitForSeconds = new WaitForSecondsRealtime(waitTime);

        foreach (var character in message)
        {
            _consoleText.text += character;

            OnCharacterWrote(character);

            if (!DiscardedCharacters.Contains(character))
            {
                yield return waitForSeconds;
            }
        }

        _editingText = false;

        if (onWriteEnd != null)
            onWriteEnd();

        OnWriteCompleted();
    }

    // ----

    private void OnCharacterWrote(char character)
    {
        // TODO, write sounds
    }

    private void OnCharacterRemove(char character)
    {
        // TODO, write sounds
    }
    

    private void OnWriteStarted()
    {
        HackerCharacter.Instance.SetHacking();
    }

    private void OnClearStarted()
    {

    }

    private void OnWriteCompleted()
    {
        HackerCharacter.Instance.SetIdle();
    }

    private void OnClearCompleted()
    {
        //HackerCharacter.Instance.SetIdle();
    }

    // ---

    public readonly char[] DiscardedCharacters = new char[] // skip waiting in these characters
    {
        ' ',
        '\n'
    };

    public void WriteLine(string message, params object[] args)
    {
        var formattedMessage = string.Format(message, args) + "\n>\t";

        StartCoroutine(WriteIEnumerator(formattedMessage));
    }

    public void Write(string message, params object[] args)
    {
        var formattedMessage = string.Format(message, args) + "\n\t";

        StartCoroutine(WriteIEnumerator(formattedMessage));
    }

    public void WriteCallback(string message, System.Action onWriteEnd)
    {
        StartCoroutine(WriteIEnumerator(message, onWriteEnd));
    }

    public void Clear()
    {
        StartCoroutine(ClearIEnumerator(_consoleText.text.Length - 1, _defaultText.Length));
    }

    public void ClearLastLine()
    {
        int lastIndex = _consoleText.text.LastIndexOf('\n');

        if (lastIndex == -1 || lastIndex < _defaultText.Length)
            return;

        int startIndex = _consoleText.text.Length - 1;

        StartCoroutine(ClearIEnumerator(startIndex, lastIndex));
    }

    public void AddVariable(string variableName, object @object, Dictionary<string, bool> visibleAttributesDict)
    {
        if (VariableList.Find(x => x._textMeshPro.text == variableName) != default) // if already exists
            return;

        var variablePrefab = Instantiate(_variablePrefab, _variablesTransform)
                            .GetComponent<VariablePrefab>();

        variablePrefab.Set(variableName, @object, visibleAttributesDict);

        VariableList.Add(variablePrefab);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_horizontalLayoutGroup.GetComponent<RectTransform>());
    }

    //

    public static ConsolePanel Instance;
}
