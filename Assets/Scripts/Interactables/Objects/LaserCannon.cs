using UnityEngine;

public class LaserCannon : HarmfulObject
{
	private void Update()
	{
		LineRenderer laser = transform.GetChild(0).GetComponent<LineRenderer>();
		var hit = Physics2D.Raycast(laser.transform.position, (transform.localScale.x > 0 ? Vector2.right : Vector2.left));

		if (hit)
		{
			float distance = Vector2.Distance(laser.transform.position, hit.point);
			laser.SetPosition(1, Vector2.right * distance);

			hit.transform.SendMessage("KillPlayer", GameManager.DamageTypes.Laser, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			laser.SetPosition(1, Vector3.MoveTowards(laser.GetPosition(1), Vector2.right * 25f, Time.deltaTime * 20f));
		}
	}
}
