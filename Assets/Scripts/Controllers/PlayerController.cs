using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour, ITooltip
{
	#region SOUNDS
	public AudioClip JumpSound, DieSound;
	#endregion

	#region COMPONENTS
	[Header("Components")]

	[SerializeField]
	private Image _healthBar;

	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private CapsuleCollider2D _collider;

	private CameraController _cameraController;

	private Rigidbody2D _rigibody2d;

	private Animator _animator;

	private SpriteRenderer _sprite;

	private CooldownManager _cooldownManager = new CooldownManager();
	#endregion

	[Header("Ground Layer Mask")]
	public LayerMask TerrainLayer;

	#region PLAYER FIELDS
	[Header("Player")]
	public float jumpCooldown = 0.1f;
	public int canMove = 1;
	public bool isDead = false;

	public List<VisibleAttributes> VisibleAttributesList = new List<VisibleAttributes>();
	public Dictionary<string, bool> VisibleAttributesDict = new Dictionary<string, bool>();
	public Player Player = new Player();

	private Coroutine _platformerReset;
	private Coroutine _spawnedEffect;
	private bool _hasPressedJump;
	private bool _facingLeft = false;
	public bool FacingLeft
	{
		get
		{
			return _facingLeft;
		}
		set
		{
			if (_facingLeft != value)
				OnPlayerFacingDirectionChange(value);

			_facingLeft = value;
		}
	}
	#endregion

	#region UNITY FUNCTIONS
	void Start()
	{
		_rigibody2d = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		_cameraController = Camera.main.GetComponent<CameraController>();

		foreach (var attr in VisibleAttributesList)
		{
			VisibleAttributesDict.Add(attr.Name, attr.IsVisible);
		}

		Spawn();
	}

	private void Update()
	{
		_healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, Player.health / 100.0f, Time.deltaTime * 5.0f);

		if (isDead)
			return;

		if (transform.position.y < -10f)
			KillPlayer();

		_animator.SetBool("isJumped", !IsGrounded());

		if (canMove <= 0)
			return;

		if (Input.GetButtonDown("Jump") && CanPressJump())
			_hasPressedJump = true;

		if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") != 0)
			FacingLeft = true;
		else if (Input.GetAxisRaw("Horizontal") != 0)
			FacingLeft = false;

		if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && IsGrounded())
		{
			var hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, (int)Mathf.Pow(2, 7));

			if (hit)
			{
				PlatformEffector2D platformEffector = hit.transform.GetComponent<PlatformEffector2D>();

				if (platformEffector)
				{
					platformEffector.rotationalOffset = 180;

					if (_platformerReset != null)
						StopCoroutine(_platformerReset);
					_platformerReset = StartCoroutine(PlatformerReset(platformEffector));
				}
			}
		}

		if (IsGrounded())
			_animator.SetFloat("walkSpeed", Mathf.Abs(_rigibody2d.velocity.x) > 0 ? 2 : 0);
	}

	private void FixedUpdate()
	{
		if (canMove <= 0)
			return;

		if (isDead)
			return;

		float horizontal = Input.GetAxis("Horizontal");

		Vector2 targetVelocity = new Vector2(horizontal * Player.walkSpeed, _rigibody2d.velocity.y);

		if (CanJump() && _hasPressedJump)
			Jump(ref targetVelocity);

		_rigibody2d.velocity = targetVelocity;
	}
	#endregion

	#region EVENTS 
	public void OnPlayerFacingDirectionChange(bool facingLeft)
	{
		_sprite.flipX = facingLeft;
		//_cameraController.CameraOffset = facingLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

		GameManager.Instance.OnPlayerFacingDirectionChange(facingLeft);
	}

	public void OnPlayerEnterDoor()
	{
		GameManager.Instance.OnPlayerEnterDoor();

		canMove--;
		_animator.SetFloat("walkSpeed", 0);
		_animator.SetBool("isJumped", false);
	}
	#endregion

	#region FUNCTIONS 
	IEnumerator PlatformerReset(PlatformEffector2D platformEffector)
	{
		const float DISTANCE_ERROR = 0.5f;

		var waitForEndOfFrame = new WaitForEndOfFrame();

		while (true)
		{
			float distance = Mathf.Abs(platformEffector.transform.position.y - transform.position.y);

			if (distance > (_collider.size.y / (2.0f - DISTANCE_ERROR)))
				break;

			yield return waitForEndOfFrame;
		}

		platformEffector.rotationalOffset = 0;
	}

	public void Spawn()
	{
		if (isDead)
		{
			_animator.SetBool("isDead", false);
			_animator.SetInteger("damageType", 0);
			isDead = false;
			Player.health = 100f;
		}

		ResetToDefaults();

		if (Timer.Instance)
			Timer.SetTimer(Timer.Instance.Time); // restart timer

		//GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = Player.health.ToString();

		_rigibody2d.velocity = Vector2.zero;
		_rigibody2d.angularVelocity = 0;

		gameObject.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		PlaySpawnEffect(new Color(1.0f, 1.0f, 1.0f, 0.5f));

		StartCoroutine(SetCanMove(true, 0.25f));

		FadeEffect.Instance.FadeOut(() =>
		{
			//if (isDead)
			//	CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, false);
			//canMove = true;

			CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, true);
		});

		GameManager.Instance.OnPlayerSpawn();

		Debug.Log("Player has spawned.");
	}

	IEnumerator SetCanMove(bool canMove, float afterSecond)
    {
		yield return new WaitForSeconds(afterSecond);

		this.canMove = canMove ? (this.canMove + 1) : (this.canMove - 1);
    }

	public void PlaySpawnEffect(Color color, int count = 5, float waitTime = 0.2f)
	{
		if (_spawnedEffect != null)
			StopCoroutine(_spawnedEffect);

		_spawnedEffect = StartCoroutine(SpawnedEffect(color, count, waitTime));
	}

	public IEnumerator SpawnedEffect(Color color, int count = 5, float waitTime = 0.2f)
	{
		for (int i = 0; i < count; i++)
		{
			GetComponent<SpriteRenderer>().color = color;
			yield return new WaitForSeconds(waitTime);
			GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
			yield return new WaitForSeconds(waitTime);
		}
	}

	private void Jump(ref Vector2 targetVelocity)
	{
		_audioSource.PlayOneShot(JumpSound);

		targetVelocity.y = Player.jumpForce;
		_hasPressedJump = false;
		_cooldownManager.SetCooldown("jump", jumpCooldown);

		GameManager.Instance.OnPlayerJump();
	}

	public bool IsCloseToGround(float distance)
	{
		var sourcePoint = _collider.bounds.min;

		sourcePoint.x = transform.position.x;

		RaycastHit2D hit = Physics2D.Raycast(sourcePoint, Vector2.down, distance, TerrainLayer);

		return hit.transform != null;
	}

	public bool IsGrounded()
	{
		var sourcePoint = _collider.bounds.min;

		sourcePoint.x = transform.position.x;

		RaycastHit2D hit = Physics2D.Raycast(sourcePoint, Vector2.down, 0.1f, TerrainLayer);

		return hit.transform != null;
	}

	public bool CanPressJump()
	{
		return IsCloseToGround(0.25f) ? (!_cooldownManager.IsInCooldown("jump")) : false;
	}

	public bool CanJump()
	{
		return IsGrounded() ? (!_cooldownManager.IsInCooldown("jump")) : false;
	}

	public void TakeDamage(float damage, GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || canMove <= 0)
			return;

		Player.health -= damage;

		if (Player.health <= 0)
		{
			Player.health = 0;
			KillPlayer(damageType);
		}

		//GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = Player.health.ToString();

		GameManager.Instance.OnPlayerTakeDamage(damage, damageType);
	}

	public void KillPlayer(GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead/* || !canMove*/)
			return;

		Player.health = 0;

		_audioSource.PlayOneShot(DieSound);

		Debug.Log("Player has dead.");

		//CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, false);
		CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, true);

		isDead = true;
		canMove--;

		_animator.SetFloat("walkSpeed", 0);
		_animator.SetBool("isJumping", false);
		_animator.SetBool("isDead", true);
		_animator.SetInteger("damageType", (int)damageType);

		_rigibody2d.velocity = Vector2.zero;
		_rigibody2d.angularVelocity = 0;

		FadeEffect.Instance.FadeIn(() =>
		{
			Spawn();
		});

		GameManager.Instance.OnPlayerDead(damageType);
	}

	public void ResetToDefaults()
	{
		Player = new Player();

		GameManager.Instance.OnPlayerReset(this);

		CallAllProperties();
	}

	public void CallAllProperties()
	{
		foreach (var item in Player.GetType().GetProperties())
		{
			if (item.GetCustomAttributes(true).FirstOrDefault(element => element is CallAttribute) != default)
			{
				item.SetValue(Player, item.GetValue(Player));
			}
		}
	}
	#endregion

	private void OnDestroy()
	{
		if (ConsolePanel.Instance)
		{
			ConsolePanel.Instance.RemoveVariable(CONSOLE_ID);
		}
	}

    //
    const string CONSOLE_ID = "player";

	public void OnHover()
	{
		TooltipManager.Instance.ShowTooltip(CONSOLE_ID);
	}

	public void OnClick()
	{
		if (ConsolePanel.Instance)
		{
			ConsolePanel.Instance.AddVariable(CONSOLE_ID, Player, VisibleAttributesDict);
		}
	}
}


[Serializable]
public class Player
{
	public float _health = 100f;
	public float _walkSpeed = 5f;
	public float _jumpForce = 5.5f;
	public float _gravity = -9.81f;
	public float _playerPositionX, _playerPositionY;

	[Call]
	public float health
	{
		get
		{
			return _health;
		}

		set
		{
			Debug.Log("set health called!");

			_health = value;
		}
	}

	[Call]
	public float walkSpeed
	{
		get
		{
			return _walkSpeed;
		}

		set
		{
			Debug.Log("set walk speed called!");

			_walkSpeed = value;
		}
	}

	[Call]
	public float jumpForce
	{
		get
		{
			return _jumpForce;
		}

		set
		{
			Debug.Log("set jump force called!");

			_jumpForce = value;
		}
	}

	[Call]
	public float gravity
	{
		get
		{
			return _gravity;
		}

		set
		{
			Debug.Log("set gravity called!");

			Physics2D.gravity = new Vector2(0, value);

			_gravity = value;
		}
	}

	public float posX
	{
		get
		{
			return _playerPositionX;
		}

		set
		{
			Debug.Log("set pos X called!");

			Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
			playerPos.x = value;
			GameObject.FindWithTag("Player").transform.position = playerPos;

			_playerPositionX = value;
		}
	}

	public float posY
	{
		get
		{
			return _playerPositionY;
		}

		set
		{
			Debug.Log("set pos Y called!");

			Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
			playerPos.y = value;
			GameObject.FindWithTag("Player").transform.position = playerPos;

			_playerPositionY = value;
		}
	}
}

public class CallAttribute : Attribute
{

}
