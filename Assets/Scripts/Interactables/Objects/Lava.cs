using UnityEngine;

public class Lava : HarmfulObject
{
	private CooldownManager cooldownManager = new CooldownManager();

	[Range(0f, 100f)]
	public float damageCooldown;

	public override void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private void OnTriggerStay2D(UnityEngine.Collider2D collision)
	{
		if (!cooldownManager.IsInCooldown("lava_damage"))
		{
			GiveDamage(collision);
			cooldownManager.SetCooldown("lava_damage", damageCooldown);
		}
	}
}
