using System.Collections;
using UnityEngine;

public class BossFightHPPartState : BossFightStateBase, IBossFightStateParent
{
	#region Editor Variables

	[SerializeField]
	private int _healthAmount = 100;

	#endregion

	#region Variables

	private HealthPart _healthPart = null;
	private FiniteStateMachine<IBossFightStateParent> _fsm;

	public BossShip BossInstance => StateParent.BossInstance;
	public Health BossHealth => StateParent.BossHealth;

	#endregion

	public override void Initialize(IBossFightStateParent parent)
	{
		base.Initialize(parent);

		_healthPart = new HealthPart(_healthAmount);
		parent.BossHealth.AddHealthPart(_healthPart);

		_fsm = new FiniteStateMachine<IBossFightStateParent>(this, transform.GetStates<IBossFightStateParent>(), false);
	}

	public void GoToNextState()
	{
		if (!_fsm.GoToNextState())
		{
			_fsm.StartStateMachine();
		}
	}

	protected override void OnEnter()
	{
		_healthPart.Refresh();

		StateParent.BossInstance.ProjectileTarget.OnCollisionEnter += OnBossHitEvent;

		_fsm.StartStateMachine();
	}

	protected override void OnExit()
	{
		_fsm.StopStateMachine();

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

		if(_healthPart.HP == 0)
		{
			StateParent.GoToNextState();
		}
	}
}
