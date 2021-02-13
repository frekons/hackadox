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
	private Sprite[] audioButtonSprites;

	private bool isSoundActive = true;
	private float tempVolume;

	private bool isClickedPlay;

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


	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		var eventSystem = EventSystem.current;

		//eventSystem.SetSelectedGameObject(_buttons[0].gameObject);
	}

	private void LateUpdate()
	{
		if (isClickedPlay)
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
		if (isClickedPlay)
			return;

		SelectedButton = button;
	}

	public void OnVolumeSliderChange(float volume)
	{
		if (isClickedPlay)
			return;

		_audioMixer.SetFloat("MainVolume", volume);
		isSoundActive = true;
		_buttons[2].GetComponent<Image>().sprite = audioButtonSprites[1];
	}

	public void OnPressPlay()
	{
		Debug.Log("Pressed play button.");

		if (isClickedPlay)
			return;

		isClickedPlay = true;

		StartCoroutine(FadeMixerGroup.FadeOut(_audioMixer, "MainVolume", 1f, 0f, () =>
		{
			SceneManager.LoadScene(1);
		}));
	}

	public void OnClickToggleAudio()
	{
		isSoundActive = !isSoundActive;
		if (isSoundActive == false)
		{
			_audioMixer.GetFloat("MainVolume", out tempVolume);
			_audioMixer.SetFloat("MainVolume", -80f);
		}
		else
		{
			_audioMixer.SetFloat("MainVolume", tempVolume);
		}

		EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = audioButtonSprites[isSoundActive ? 1 : 0];
	}

	//

	public static MenuManager Instance;
}
