using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
	private Image image;
	public static FadeEffect instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		image = GetComponent<Image>();
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
		image.color = new Color(0, 0, 0, 0);
		for (float i = 0; i <= 1; i += Time.deltaTime)
		{
			image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		image.color = new Color(0, 0, 0, 1);

		if (callback != null)
			callback();
	}

	private IEnumerator _FadeOut(Action callback)
	{
		image.color = new Color(0, 0, 0, 1);
		for (float i = 1; i >= 0; i -= Time.deltaTime)
		{
			image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		image.color = new Color(0, 0, 0, 0);

		if (callback != null)
			callback();
	}
}
