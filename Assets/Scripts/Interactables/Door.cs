using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	[Header("Next scene path")]
	[SerializeField]
	public string scenePath;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			Debug.Log("Player entered the exit door.");

			collider.GetComponent<PlayerController>().canMove = false;

			FadeEffect.instance.FadeIn(() =>
			{
				Debug.Log("Loading '" + scenePath.Split('/')[2].Replace(".unity", "") + "' named scene.");

				SceneManager.LoadScene(scenePath);
			});
		}
	}
}
