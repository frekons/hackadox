using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

	private object _object;

	Dictionary<string, bool> VisibleAttributesDict;

	public void Set(string variableName, object @object, Dictionary<string, bool> visibleAttributesDict)
	{
		_textMeshPro.text = variableName;

		this._object = @object;

		this.VisibleAttributesDict = visibleAttributesDict;
	}

	public void OnButtonPress()
	{
		//Debug.Log("Button Pressed! Type: " + _type.Name);
		if (ConsolePanel.Instance.TutorialPlaying)
			return;

		_button.interactable = false;

		ConsolePanel.Instance.WriteCallback(VariableName + ".", () =>
		{
			var dropdown = Instantiate(VariablesDropdownPrefab, ConsolePanel.Instance.transform)
												.GetComponent<TMP_Dropdown>();

			dropdown.options.Add(new TMP_Dropdown.OptionData("none"));

			foreach (var field in _object.GetType().GetProperties())
			{
				if (VisibleAttributesDict.ContainsKey(field.Name) && VisibleAttributesDict[field.Name])
				{
					dropdown.options.Add(new TMP_Dropdown.OptionData(field.Name /*+ ": " + field.GetValue(_object)*/));
				}
			}

			dropdown.transform.position = GetTextWorldTopRightPosition(ConsolePanel.Instance._consoleText);

			dropdown.Show();

			dropdown.onValueChanged.AddListener((int selectedIndex) =>
			{
				var option = dropdown.options[selectedIndex];

				//ConsolePanel.Instance.Clear();

				ConsolePanel.Instance.WriteCallback(option.text + " = ", () =>
				{
					var inputField = Instantiate<TMP_InputField>(PopInputFieldPrefab, ConsolePanel.Instance.transform);

					inputField.transform.position = GetTextWorldTopRightPosition(ConsolePanel.Instance._consoleText);

					inputField.Select();

					inputField.onEndEdit.AddListener((string input) =>
					{
						if (input.Trim() == string.Empty)
							return;

						Debug.Log("Option: " + option.text);

						var field = _object.GetType().GetProperty(option.text);

						var args = new object[] { input };

						var parseMethodInfo = field.PropertyType.GetMethod("Parse", new System.Type[] { typeof(string) });

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
