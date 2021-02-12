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
                Debug.Log("set called!");

                _health = value;
            }
        }
    }
}
