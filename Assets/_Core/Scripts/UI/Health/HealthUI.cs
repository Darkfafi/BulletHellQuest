using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
	#region Editor Variables

	[SerializeField]
	private HealthPartUI _healthPartUIPrefab = null;

	[SerializeField]
	private GameObject _displayContainer = null;

	#endregion

	#region Variables

	private Health _health = null;
	private List<HealthPartUI> _healthPartUIInstances = new List<HealthPartUI>();

	#endregion

	#region Lifecycle

	protected void Awake()
	{
		if (_health == null)
		{
			SetDisplayingHealth(null);
		}
	}

	#endregion

	#region Public Methods

	public void SetDisplayingHealth(Health health)
	{
		if(_health != null)
		{
			_health.HealthPartAddedEvent -= OnHealthPartAddedEvent;
			_health.HealthPartRemovedEvent -= OnHealthPartRemovedEvent;

			RemoveAllHealthParts();

			_health = null;
		}

		_health = health;

		if (_health != null)
		{
			_health.HealthPartAddedEvent += OnHealthPartAddedEvent;
			_health.HealthPartRemovedEvent += OnHealthPartRemovedEvent;

			for(int i = 0; i < _health.HealthParts.Count; i++)
			{
				AddHealthPart(_health.HealthParts[i]);
			}
		}

		if (_displayContainer != null)
		{
			_displayContainer.SetActive(_health != null);
		}
	}

	#endregion

	#region Private Methods

	private void AddHealthPart(HealthPart healthPart)
	{
		HealthPartUI healthPartUI = Instantiate(_healthPartUIPrefab, _displayContainer.transform, false);
		healthPartUI.SetDisplayingHealthPart(healthPart);
		_healthPartUIInstances.Add(healthPartUI);
	}

	private void RemoveHealthPart(HealthPart healthPart)
	{
		for (int i = _healthPartUIInstances.Count - 1; i >= 0; i--)
		{
			HealthPartUI healthPartUI = _healthPartUIInstances[i];
			if (healthPartUI != null && healthPartUI.HealthPart == healthPart)
			{
				healthPartUI.SetDisplayingHealthPart(null);
				_healthPartUIInstances.Remove(healthPartUI);
				Destroy(healthPartUI.gameObject);
				return;
			}
		}
	}
	
	private void RemoveAllHealthParts()
	{
		for (int i = _healthPartUIInstances.Count - 1; i >= 0; i--)
		{
			HealthPartUI healthPartUI = _healthPartUIInstances[i];
			if (healthPartUI != null)
			{
				healthPartUI.SetDisplayingHealthPart(null);
				Destroy(healthPartUI.gameObject);
			}
		}
		_healthPartUIInstances.Clear();
	}

	private void OnHealthPartRemovedEvent(Health hp, HealthPart hpPart)
	{
		RemoveHealthPart(hpPart);
	}

	private void OnHealthPartAddedEvent(Health hp, HealthPart hpPart)
	{
		AddHealthPart(hpPart);
	}

	#endregion
}
