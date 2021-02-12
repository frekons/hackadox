using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Camera mainCamera;
	private Rigidbody2D rigibody2d;

	[Header("Ground Layer Mask")]
	public LayerMask terrainLayer;

	[Header("Player")]
	public float walkForce = 5f;
	public float jumpForce = 5f;
	public float jumpCooldown = 0.1f;
	public bool canMove = true;

	private bool hasPressedJump;
	private float lastJump;

	void Start()
	{
		canMove = true;
		rigibody2d = GetComponent<Rigidbody2D>();


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
		if (!canMove)
			return;

		if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastJump > jumpCooldown)
			hasPressedJump = true;
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
		lastJump = Time.time;
	}
}
