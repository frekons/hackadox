using UnityEngine;

public class LaserCannon : HarmfulObject
{
	public override void OnTriggerEnter2D(Collider2D collision) { }

	private void Update()
	{
		LineRenderer laser = transform.GetChild(0).GetComponent<LineRenderer>();
		var hit = Physics2D.Raycast(laser.transform.position, (transform.localScale.x > 0 ? Vector2.left : -Vector2.left));

		if (hit)
		{
			float distance = Vector2.Distance(transform.position, hit.collider.transform.position);
			distance -= 0.3f;

			laser.SetPosition(1, (transform.localScale.x > 0 ? -Vector2.left : Vector2.right) * distance);

			if (hit.transform.CompareTag("Player"))
				hit.transform.GetComponent<PlayerController>().OnPlayerDead(GameManager.DamageTypes.Laser);
		}
		else
			laser.SetPosition(1, Vector3.MoveTowards(laser.GetPosition(1), (transform.localScale.x > 0 ? -Vector2.left : Vector2.right) * 25f, Time.deltaTime * 20f));
	}
}
