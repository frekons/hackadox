using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager instance;

	public enum CanvasNames
	{
		MainScreen,
		GameScreen,
		DeathScreen // olacak mı belli değil
	}
	private Dictionary<CanvasNames, Canvas> canvasList = new Dictionary<CanvasNames, Canvas>();

	void Awake()
	{
		instance = this;

		canvasList.Add(CanvasNames.MainScreen, GameObject.Find("Main Canvas")?.GetComponent<Canvas>());
		canvasList.Add(CanvasNames.GameScreen, GameObject.Find("Game Canvas")?.GetComponent<Canvas>());
		canvasList.Add(CanvasNames.DeathScreen, GameObject.Find("Death Canvas")?.GetComponent<Canvas>());
	}

	public void SetCanvasVisibility(CanvasNames canvasName, bool state)
	{
		if (canvasList[canvasName])
			canvasList[canvasName].enabled = state;
		else
			Debug.LogError(canvasName.ToString() + " is null.");
	}
}
