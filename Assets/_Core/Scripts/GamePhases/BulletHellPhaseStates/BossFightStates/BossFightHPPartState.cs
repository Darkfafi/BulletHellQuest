using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightHPPartState : BossFightStateBase
{
	#region Editor Variables

	[SerializeField]
	private int _healthAmount = 100;

	#endregion

	#region Variables

	private HealthPart _healthPart = null;

	#endregion

	public override void Initialize(BulletHellBossFightState parent)
	{
		base.Initialize(parent);

		_healthPart = new HealthPart(_healthAmount);
		parent.BossHealth.AddHealthPart(_healthPart);
	}

	protected override void OnEnter()
	{
		_healthPart.Refresh();
	}

	protected override void OnExit()
	{
		_healthPart.Kill();
	}

	public override void Deinitialize()
	{
		if (StateParent != null && StateParent.BossInstance != null)
		{
			StateParent.BossHealth.RemoveHealthPart(_healthPart);
		}
		base.Deinitialize();
	}
}
