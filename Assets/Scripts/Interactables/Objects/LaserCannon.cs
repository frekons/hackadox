using System;
using UnityEngine;

public class LaserCannon : HarmfulObject, ITooltip
{
	public LaserSt Laser;

	private void Update()
	{
		LineRenderer laser = transform.GetChild(0).GetComponent<LineRenderer>();

		if (Laser.Work)
		{
			var hit = Physics2D.Raycast(laser.transform.position, (transform.localScale.x > 0 ? Vector2.right : Vector2.left));

			if (hit)
			{
				float distance = Vector2.Distance(laser.transform.position, hit.point);
				laser.SetPosition(1, Vector2.right * distance);

				hit.transform.SendMessage("KillPlayer", GameManager.DamageTypes.Laser, SendMessageOptions.DontRequireReceiver);

				//if (hit.transform.CompareTag("Player"))
				//{
				//	Rigidbody2D playerRigidbody = hit.transform.GetComponent<Rigidbody2D>();
				//	Vector2 vel = (transform.localScale.x > 0 ? Vector2.right : Vector2.left) * 5;

				//	vel.y = playerRigidbody.velocity.y;
				//	playerRigidbody.velocity = vel;
				//}
			}
			else
			{
				laser.SetPosition(1, Vector3.MoveTowards(laser.GetPosition(1), Vector2.right * 25f, Time.deltaTime * 20f));
			}
		}
		else
        {

        }
	}

	public void OnHover()
	{
		TooltipManager.Instance.ShowTooltip("RESTRICTED");
	}

    public void OnClick()
    {
		MiniGame.CreateMinigame(21, 20, () =>
		{
			ConsolePanel.Instance.AddVariable("laser", Laser, null);
		},
		() =>
		{
			Debug.Log("player lost game");
		});
    }
}

[Serializable]
public class LaserSt
{
	private bool _work = true;

	public bool Work
    {
        get
        {
			return _work;
        }

		set
        {
			_work = value;
        }
    }
}
