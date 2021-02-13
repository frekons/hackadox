using UnityEngine;

public class HarmfulObject : Interactable
{
	[Header("Damage")]
	[Range(0, 100)]
	[SerializeField]
	private float _maxDamage;

	[SerializeField]
	private bool _randomDamage;

	public void GiveDamage(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			collision.GetComponent<PlayerController>()?.TakeDamage(_randomDamage ? Random.Range(0, _maxDamage) : _maxDamage);
	}
}
