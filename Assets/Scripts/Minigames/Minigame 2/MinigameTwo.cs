using TMPro;
using UnityEngine;

public class MinigameTwo : MonoBehaviour
{
	[SerializeField]
	private RectTransform _movingObject;
	[SerializeField]
	private RectTransform _endLine;

	private CooldownManager cooldownManager = new CooldownManager();

	[SerializeField]
	private Vector2 _movingObjectStartSize;

	[SerializeField]
	private RectTransform _minigameWindow;
	[SerializeField]
	private RectTransform _counterTransform;
	[SerializeField]
	private TextMeshProUGUI _counterText;

	[SerializeField]
	private float _movingObjectSpeed;

	[SerializeField]
	private bool _isStarted;
	private bool _isPassed;

	private void Start()
	{
		_movingObjectStartSize = _movingObject.sizeDelta;

		ResetPosition();
		StartGame();
	}

	public void StartGame()
	{
		ResetPosition();
		cooldownManager.SetCooldown("minigame_2", 0f);
		_isStarted = true;
		_movingObject.sizeDelta = _movingObjectStartSize;
		_counterTransform.GetComponent<CanvasGroup>().alpha = 1;
	}

	private void ResetPosition()
	{
		Vector3 newObjectPosition = Vector3.zero;

		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		newObjectPosition.x = windowRect.x;
		newObjectPosition.x -= _movingObject.rect.width / 2;

		newObjectPosition.y = windowRect.height + windowRect.y;
		newObjectPosition.y -= windowRect.height / 2;

		_movingObject.transform.position = newObjectPosition;

		_isPassed = false;
	}

	private bool isPassedWindow(ref Vector3 newObjectPosition)
	{
		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		return newObjectPosition.x - windowRect.x - (_movingObject.rect.width / 2) > windowRect.width;
	}

	private float getDistanceBetweenLine(ref Vector3 newObjectPosition)
	{
		Rect endLineRect = RectTransformExt.GetWorldRect(_endLine, new Vector2(1, 1));
		Rect moveObjectRect = RectTransformExt.GetWorldRect(_movingObject, new Vector2(1, 1));

		return Mathf.Abs((moveObjectRect.width / 2) + newObjectPosition.x - endLineRect.x - (endLineRect.width / 2));
	}

	private bool isPassedLine(ref Vector3 newObjectPosition)
	{
		Rect endLineRect = RectTransformExt.GetWorldRect(_endLine, new Vector2(1, 1));

		return newObjectPosition.x + (_movingObject.rect.width / 2) > endLineRect.x;
	}
	private void LateUpdate()
	{


	}

	private void Update()
	{

		Vector3 newObjectPosition = _movingObject.transform.position;

		GameObject.Find("Debug Text").GetComponent<TextMeshProUGUI>().text = "isPassed: " + isPassedLine(ref newObjectPosition) + "\n";
		GameObject.Find("Debug Text").GetComponent<TextMeshProUGUI>().text += "getDistanceBetweenLine: " + getDistanceBetweenLine(ref newObjectPosition);

		//GameObject.Find("Debug Text").GetComponent<TextMeshProUGUI>().text = "";

		if (!_isStarted)
			return;

		if (cooldownManager.IsInCooldown("minigame_2"))
		{
			_counterText.text = Mathf.Round(cooldownManager.GetCooldown("minigame_2")).ToString();
			return;
		}
		else
		{
			_counterTransform.GetComponent<CanvasGroup>().alpha = 0;
		}

		newObjectPosition = _movingObject.transform.position;

		if (isPassedWindow(ref newObjectPosition))
		{
			ResetPosition();
			return;
		}

		if (isPassedLine(ref newObjectPosition) && !_isPassed)
		{
			_movingObject.sizeDelta -= new Vector2(10, 10);
			_isPassed = true;

			if (_movingObject.sizeDelta.x <= 10f)
				_isStarted = false;
		}


		//if (Mathf.Floor(getDistanceBetweenLine(ref newObjectPosition)) == 0)
		//{
		//	_isStarted = false;
		//	return;
		//}



		//Debug.Log(getDistanceBetweenLine(ref newObjectPosition));
		//if (getDistanceBetweenLine(ref newObjectPosition) > 0 && getDistanceBetweenLine(ref newObjectPosition) <= 5)
		//	_isStarted = false;

		GameObject.Find("Debug Text").GetComponent<TextMeshProUGUI>().text += "\ncanWin: " + ((getDistanceBetweenLine(ref newObjectPosition) > 0 && getDistanceBetweenLine(ref newObjectPosition) <= (_movingObject.rect.width / 6) + 1));

		if (Input.GetButtonDown("Jump") && _isPassed)
		{
			_isStarted = false;
			return;
		}
		//if (Input.GetButtonDown("Jump") && (getDistanceBetweenLine(ref newObjectPosition) > 0 && getDistanceBetweenLine(ref newObjectPosition) <= (_movingObject.rect.width / 6)) && !_isPassed)
		//{
		//	GameObject.Find("Debug Text").GetComponent<TextMeshProUGUI>().text += "\ngeçtin";
		//	_isStarted = false;
		//	return;

		//}

		newObjectPosition.x += _movingObjectSpeed;




		_movingObject.transform.position = Vector3.Lerp(_movingObject.transform.position, newObjectPosition, Time.deltaTime * 20);


	}
}
