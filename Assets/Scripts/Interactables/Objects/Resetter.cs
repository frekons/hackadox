using UnityEngine;

public class Resetter : Interactable
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Debug.Log("Everything has reset.");

			PlayerController playerController = collision.GetComponent<PlayerController>();

			playerController.ResetToDefaults();
		}
	}
}
