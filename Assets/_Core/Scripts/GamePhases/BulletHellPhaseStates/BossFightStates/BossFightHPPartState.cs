using System;
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

		StateParent.BossInstance.ProjectileTarget.OnCollisionEnter += OnBossHitEvent;
	}

	protected override void OnExit()
	{
		_healthPart.Kill();

		if(StateParent != null && StateParent.BossInstance != null && StateParent.BossInstance.ProjectileTarget != null)
		{
			StateParent.BossInstance.ProjectileTarget.OnCollisionEnter -= OnBossHitEvent;
		}
	}

	public override void Deinitialize()
	{
		if (StateParent != null && StateParent.BossInstance != null)
		{
			StateParent.BossHealth.RemoveHealthPart(_healthPart);
		}
		base.Deinitialize();
	}

	private void OnBossHitEvent(Projectile projectile, ProjectileTarget target)
	{
		_healthPart.Damage(1);
	}
}
