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

	public void FadeIn()
	{
		StopAllCoroutines();
		StartCoroutine(_FadeIn());
	}

	public void FadeOut()
	{
		StopAllCoroutines();
		StartCoroutine(_FadeOut());
	}

	private IEnumerator _FadeIn()
	{
		for (float i = 0; i <= 1; i += Time.deltaTime)
		{
			image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		image.color = new Color(0, 0, 0, 1);
	}

	private IEnumerator _FadeOut()
	{
		for (float i = 1; i >= 0; i -= Time.deltaTime)
		{
			image.color = new Color(0, 0, 0, i);
			yield return null;
		}
		image.color = new Color(0, 0, 0, 0);
	}
}
