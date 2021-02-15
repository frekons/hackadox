using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasObject : MonoBehaviour
{
	public CanvasManager.CanvasNames canvasName;

	private void Awake()
	{
		var canvas = GetComponent<Canvas>();

		if (!CanvasManager.CanvasList.ContainsKey(canvasName))
		{
			CanvasManager.CanvasList.Add(canvasName, canvas);
		}
		else
		{
			CanvasManager.CanvasList[canvasName] = canvas;
		}
	}

}
