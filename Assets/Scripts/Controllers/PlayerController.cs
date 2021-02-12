﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Camera mainCamera;
	private Rigidbody2D rigibody2d;

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
	private float lastJump;

	void Start()
	{
		rigibody2d = GetComponent<Rigidbody2D>();

		Spawn();
	}

	public void TakeDamage(float damage)
	{
		if (isDead || !canMove)
			return;

		health -= damage;

		if (health <= 0)
			OnPlayerDead();

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
		rigibody2d.velocity = Vector2.zero;
		rigibody2d.angularVelocity = 0;

		gameObject.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);

		canMove = true;

		FadeEffect.instance.FadeOut(() =>
		{
			if (isDead)
			{
				isDead = false;
				CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.DeathScreen, false);
			}

			CanvasManager.instance.SetCanvasVisibility(CanvasManager.CanvasNames.GameScreen, true);

			health = 100f;
		});

		Debug.Log("Player has spawned.");
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
		if (transform.position.y < -10f)
			TakeDamage(100f);

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
