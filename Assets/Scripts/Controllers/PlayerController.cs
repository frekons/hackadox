using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Camera mainCamera;
	private Rigidbody2D rigibody2d;
	private Animator animator;
	private SpriteRenderer sprite;
	private CooldownManager cooldownManager = new CooldownManager();

	[Header("Ground Layer Mask")]
	public LayerMask terrainLayer;

	[Header("Player")]
	public float health = 100f;
	public float walkForce = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;
	public bool canMove = true;
	public bool isDead = false;
	public bool facingLeft = false;

	private bool hasPressedJump;

	void Start()
	{
		rigibody2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();

		Spawn();
	}

	public void TakeDamage(float damage)
	{
		if (isDead || !canMove)
			return;

		health -= damage;

		if (health <= 0)
			OnPlayerDead();

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();
	}

	public void OnPlayerDead()
	{
		if (isDead || !canMove)
			return;

		Debug.Log("Player has dead.");

		CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, false);
		CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, true);

		isDead = true;
		canMove = false;

		FadeEffect.instance.FadeIn(() =>
		{
			Spawn();
		});
	}

	void Spawn()
	{
		health = 100f;

		GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = health.ToString();

		rigibody2d.velocity = Vector2.zero;
		rigibody2d.angularVelocity = 0;

		gameObject.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		canMove = true;

		FadeEffect.instance.FadeOut(() =>
		{
			if (isDead)
			{
				isDead = false;
				CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, false);
			}

			CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, true);
		});

		Debug.Log("Player has spawned.");
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
		if (transform.position.y < -10f)
			TakeDamage(100f);

		animator.SetBool("isJumped", !IsGrounded());

		if (!canMove)
			return;

		if (Input.GetKeyDown(KeyCode.Space) && !cooldownManager.IsInCooldown("jump"))
			hasPressedJump = true;

		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
			facingLeft = true;
		else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
			facingLeft = false;

		sprite.flipX = facingLeft;
		Debug.Log(facingLeft);
	}

	private void FixedUpdate()
	{
		if (!canMove)
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
