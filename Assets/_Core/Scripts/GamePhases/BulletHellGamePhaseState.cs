using UnityEngine;

public class BulletHellGamePhaseState : GamePhaseStateBase, IStatesParent
{
	#region Editor Variables

	[Header("Requirements")]
	[SerializeField]
	private Transform _shipSpawnPoint = null;

	[SerializeField]
	private BulletHellShip _shipPrefab = null;

	#endregion

	#region Variables

	private FiniteStateMachine<BulletHellGamePhaseState> _fsm;

	#endregion

	#region Properties

	public BulletHellShip ShipInstance
	{
		get; private set;
	}

	#endregion

	public override void Initialize(GameManager parent)
	{
		base.Initialize(parent);
		_fsm = new FiniteStateMachine<BulletHellGamePhaseState>(this, transform.GetStates<BulletHellGamePhaseState>(), false);
	}

	protected override void OnEnter()
	{
		SpawnShip();
		_fsm.StartStateMachine();
	}

	protected override void OnExit()
	{
		_fsm.StopStateMachine();
		DestroyShip();
	}

	public override void Deinitialize()
	{
		_fsm.Dispose();
		base.Deinitialize();
	}

	private void SpawnShip()
	{
		DestroyShip();
		ShipInstance = Instantiate(_shipPrefab, _shipSpawnPoint != null ? _shipSpawnPoint.position : Vector3.zero, _shipSpawnPoint != null ? _shipSpawnPoint.rotation : Quaternion.identity);
	}

	private void DestroyShip()
	{
		if (ShipInstance != null)
		{
			Destroy(ShipInstance.gameObject);
			ShipInstance = null;
		}
	}
}
