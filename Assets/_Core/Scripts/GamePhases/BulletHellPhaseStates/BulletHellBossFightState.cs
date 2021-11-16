using UnityEngine;

public class BulletHellBossFightState : BulletHellPhaseStateBase, IBossFightStateParent
{
	#region Editor Variables

	[Header("Requirements")]
	[SerializeField]
	private Transform _bossSpawnPoint = null;

	[SerializeField]
	private BossShip _bossPrefab = null;

	[SerializeField]
	private HealthUI _healthUI = null;

	#endregion

	#region Variables

	private FiniteStateMachine<IBossFightStateParent> _fsm = null;

	#endregion

	#region Properties

	public BossShip BossInstance
	{
		get; private set;
	}

	public Health BossHealth
	{
		get; private set;
	}

	#endregion

	public void GoToNextState()
	{
		_fsm.GoToNextState();

		if(_fsm.CurrentState == null)
		{
			StateParent.GoToNextState();
		}
	}

	public override void Initialize(BulletHellGamePhaseState parent)
	{
		base.Initialize(parent);
		BossHealth = new Health();

		_fsm = new FiniteStateMachine<IBossFightStateParent>(this, transform.GetStates<IBossFightStateParent>(), false);
	}

	protected override void OnEnter()
	{
		SpawnBoss();
		BossHealth.Refresh();

		_healthUI.SetDisplayingHealth(BossHealth);

		_fsm.StartStateMachine();
	}

	protected override void OnExit()
	{
		_healthUI.SetDisplayingHealth(null);

		_fsm.StopStateMachine();
		DestroyBoss();
	}

	public override void Deinitialize()
	{
		_fsm.Dispose();
		BossHealth.ClearParts();
		BossHealth = null;
		base.Deinitialize();
	}

	private void SpawnBoss()
	{
		DestroyBoss();
		BossInstance = Instantiate(_bossPrefab, _bossSpawnPoint != null ? _bossSpawnPoint.position : Vector3.zero, _bossSpawnPoint != null ? _bossSpawnPoint.rotation : Quaternion.identity);
	}

	private void DestroyBoss()
	{
		if (BossInstance != null)
		{
			Destroy(BossInstance.gameObject);
			BossInstance = null;
		}
	}
}
