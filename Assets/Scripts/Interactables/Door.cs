using UnityEngine;

public class Door : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			FadeEffect.instance.FadeOut();
		}
	}
}
