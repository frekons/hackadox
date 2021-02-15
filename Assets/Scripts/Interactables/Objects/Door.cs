using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
	public Sprite[] Sprites = new Sprite[2];

	[HideInInspector]
	public string ScenePath;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			Timer.Pause();

			Debug.Log("Player entered the exit door.");

			PlayerController player = collider.GetComponent<PlayerController>();

			if (!string.IsNullOrWhiteSpace(ScenePath))
			{
				if (player)
					if (player.canMove > 0)
					{
						SetDoorOpen(true);

						collider.GetComponent<PlayerController>().OnPlayerEnterDoor();

						FadeEffect.Instance.FadeIn(() =>
						{
							Debug.Log("Loading '" + ScenePath + "' named scene.");

							SceneManager.LoadScene(ScenePath);

							Timer.Resume();
						});
					}
			}
			else
            {
				Debug.LogError("No scene selected for the door.");
			}
		}
	}

	public void SetDoorOpen(bool state)
	{
		GetComponent<SpriteRenderer>().sprite = Sprites[state == false ? 0 : 1];
	}
}
