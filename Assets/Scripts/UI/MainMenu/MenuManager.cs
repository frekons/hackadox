using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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
        var eventSystem = EventSystem.current;

        if (eventSystem && eventSystem.currentSelectedGameObject != null && SelectedButton != eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject.GetComponent<Button>())
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
        SelectedButton = button;
    }

    public void OnVolumeSliderChange(float volume)
    {
        _audioMixer.SetFloat("MainVolume", volume);
    }

    //

    public static MenuManager Instance;
}
