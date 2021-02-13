using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
	public Sprite[] sprites = new Sprite[2];

	private bool isOpen;

	[HideInInspector]
	public string scenePath;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			Debug.Log("Player entered the exit door.");

			PlayerController player = collider.GetComponent<PlayerController>();

			if (!string.IsNullOrWhiteSpace(scenePath))
			{
				if (player)
					if (player.canMove)
					{
						SetDoorOpen(true);

						collider.GetComponent<PlayerController>().canMove = false;

						FadeEffect.instance.FadeIn(() =>
						{
							Debug.Log("Loading '" + scenePath + "' named scene.");

							SceneManager.LoadScene(scenePath);
						});
					}
			}
			else
				Debug.LogError("No scene selected for the door.");

		}
	}

	public void SetDoorOpen(bool state)
	{
		isOpen = state;
		GetComponent<SpriteRenderer>().sprite = sprites[state == false ? 0 : 1];
	}
}
