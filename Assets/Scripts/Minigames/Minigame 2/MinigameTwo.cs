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
	private CanvasGroup _deathScreen;
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

		cooldownManager.SetCooldown("minigame_2", 5f);

		_isStarted = true;
		_movingObject.sizeDelta = _movingObjectStartSize;

		_counterTransform.GetComponent<CanvasGroup>().alpha = 1;

		_deathScreen.interactable = false;
		_deathScreen.alpha = 0;
	}

	private void ResetPosition()
	{
		Vector3 newObjectPosition = Vector3.zero;

		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		newObjectPosition.x = -580f;

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

	//private float getDistanceBetweenLine(ref Vector3 newObjectPosition)
	//{
	//	Rect endLineRect = RectTransformExt.GetWorldRect(_endLine, new Vector2(1, 1));
	//	Rect moveObjectRect = RectTransformExt.GetWorldRect(_movingObject, new Vector2(1, 1));

	//	return Mathf.Abs((moveObjectRect.width / 2) + newObjectPosition.x - endLineRect.x - (endLineRect.width / 2));
	//}

	private bool isPassedLine(ref Vector3 newObjectPosition)
	{
		Rect endLineRect = RectTransformExt.GetWorldRect(_endLine, new Vector2(1, 1));

		return newObjectPosition.x + (_movingObject.rect.width / 2) > endLineRect.x;
	}

	private void Update()
	{
		if (!_isStarted)
			return;

		if (cooldownManager.IsInCooldown("minigame_2"))
		{
			float time = Mathf.Round(cooldownManager.GetCooldown("minigame_2"));

			_counterText.text = time >= 4 ? "Hazır.." : time == 0 ? "Başla" : time.ToString();
			return;
		}
		else
		{
			_counterTransform.GetComponent<CanvasGroup>().alpha = 0;
		}

		Vector3 newObjectPosition = _movingObject.transform.position;

		if (isPassedWindow(ref newObjectPosition))
		{
			ResetPosition();
			return;
		}

		if (isPassedLine(ref newObjectPosition) && !_isPassed)
		{
			_movingObject.sizeDelta -= new Vector2(5, 5);
			_isPassed = true;

			if (_movingObject.sizeDelta.x <= 20f)
			{
				_isStarted = false;

				_deathScreen.interactable = true;
				_deathScreen.alpha = 1;

				ResetPosition();

				return;
			}
		}

		if (Input.GetButtonDown("Jump") && _isPassed)
		{
			_isStarted = false;
			return;
		}

		newObjectPosition.x += _movingObjectSpeed;

		_movingObject.transform.position = Vector3.Lerp(_movingObject.transform.position, newObjectPosition, Time.deltaTime * 20);
	}
}
