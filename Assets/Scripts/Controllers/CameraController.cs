using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Camera")]
	private Rigidbody2D localPlayerRb;
	public Vector3 cameraOffset = Vector3.zero;
	public float followSpeed = 1f;

	void Start()
	{
		localPlayerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
	}

	void LateUpdate()
	{
		Vector3 _camPos = localPlayerRb.transform.position;
		_camPos.z = transform.position.z;

		transform.position = Vector3.Lerp(transform.position, _camPos + cameraOffset + new Vector3(localPlayerRb.velocity.x / 2, 0, 0), Time.deltaTime * followSpeed);
	}
}
