using System;
using UnityEngine;

public class HealthPart
{
	public event Action<HealthPart> HealthChangedEvent;

	private int _hp = 0;

	public int HP
	{
		get => _hp;
		private set
		{
			_hp = Mathf.Clamp(value, 0, MaxHP);
			HealthChangedEvent?.Invoke(this);
		}
	}

	public int MaxHP
	{
		get; private set;
	}

	public HealthPart(int hp)
	{
		MaxHP = hp;
		Refresh();
	}

	public void Refresh()
	{
		HP = MaxHP;
	}

	public void Damage(int amount)
	{
		if(amount <= 0)
		{
			return;
		}

		HP -= amount;
	}

	public void Heal(int amount)
	{
		if (amount <= 0)
		{
			return;
		}

		HP += amount;
	}

	public void Kill()
	{
		HP = 0;
	}
}