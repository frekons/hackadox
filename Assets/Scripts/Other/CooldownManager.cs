using System.Collections.Generic;
using UnityEngine;

public class CooldownManager
{
	private Dictionary<string, float> _cooldownList = new Dictionary<string, float>();

	public void SetCooldown(string key, float time)
	{
		if (_cooldownList.ContainsKey(key))
			_cooldownList[key] = Time.time + time;
		else
			_cooldownList.Add(key, Time.time + time);
	}

	public void RemoveCooldown(string key)
	{
		_cooldownList.Remove(key);
	}

	public float GetCooldown(string key)
	{
		if (!_cooldownList.ContainsKey(key))
			return 0;

		return Time.time - _cooldownList[key];
	}

	public bool IsInCooldown(string key)
	{
		if (!_cooldownList.ContainsKey(key))
			return false;

		bool isIn = Time.time - _cooldownList[key] <= 0;

		if (!isIn)
			RemoveCooldown(key);

		return isIn;
	}
}
