using UnityEngine;

public class Lava : HarmfulObject
{
	private CooldownManager cooldownManager = new CooldownManager();

	[Space]
	public bool isStaying;

	[Range(0f, 100f)]
	public float damageCooldown;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			isStaying = true;

		GameManager.Instance.OnEnterLava(collision);
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			isStaying = false;

		GameManager.Instance.OnExitLava(collision);
	}
	private void Update()
	{
		if (isStaying)
			if (!cooldownManager.IsInCooldown("lava_damage"))
			{
				GiveDamage(GameObject.FindWithTag("Player").GetComponent<Collider2D>());
				cooldownManager.SetCooldown("lava_damage", damageCooldown);
			}
	}
}
