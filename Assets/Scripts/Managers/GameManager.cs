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

}
