using System.Collections.Generic;
using UnityEngine;

public class CooldownManager
{
	private Dictionary<string, float> cooldownList = new Dictionary<string, float>();

	public void SetCooldown(string key, float time)
	{
		if (cooldownList.ContainsKey(key))
			cooldownList[key] = Time.time + time;
		else
			cooldownList.Add(key, Time.time + time);
	}

	public void RemoveCooldown(string key)
	{
		cooldownList.Remove(key);
	}

	public float GetCooldown(string key)
	{
		if (!cooldownList.ContainsKey(key))
			return 0;

		return Time.time - cooldownList[key];
	}

	public bool IsInCooldown(string key)
	{
		if (!cooldownList.ContainsKey(key))
			return false;

		bool isIn = Time.time - cooldownList[key] <= 0;

		if (!isIn)
			RemoveCooldown(key);

		return isIn;
	}
}
