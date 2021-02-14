using UnityEngine;

public class MinigameTwo : MonoBehaviour
{
	[SerializeField]
	private GameObject _movingObject;

	[SerializeField]
	private float _movingObjectSpeed = 5f;

	private Resolution resolution;

	private void Start()
	{
		resolution = Screen.currentResolution;

		_movingObject.transform.position = new Vector3(0, resolution.height / 2, 0);
	}

	void Update()
	{
		resolution = Screen.currentResolution;

		Vector3 newObjectPosition = _movingObject.transform.position;

		if (newObjectPosition.x - (_movingObject.GetComponent<RectTransform>().rect.width / 2) > resolution.width)
		{
			newObjectPosition.x = -(resolution.width / 2) + resolution.width / 2;
			newObjectPosition.x -= (_movingObject.GetComponent<RectTransform>().rect.width / 2);

			_movingObject.transform.position = newObjectPosition;
			return;
		}

		_movingObject.transform.position = Vector3.Lerp(_movingObject.transform.position, newObjectPosition + new Vector3(_movingObjectSpeed, 0, 0), Time.deltaTime * 1);
	}
}
