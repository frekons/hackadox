using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager Instance;

	public enum CanvasNames
	{
		MainScreen,
		GameScreen,
		DeathScreen // olacak mı belli değil
	}
	public static Dictionary<CanvasNames, Canvas> CanvasList = new Dictionary<CanvasNames, Canvas>();

	private void OnEnable()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			throw new System.Exception("More than one instance of singleton detected.");
		}

		Instance = this;
	}

	public void SetCanvasVisibility(CanvasNames canvasName, bool state)
	{
		if (CanvasList.ContainsKey(canvasName))
			CanvasList[canvasName].enabled = state;
		else
			Debug.LogError(canvasName.ToString() + " is null.");
	}
}
