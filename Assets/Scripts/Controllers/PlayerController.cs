using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
	public float walkSpeed = 5f;
	public float jumpSpeed = 5f;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");//, vertical = Input.GetAxis("Vertical");

        Vector2 toMove = new Vector2(horizontal * walkSpeed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space))
            toMove.y = jumpSpeed * 1f;
        rb.velocity = toMove;
    }
}
