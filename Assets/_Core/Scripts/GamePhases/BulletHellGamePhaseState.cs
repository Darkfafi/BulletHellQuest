using UnityEngine;

public class BulletHellGamePhaseState : GamePhaseStateBase
{
	#region Editor Variables

	[Header("Requirements")]
	[SerializeField]
	private Transform _shipSpawnPoint = null;

	[SerializeField]
	private BulletHellShip _shipPrefab = null;

	#endregion

	#region Properties

	public BulletHellShip ShipInstance
	{
		get; private set;
	}

	#endregion

	protected override void OnEnter()
	{
		SpawnShip();
	}

	protected override void OnExit()
	{
		DestroyShip();
	}

	private void SpawnShip()
	{
		DestroyShip();
		ShipInstance = Instantiate(_shipPrefab, _shipSpawnPoint != null ? _shipSpawnPoint.position : Vector3.zero, Quaternion.identity);
	}

	private void DestroyShip()
	{
		if (ShipInstance != null)
		{
			Destroy(ShipInstance);
			ShipInstance = null;
		}
	}
}
