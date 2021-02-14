using TMPro;
using UnityEngine;

public class MinigameTwo : MonoBehaviour
{
	[SerializeField]
	private RectTransform _movingObject;
	[SerializeField]
	private RectTransform _minigameWindow;

	private CooldownManager cooldownManager = new CooldownManager();

	[SerializeField]
	private RectTransform _counterTransform;
	[SerializeField]
	private TextMeshProUGUI _counterText;

	[SerializeField]
	private float _movingObjectSpeed = 10f;

	private bool _isStarted;

	private void Start()
	{
		ResetPosition();
		StartGame();
	}

	public void StartGame()
	{
		ResetPosition();
		cooldownManager.SetCooldown("minigame2", 3f);
	}

	private void ResetPosition()
	{
		Vector3 newObjectPosition = Vector3.zero;

		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		newObjectPosition.x = windowRect.x;
		newObjectPosition.x -= -(windowRect.width / 2) + windowRect.width / 2;
		newObjectPosition.x -= (windowRect.width / 2);

		newObjectPosition.y = windowRect.height + windowRect.y;
		newObjectPosition.y -= windowRect.height / 2;

		_movingObject.transform.position = newObjectPosition;
	}

	private bool isPassedWindow(ref Vector3 newObjectPosition)
	{
		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		return newObjectPosition.x - windowRect.x - _movingObject.rect.width / 2 > windowRect.width;
	}

	private bool isPassedLine(ref Vector3 newObjectPosition)
	{
		Rect windowRect = RectTransformExt.GetWorldRect(_minigameWindow, new Vector2(1, 1));

		return newObjectPosition.x - windowRect.x - _movingObject.rect.width / 2 > windowRect.width;
	}

	private void Update()
	{
		_counterText.text = (Mathf.Floor(cooldownManager.GetCooldown("minigame2"))).ToString() + " " + (cooldownManager.IsInCooldown("minigame2"));

		if (!_isStarted)
		{
			return;
		}
		//_isStarted = true;

		Vector3 newObjectPosition = _movingObject.transform.position;

		if (isPassedWindow(ref newObjectPosition))
		{
			ResetPosition();
			return;
		}

		if (isPassedLine(ref newObjectPosition))
		{
			Debug.Log("geçti");
		}

		newObjectPosition.x += 30;

		_movingObject.transform.position = Vector3.Lerp(_movingObject.transform.position, newObjectPosition, Time.deltaTime * 20);
	}
}
