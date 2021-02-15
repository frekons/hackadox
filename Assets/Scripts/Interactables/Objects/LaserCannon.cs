using System;
using UnityEngine;

public class LaserCannon : HarmfulObject, ITooltip
{
	public LaserSt Laser = new LaserSt();

	[SerializeField]
	private AudioClip _winSound, _loseSound;

	[SerializeField]
	private AudioSource _audioSource;

	private void Update()
	{
		LineRenderer laser = transform.GetChild(0).GetComponent<LineRenderer>();

		if (Laser.Work)
		{
			var hit = Physics2D.Raycast(laser.transform.position, transform.localScale.x > 0 ? laser.transform.right : -laser.transform.right);

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
			laser.SetPosition(1, Vector3.zero);
		}
	}

	public void OnHover()
	{
		TooltipManager.Instance.ShowTooltip(Laser.Work ? "KORUMALI ADRES" : "HACKED");
	}

	private bool _playingMinigameCurrently = false;

	public void OnClick()
	{
		if (Laser.Work && !_playingMinigameCurrently)
		{
			_playingMinigameCurrently = true;

			MiniGame.CreateMinigame(21, 20, () =>
			{
				Debug.Log("player won game");
				//ConsolePanel.Instance.AddVariable("laser", Laser, null);
				Laser.Work = false;

				_audioSource.PlayOneShot(_winSound);

				_playingMinigameCurrently = false;
			},
			() =>
			{
				_audioSource.PlayOneShot(_loseSound);

				Debug.Log("player lost game");

				_playingMinigameCurrently = false;
			});
		}
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
