using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Camera mainCamera;
	private Rigidbody2D rb;

	public LayerMask terrainLayer;

	public float walkForce = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;

	private bool hasPressedJump;
	private float lastJump;

	void Start()
	{
		mainCamera = Camera.main;
		rb = GetComponent<Rigidbody2D>();
	}

	public bool IsGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, terrainLayer);

		return hit.transform != null ? true : false;
	}

	public bool CanJump()
	{
		return IsGrounded() ? (Time.time - lastJump > jumpCooldown) && hasPressedJump : false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastJump > jumpCooldown)
			hasPressedJump = true;

		if (Input.GetKeyDown(KeyCode.A))
			FadeEffect.instance.FadeIn();
		else if (Input.GetKeyDown(KeyCode.B))
			FadeEffect.instance.FadeOut();

	}

	private void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");

		Vector2 targetVelocity = new Vector2(horizontal * walkForce, rb.velocity.y);

		if (CanJump())
			Jump(ref targetVelocity);

		rb.velocity = targetVelocity;
	}

	private void Jump(ref Vector2 targetVelocity)
	{
		targetVelocity.y += jumpForce;
		hasPressedJump = false;
		lastJump = Time.time;

	}
}
