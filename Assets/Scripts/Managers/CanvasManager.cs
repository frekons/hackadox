using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager instance;

	public enum CanvasNames
	{
		MainScreen,
		GameScreen,
		DeathScreen // olacak mı belli değilsa
	}
	public static Dictionary<CanvasNames, Canvas> canvasList = new Dictionary<CanvasNames, Canvas>();

	void Awake()
	{
		instance = this;
	}

	public void SetCanvasVisibility(CanvasNames canvasName, bool state)
	{
		if (canvasList.ContainsKey(canvasName))
			canvasList[canvasName].enabled = state;
		else
			Debug.LogError(canvasName.ToString() + " is null.");
	}
}
