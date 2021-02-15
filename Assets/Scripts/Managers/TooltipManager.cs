using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
	private string _message;
	private bool _isShowing, _isReceived;

	[SerializeField]
	private Camera _mainCamera;

	public TextMeshProUGUI MessageText;

	public string Message
	{
		get
		{
			return _message;
		}
		set
		{
			if (value != _message)
				OnTooltipMessageChange(value);

			_message = value;
		}
	}

    private void Start()
    {
        _mainCamera = Camera.main;

		_rectTransform = GetComponent<RectTransform>();

	}

    private void OnLevelWasLoaded(int level)
    {
		_mainCamera = Camera.main;
	}

    private void OnEnable()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			throw new System.Exception("More than one instance of singleton detected.");
		}

		Instance = this;
	}

	private void Update()
	{
		var mousePosition = Input.mousePosition;

		mousePosition.z = 9.2f;

		Vector2 screen2world = _mainCamera.ScreenToWorldPoint(mousePosition);

		var hits = Physics2D.Raycast(screen2world, Vector2.zero);

		Debug.Log("hits length: " + (bool)hits + ", screen2world: " + screen2world + ", inputMousePos: " + Input.mousePosition);

		if (hits)
		{
			_isReceived = false;

			hits.transform.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);

			if (_isReceived && Input.GetKeyDown(KeyCode.Mouse0))
			{
				hits.transform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			if (_isShowing)
				HideTooltip();
		}


		if (!_isReceived)
			HideTooltip();
	}

	private RectTransform _rectTransform;

	public void ShowTooltip(string message = "")
	{
		Message = message;
		Debug.Log("mousepos: " + Input.mousePosition);
		_rectTransform.anchoredPosition = Input.mousePosition + new Vector3(0, (42.5f / 2) + 5f, 0); // 42.5f == transform.GetComponent<RectTransform>().rect.height
		transform.GetComponent<CanvasGroup>().alpha = 1;

		_isReceived = true;
		_isShowing = true;
	}

	public void HideTooltip()
	{
		transform.GetComponent<CanvasGroup>().alpha = 0;
		_isShowing = false;
	}

	public void OnTooltipMessageChange(string newMessage)
	{
		MessageText.text = newMessage;
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
	}

	public static TooltipManager Instance;
}
