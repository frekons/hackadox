using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public enum DamageTypes
	{
		Suicide,
		Laser,
		Weapon
	}


	void Awake()
	{
		instance = this;
	}

	public void ResetGame()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResetToDefaults();
	}

	#region PLAYER EVENTS
	public void OnPlayerDead(DamageTypes damageType = DamageTypes.Suicide)
	{

	}

	public void OnPlayerEnterDoor()
	{

	}

	public void OnPlayerSpawn()
	{

	}

	public void OnPlayerJump()
	{

	}

	public void OnPlayerFacingDirectionChange(bool facingLeft)
	{

	}

	public void OnPlayerTakeDamage(float damage, DamageTypes damageType = DamageTypes.Suicide)
	{

	}

	public void OnPlayerReset()
	{

	}
	#endregion

	#region OBJECT EVENTS
	public void OnEnterLava(Collider2D collision)
	{

	}

	public void OnExitLava(Collider2D collision)
	{

	}
	#endregion
}
