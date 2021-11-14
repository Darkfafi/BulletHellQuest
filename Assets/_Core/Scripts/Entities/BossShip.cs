using UnityEngine;

public class BossShip : MonoBehaviour
{
	[SerializeField]
	private ProjectileTarget _projectileTarget = null;

	public ProjectileTarget ProjectileTarget => _projectileTarget;
}
