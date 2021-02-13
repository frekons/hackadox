using UnityEngine;

public class HarmfulObject : Interactable
{
	[Range(0, 100)]
	public float maxDamage;

	public void GiveDamage(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponent<PlayerController>()?.TakeDamage(Random.Range(0, maxDamage));
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		GiveDamage(collision);
	}
}
