using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVariableScript : MonoBehaviour
{
    private PlayerSt player = new PlayerSt();

    public void AddVariable()
    {
        ConsolePanel.Instance.AddVariable("test", player, typeof(PlayerSt));
    }

    [System.Serializable]
    public class PlayerSt
    {
        public int health = 100;
        public float gravity = 800;
    }
}
