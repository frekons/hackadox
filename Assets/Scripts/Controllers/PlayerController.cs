using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CameraController cameraController;
	private Rigidbody2D rigibody2d;
	private Animator animator;
	private SpriteRenderer sprite;
	private CooldownManager cooldownManager = new CooldownManager();
	private Coroutine spawnedEffect;

	[Header("Ground Layer Mask")]
	public LayerMask terrainLayer;

	#region PLAYER FIELDS
	[Header("Player")]
	public float health = 100f;
	public float walkSpeed = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;
	public bool canMove = true;
	public bool isDead = false;
	#endregion

	private bool hasPressedJump;
	private bool _facingLeft = false;
	public bool facingLeft
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

	#region UNITY FUNCTIONS
	void Start()
	{
		rigibody2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		cameraController = Camera.main.GetComponent<CameraController>();

		Spawn();
	}

	private void Update()
	{
		if (isDead)
			return;

		if (transform.position.y < -10f)
			KillPlayer();

		animator.SetBool("isJumped", !IsGrounded());

		if (!canMove)
			return;

		if (Input.GetKeyDown(KeyCode.Space) && !cooldownManager.IsInCooldown("jump"))
			hasPressedJump = true;

		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
			facingLeft = true;
		else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
			facingLeft = false;

		if (IsGrounded())
			animator.SetFloat("walkSpeed", Mathf.Abs(rigibody2d.velocity.x) > 0 ? 2 : 0);
	}

	private void FixedUpdate()
	{
		if (!canMove)
			return;
		if (isDead)
			return;

		float horizontal = Input.GetAxis("Horizontal");

		Vector2 targetVelocity = new Vector2(horizontal * walkSpeed, rigibody2d.velocity.y);

		if (CanJump())
			Jump(ref targetVelocity);

		rigibody2d.velocity = targetVelocity;
	}
	#endregion

	#region EVENTS 
	public void OnPlayerFacingDirectionChange(bool facingLeft)
	{
		sprite.flipX = facingLeft;
		cameraController.cameraOffset = facingLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);

		GameManager.instance.OnPlayerFacingDirectionChange(facingLeft);
	}

	public void OnPlayerEnterDoor()
	{
		GameManager.instance.OnPlayerEnterDoor();

		canMove = false;
		animator.SetFloat("walkSpeed", 0);
		animator.SetBool("isJumping", false);
	}
	#endregion

	#region FUNCTIONS 
	void Spawn()
	{
		if (isDead)
		{
			animator.SetBool("isDead", false);
			animator.SetInteger("damageType", 0);
			isDead = false;
			health = 100f;
		}

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();

		rigibody2d.velocity = Vector2.zero;
		rigibody2d.angularVelocity = 0;

		gameObject.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		if (spawnedEffect != null)
			StopCoroutine(spawnedEffect);

		spawnedEffect = StartCoroutine(SpawnedEffect());

		FadeEffect.instance.FadeOut(() =>
		{
			//if (isDead)
			//	CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, false);
			canMove = true;

			CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, true);
		});

		GameManager.instance.OnPlayerSpawn();

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
		targetVelocity.y += jumpForce;
		hasPressedJump = false;
		cooldownManager.SetCooldown("jump", jumpCooldown);

		GameManager.instance.OnPlayerJump();
	}

	public bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, terrainLayer);

		return hit.transform != null ? true : false;
	}

	public bool CanJump()
	{
		return IsGrounded() ? (!cooldownManager.IsInCooldown("jump")) && hasPressedJump : false;
	}

	public void TakeDamage(float damage, GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || !canMove)
			return;

		health -= damage;

		if (health <= 0)
			KillPlayer(damageType);

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();

		GameManager.instance.OnPlayerTakeDamage(damage, damageType);
	}

	public void KillPlayer(GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || !canMove)
			return;

		Debug.Log("Player has dead.");

		CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, false);
		CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, true);

		isDead = true;
		canMove = false;

		animator.SetFloat("walkSpeed", 0);
		animator.SetBool("isJumping", false);
		animator.SetBool("isDead", true);

		animator.SetInteger("damageType", (int)damageType);

		ResetToDefaults();

		FadeEffect.instance.FadeIn(() =>
		{
			Spawn();
		});

		GameManager.instance.OnPlayerDead(damageType);
	}

	public void ResetToDefaults()
	{
		health = 100f;
		walkSpeed = 5f;
		jumpForce = 5f;
		jumpCooldown = 0.1f;

		GameManager.instance.OnPlayerReset();
	}
	#endregion
}
