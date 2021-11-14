using System;
using System.Collections.Generic;

public class Health
{
	public event Action<Health, HealthPart> HealthPartAddedEvent;
	public event Action<Health, HealthPart> HealthPartRemovedEvent;

	private List<HealthPart> _healthParts = new List<HealthPart>();

	public IReadOnlyList<HealthPart> HealthParts => _healthParts;


	#region Public Methods

	public void AddHealthPart(HealthPart healthPart)
	{
		if (!_healthParts.Contains(healthPart))
		{
			_healthParts.Add(healthPart);
			HealthPartAddedEvent?.Invoke(this, healthPart);
		}
	}

	public void RemoveHealthPart(HealthPart healthPart)
	{
		if(_healthParts.Remove(healthPart))
		{
			HealthPartRemovedEvent?.Invoke(this, healthPart);
		}
	}

	public void ClearParts()
	{
		for (int i = _healthParts.Count - 1; i >= 0; i--)
		{
			RemoveHealthPart(_healthParts[i]);
		}
	}

	public void Refresh()
	{
		for (int i = 0; i < _healthParts.Count; i++)
		{
			_healthParts[i].Refresh();
		}
	}

	public void Kill()
	{
		for (int i = 0; i < _healthParts.Count; i++)
		{
			_healthParts[i].Kill();
		}
	}

	public int GetTotalHealth()
	{
		int hp = 0;
		for(int i = 0; i < _healthParts.Count; i++)
		{
			hp += _healthParts[i].HP;
		}
		return hp;
	}

	public int GetTotalMaxHealth()
	{
		int maxHP = 0;
		for (int i = 0; i < _healthParts.Count; i++)
		{
			maxHP += _healthParts[i].MaxHP;
		}
		return maxHP;
	}

	#endregion
}
