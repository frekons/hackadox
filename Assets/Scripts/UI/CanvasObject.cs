using UnityEngine;

public class CanvasObject : MonoBehaviour
{
	public CanvasManager.CanvasNames canvasName;

	private void Awake()
	{
		CanvasManager.CanvasList.Add(canvasName, GetComponent<Canvas>());
	}
}
