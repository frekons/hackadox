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

	[Header("Player")]
	public float health = 100f;
	public float walkForce = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;
	public bool canMove = true;
	public bool isDead = false;

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
				OnFacingDirectionChange(value);

			_facingLeft = value;
		}
	}


	void Start()
	{
		rigibody2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		cameraController = Camera.main.GetComponent<CameraController>();

		Spawn();
	}

	public void TakeDamage(float damage, GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
	{
		if (isDead || !canMove)
			return;

		health -= damage;

		if (health <= 0)
			OnPlayerDead(damageType);

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();
	}

	public void OnPlayerDead(GameManager.DamageTypes damageType = GameManager.DamageTypes.Suicide)
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

		FadeEffect.instance.FadeIn(() =>
		{
			Spawn();
		});
	}

	public void OnFacingDirectionChange(bool facingLeft)
	{
		sprite.flipX = facingLeft;
		cameraController.cameraOffset = facingLeft ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
	}

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

	public bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, terrainLayer);

		return hit.transform != null ? true : false;
	}

	public bool CanJump()
	{
		return IsGrounded() ? (!cooldownManager.IsInCooldown("jump")) && hasPressedJump : false;
	}

	private void Update()
	{
		if (isDead)
			return;

		if (transform.position.y < -10f)
			OnPlayerDead();

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

		Vector2 targetVelocity = new Vector2(horizontal * walkForce, rigibody2d.velocity.y);

		if (CanJump())
			Jump(ref targetVelocity);

		rigibody2d.velocity = targetVelocity;
	}

	private void Jump(ref Vector2 targetVelocity)
	{
		targetVelocity.y += jumpForce;
		hasPressedJump = false;
		cooldownManager.SetCooldown("jump", jumpCooldown);
	}
}
