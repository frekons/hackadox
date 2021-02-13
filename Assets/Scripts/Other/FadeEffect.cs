using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
	private Image _image;
	public static FadeEffect Instance;

	private void Awake()
	{
		_image = GetComponent<Image>();
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

	public void FadeIn(Action callback = null)
	{
		StopAllCoroutines();
		StartCoroutine(_FadeIn(callback));
	}

	public void FadeOut(Action callback = null)
	{
		StopAllCoroutines();
		StartCoroutine(_FadeOut(callback));
	}

	private IEnumerator _FadeIn(Action callback)
	{
		_image.color = new Color(0, 0, 0, 0);
		for (float i = 0; i <= 1; i += Time.deltaTime)
		{
			_image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		_image.color = new Color(0, 0, 0, 1);

		if (callback != null)
			callback();
	}

	private IEnumerator _FadeOut(Action callback)
	{
		_image.color = new Color(0, 0, 0, 1);
		for (float i = 1; i >= 0; i -= Time.deltaTime)
		{
			_image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		_image.color = new Color(0, 0, 0, 0);

		if (callback != null)
			callback();
	}
}
