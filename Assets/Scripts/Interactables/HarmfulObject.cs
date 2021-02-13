using UnityEngine;

public class HarmfulObject : Interactable
{
	[Header("Damage")]
	[Range(0, 100)]
	[SerializeField]
	private float maxDamage;

	[SerializeField]
	private bool randomDamage;

	public void GiveDamage(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			collision.GetComponent<PlayerController>()?.TakeDamage(randomDamage ? Random.Range(0, maxDamage) : maxDamage);
	}
}
