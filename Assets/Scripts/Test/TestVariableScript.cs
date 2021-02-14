using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisibleAttributes
{
	public string Name;
	public bool IsVisible;
}

public class TestVariableScript : MonoBehaviour
{
	public List<VisibleAttributes> VisibleAttributesList = new List<VisibleAttributes>();

	private Dictionary<string, bool> VisibleAttributesDict = new Dictionary<string, bool>();

	public PlayerSt player = new PlayerSt();

	private void Start()
	{
		foreach (var attr in VisibleAttributesList)
		{
			VisibleAttributesDict.Add(attr.Name, attr.IsVisible);
		}

	}

	public void AddVariable()
	{
		ConsolePanel.Instance.AddVariable("player", GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Player, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().VisibleAttributesDict);
	}

	[System.Serializable]
	public class PlayerSt
	{
		public int _health = 100;
		public float _gravity = 800;

		public int health
		{
			get
			{
				return _health;
			}

			set
			{
				Debug.Log("set health called!");

				_health = value;
			}
		}

		public float gravity
		{
			get
			{
				return _gravity;
			}

			set
			{
				Debug.Log("set gravity called!");

				_gravity = value;
			}
		}
	}
}
