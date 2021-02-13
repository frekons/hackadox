using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[Header("Properties")]
	public bool Active = false;
	public float Time = 5;

	[Header("Events")]
	public UnityEvent OnEnd;

	[Header("Objects")]
	[SerializeField]
	private Slider _progressSlider;

	[SerializeField]
	private GameObject _timeIsUpPrefab;

	private IEnumerator timerNumerator;

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
		if (Active)
		{
			SetTimer(Time);
		}
	}

	private GameObject _timeIsUpObject;

	private IEnumerator TimerIEnumerator(float seconds, UnityAction onStart = null, UnityAction<float> onProgress = null, UnityAction onEnd = null)
	{
		if (_timeIsUpObject)
		{
			Destroy(_timeIsUpObject);

			_timeIsUpObject = null;
		}

		if (onStart != null)
			onStart();

		var waitForEndOfFrame = new WaitForEndOfFrame();

		float currentTime = seconds;

		float value = 1.0f;

		while (value >= 0)
		{
			_progressSlider.value = value;

			currentTime -= UnityEngine.Time.deltaTime;

			value = currentTime / seconds;

			if (onProgress != null)
				onProgress.Invoke(value);

			yield return waitForEndOfFrame;
		}

		if (onEnd != null)
			onEnd();

		OnEnd.Invoke();

		_timeIsUpObject = Instantiate(_timeIsUpPrefab, transform);

		timerNumerator = null;
	}

	#region public static methods

	public static void SetTimer(float seconds, UnityAction onStart = null, UnityAction<float> onProgress = null, UnityAction onEnd = null)
	{
		if (Instance.timerNumerator != null)
		{
			Instance.StopCoroutine(Instance.timerNumerator);

			Instance.timerNumerator = null;
		}

		Instance.timerNumerator = Instance.TimerIEnumerator(seconds, onStart, onProgress, onEnd);

		Instance.StartCoroutine(Instance.timerNumerator);
	}

	#endregion

	public static Timer Instance;
}
