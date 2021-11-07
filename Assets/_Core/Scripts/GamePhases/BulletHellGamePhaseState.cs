using UnityEngine;

public class BulletHellGamePhaseState : GamePhaseStateBase
{
	#region Editor Variables

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
		ShipInstance = Instantiate(_shipPrefab);
	}

	protected override void OnExit()
	{
		if (ShipInstance != null)
		{
			Destroy(ShipInstance);
			ShipInstance = null;
		}
	}
}
