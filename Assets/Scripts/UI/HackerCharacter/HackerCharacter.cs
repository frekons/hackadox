using UnityEngine;

public class HackerCharacter : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	private void OnEnable()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			throw new System.Exception("More than one instance of singleton detected.");
		}

		Instance = this;
	}

	public void SetIdle()
	{
		_animator.SetInteger("State", (int)HACKER_STATE.IDLE);
	}

	public void SetHacking()
	{
		_animator.SetInteger("State", (int)HACKER_STATE.HACKING);
	}


	enum HACKER_STATE
	{
		IDLE,
		HACKING
	}

	//

	public static HackerCharacter Instance;
}
