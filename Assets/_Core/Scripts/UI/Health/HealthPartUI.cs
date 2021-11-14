using UnityEngine;
using UnityEngine.UI;

public class HealthPartUI : MonoBehaviour
{
	#region Editor Variables

	[SerializeField]
	private Slider _slider = null;

	#endregion

	#region Properties

	public HealthPart HealthPart
	{
		get; private set;
	}

	#endregion

	#region Lifecycle

	protected void Awake()
	{
		if (HealthPart == null)
		{
			SetDisplayingHealthPart(null);
		}
	}

	#endregion

	#region Public Methods

	public void SetDisplayingHealthPart(HealthPart healthPart)
	{
		if(HealthPart != null)
		{
			HealthPart.HealthChangedEvent -= OnHealthChangedEvent;
			HealthPart = null;
		}

		HealthPart = healthPart;

		if (HealthPart != null)
		{
			HealthPart.HealthChangedEvent += OnHealthChangedEvent;

			_slider.minValue = 0;
			_slider.maxValue = HealthPart.MaxHP;
			SetValue(healthPart.HP);
		}
		else
		{
			_slider.minValue = 0;
			_slider.maxValue = 1;
			SetValue(0);
		}
	}

	private void OnHealthChangedEvent(HealthPart healthPart)
	{
		SetValue(healthPart.HP);
	}

	#endregion

	#region Private Methods

	private void SetValue(float v)
	{
		_slider.value = v;
	}

	#endregion
}
