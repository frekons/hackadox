using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
	#region COMPONENTS
	private CameraController _cameraController;
	private Rigidbody2D _rigibody2d;
	private Animator _animator;
	private SpriteRenderer _sprite;
	private CooldownManager _cooldownManager = new CooldownManager();
	private Coroutine _spawnedEffect;
	#endregion

	[Header("Ground Layer Mask")]
	public LayerMask TerrainLayer;

	#region PLAYER FIELDS
	[Header("Player")]
	public float health = 100f;
	public float walkSpeed = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;
	public bool canMove = true;
	public bool isDead = false;


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

		Spawn();
	}

	private void Update()
	{
		if (isDead)
			return;

		if (transform.position.y < -10f)
			KillPlayer();

		_animator.SetBool("isJumped", !IsGrounded());

		if (!canMove)
			return;

		if (Input.GetKeyDown(KeyCode.Space) && !_cooldownManager.IsInCooldown("jump"))
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
				Debug.Log(hit.transform.name);

				PlatformEffector2D platformEffector = hit.transform.GetComponent<PlatformEffector2D>();

				if (platformEffector)
				{
					platformEffector.rotationalOffset = 180;
					StartCoroutine(PlatformerReset(platformEffector));
				}
			}
		}

		if (IsGrounded())
			_animator.SetFloat("walkSpeed", Mathf.Abs(_rigibody2d.velocity.x) > 0 ? 2 : 0);
	}

	private void FixedUpdate()
	{
		if (!canMove)
			return;
		if (isDead)
			return;

		float horizontal = Input.GetAxis("Horizontal");

		Vector2 targetVelocity = new Vector2(horizontal * walkSpeed, _rigibody2d.velocity.y);

		if (CanJump())
			Jump(ref targetVelocity);

		_rigibody2d.velocity = targetVelocity;
	}
	#endregion

	#region EVENTS 
	public void OnPlayerFacingDirectionChange(bool facingLeft)
	{
		_sprite.flipX = facingLeft;
		_cameraController.CameraOffset = facingLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

		GameManager.Instance.OnPlayerFacingDirectionChange(facingLeft);
	}

	public void OnPlayerEnterDoor()
	{
		GameManager.Instance.OnPlayerEnterDoor();

		canMove = false;
		_animator.SetFloat("walkSpeed", 0);
		_animator.SetBool("isJumping", false);
	}
	#endregion

	#region FUNCTIONS 
	IEnumerator PlatformerReset(PlatformEffector2D platformEffector)
	{
		yield return new WaitForSeconds(0.5f);
		platformEffector.rotationalOffset = 0;
	}

	void Spawn()
	{
		if (isDead)
		{
			_animator.SetBool("isDead", false);
			_animator.SetInteger("damageType", 0);
			isDead = false;
			health = 100f;
		}

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();

		_rigibody2d.velocity = Vector2.zero;
		_rigibody2d.angularVelocity = 0;

		gameObject.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		if (_spawnedEffect != null)
			StopCoroutine(_spawnedEffect);

		_spawnedEffect = StartCoroutine(SpawnedEffect());

		FadeEffect.Instance.FadeOut(() =>
		{
			//if (isDead)
			//	CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, false);
			canMove = true;

			CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, true);
		});

		GameManager.Instance.OnPlayerSpawn();

		Debug.Log("Player has spawned.");
	}

	public IEnumerator SpawnedEffect()
	{
		for (int i = 0; i < 5; i++)
		{
			GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
			yield return new WaitForSeconds(0.2f);
			GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void Jump(ref Vector2 targetVelocity)
	{
		targetVelocity.y = jumpForce;
		_hasPressedJump = false;
		_cooldownManager.SetCooldown("jump", jumpCooldown);

		GameManager.Instance.OnPlayerJump();
	}

	public bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, TerrainLayer);

		return hit.transform != null ? true : false;
	}

	public bool CanJump()
	{
		return IsGrounded() ? (!_cooldownManager.IsInCooldown("jump")) && _hasPressedJump : false;
	}

	public void TakeDamage(float damage, GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || !canMove)
			return;

		health -= damage;

		if (health <= 0)
			KillPlayer(damageType);

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();

		GameManager.Instance.OnPlayerTakeDamage(damage, damageType);
	}

	public void KillPlayer(GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || !canMove)
			return;

		Debug.Log("Player has dead.");

		CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, false);
		CanvasManager.Instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, true);

		isDead = true;
		canMove = false;

		_animator.SetFloat("walkSpeed", 0);
		_animator.SetBool("isJumping", false);
		_animator.SetBool("isDead", true);

		_animator.SetInteger("damageType", (int)damageType);

		ResetToDefaults();

		FadeEffect.Instance.FadeIn(() =>
		{
			Spawn();
		});

		GameManager.Instance.OnPlayerDead(damageType);
	}

	public void ResetToDefaults()
	{
		health = 100f;
		walkSpeed = 5f;
		jumpForce = 5.5f;
		jumpCooldown = 1;

		GameManager.Instance.OnPlayerReset();
	}
	#endregion
}
