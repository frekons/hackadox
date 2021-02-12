using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VariablePrefab : MonoBehaviour
{
    public TMP_Dropdown VariablesDropdownPrefab;

    public TMP_InputField PopInputFieldPrefab;

    public TextMeshProUGUI _textMeshPro;

    public Button _button;

    public string VariableName
    {
        get
        {
            return _textMeshPro.text;
        }
    }

    private System.Type _type;

    private object _object;

    public void Set(string variableName, object @object, System.Type type)
    {
        _textMeshPro.text = variableName;

        this._object = @object;

        this._type = type;
    }

    public void OnButtonPress()
    {
        //Debug.Log("Button Pressed! Type: " + _type.Name);

        _button.interactable = false;

        ConsolePanel.Instance.WriteCallback(VariableName + ".", () =>
        {
            var dropdown = Instantiate(VariablesDropdownPrefab, ConsolePanel.Instance.transform)
                                                .GetComponent<TMP_Dropdown>();

            dropdown.options.Add(new TMP_Dropdown.OptionData("none"));

            foreach (var field in _type.GetFields())
            {
                //print(field /*+ ": " + field.GetValue(_object)*/);

                dropdown.options.Add(new TMP_Dropdown.OptionData(field.Name /*+ ": " + field.GetValue(_object)*/));
            }

            dropdown.transform.position = GetTextWorldTopRightPosition(ConsolePanel.Instance._consoleText);

            dropdown.Show();

            dropdown.onValueChanged.AddListener((int selectedIndex) =>
            {
                var option = dropdown.options[selectedIndex];

                //ConsolePanel.Instance.Clear();

                ConsolePanel.Instance.WriteCallback(option.text + " = ", ()=>
                {
                    var inputField = Instantiate<TMP_InputField>(PopInputFieldPrefab, ConsolePanel.Instance.transform);

                    inputField.transform.position = GetTextWorldTopRightPosition(ConsolePanel.Instance._consoleText);

                    inputField.Select();

                    inputField.onEndEdit.AddListener((string input) =>
                    {
                        Debug.Log("Option: " + option.text);

                        var field = _type.GetField(option.text);

                        var args = new object[] { input };

                        var parseMethodInfo = field.FieldType.GetMethod("Parse", new System.Type[] { typeof(string) });

                        var result = parseMethodInfo.Invoke(null, args);

                        field.SetValue(_object, result);

                        Debug.Log("result: " + result);

                        ConsolePanel.Instance.WriteLine(result.ToString());

                        _button.interactable = true;

                        Destroy(inputField.gameObject);
                    });
                });

                Destroy(dropdown.gameObject);
            });
        });


    }

    Vector3 GetTextWorldTopRightPosition(TextMeshProUGUI textMeshProUGUI)
    {
        var characterInfo = textMeshProUGUI.textInfo.characterInfo[ConsolePanel.Instance._consoleText.textInfo.characterCount - 1];

        var localPosition = characterInfo.topRight + Vector3.right * 10 + Vector3.up * 10;

        return textMeshProUGUI.transform.TransformPoint(localPosition);
    }
}
