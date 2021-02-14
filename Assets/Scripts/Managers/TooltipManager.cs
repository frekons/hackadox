using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
	private string _message;
	private bool _isShowing, _isReceived;

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

    //private void Start()
    //{
    //	_mainCamera = Camera.main;
    //}

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

	private void LateUpdate()
	{
		Vector2 screen2world = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

		var hit = Physics2D.Raycast(screen2world, Vector2.zero);

		if (hit)
		{
			_isReceived = false;

			hit.transform.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);

			if (_isReceived && Input.GetKeyDown(KeyCode.Mouse0))
			{
				hit.transform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
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

	public void ShowTooltip(string message = "")
	{
		Message = message;

		transform.position = Input.mousePosition + new Vector3(0, (42.5f / 2) + 5f, 0); // 42.5f == transform.GetComponent<RectTransform>().rect.height
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
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newMessage;
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
	}

	public static TooltipManager Instance;
}
