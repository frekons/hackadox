using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip _switchButtonSound;

	[SerializeField]
	private GameObject _selector;

	[SerializeField]
	private List<Button> _buttons;

	[SerializeField]
	private AudioMixer _audioMixer;


	private GameObject _selectedButton;

	[SerializeField]
	private Sprite[] _audioButtonSprites;

	private bool _isSoundActive = true;
	private float _tempVolume;
	private bool _isClickedPlay;

	public GameObject SelectedButton
	{
		get
		{
			return _selectedButton;
		}

		set
		{
			if (_selectedButton != value && value != null)
			{
				UISoundManager.PlayOneShot(_switchButtonSound);
			}

			_selectedButton = value;
		}
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

	private void Start()
	{
		AudioMixer = _audioMixer;

		//var eventSystem = EventSystem.current;

		//eventSystem.SetSelectedGameObject(_buttons[0].gameObject);
	}

	private void LateUpdate()
	{
		if (_isClickedPlay)
			return;

		var eventSystem = EventSystem.current;

		if (eventSystem && eventSystem.currentSelectedGameObject != null &&
			SelectedButton != eventSystem.currentSelectedGameObject &&
			eventSystem.currentSelectedGameObject.GetComponent<Button>() &&
			eventSystem.currentSelectedGameObject.name != "Audio Toggle Button")
		{
			SelectedButton = eventSystem.currentSelectedGameObject;
		}

		if (SelectedButton)
		{
			_selector.transform.position = Vector3.Lerp(_selector.transform.position, SelectedButton.transform.position, Time.deltaTime * 10.0f);
		}
	}

	//

	public void OnMouseEnterButton(GameObject button, PointerEventData eventData)
	{
		if (_isClickedPlay)
			return;

		SelectedButton = button;
	}

	public void OnVolumeSliderChange(float volume)
	{
		if (_isClickedPlay)
			return;

		_audioMixer.SetFloat("MainVolume", volume);
		_isSoundActive = true;
		_buttons[2].GetComponent<Image>().sprite = _audioButtonSprites[1];
	}

	public void OnPressPlay()
	{
		Debug.Log("Pressed play button.");

		if (_isClickedPlay)
			return;

		FadeEffect.Instance.FadeIn();

		_isClickedPlay = true;

		CoroutineManager.Instance.StartCoroutine(FadeMixerGroup.FadeOut(_audioMixer, "MainVolume", 1f, 0f, () =>
		{
			SceneManager.LoadScene(1);

			CoroutineManager.Instance.StartCoroutine(FadeMixerGroup.FadeOut(_audioMixer, "MainVolume", 1f, 1f));
		}));
	}

	public void OnClickToggleAudio()
	{
		_isSoundActive = !_isSoundActive;
		if (_isSoundActive == false)
		{
			_audioMixer.GetFloat("MainVolume", out _tempVolume);
			_audioMixer.SetFloat("MainVolume", -80f);
		}
		else
		{
			_audioMixer.SetFloat("MainVolume", _tempVolume);
		}

		EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = _audioButtonSprites[_isSoundActive ? 1 : 0];
	}

	//

	public static MenuManager Instance;

	public static AudioMixer AudioMixer;
}
