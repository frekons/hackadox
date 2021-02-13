using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Camera")]
	private GameObject localPlayer;
	public Vector3 cameraOffset = Vector3.zero;
	public float followSpeed = 1f;

	void Start()
	{
		localPlayer = GameObject.FindWithTag("Player");
	}

	void LateUpdate()
	{
		Vector3 _camPos = localPlayer.transform.position;
		_camPos.z = transform.position.z;

		//transform.position = Vector3.Lerp(transform.position, _camPos + cameraOffset, Time.deltaTime * followSpeed);
		transform.position = _camPos;
	}
}
