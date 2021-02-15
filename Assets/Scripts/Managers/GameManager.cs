using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum DamageTypes
	{
		Suicide,
		Laser,
		Weapon
	}

	private void OnEnable()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			throw new System.Exception("More than one instance of singleton detected.");
		}

		Instance = this;

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if (Functions.Instance)
        {
			Functions.Instance.GameManager = this;
		}
	}

	public void ResetGame()
	{
		//var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		//playerController.ResetToDefaults();

		//playerController.Spawn();

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

	public void OnPlayerReset(PlayerController playerController)
	{
		//ConsolePanel.Instance.AddVariable("player", playerController.Player, playerController.VisibleAttributesDict);
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

	public static GameManager Instance;
}
