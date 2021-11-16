using UnityEngine;

public class BossShip : MonoBehaviour
{
	[SerializeField]
	private ProjectileTarget _projectileTarget = null;
	[SerializeField]
	private ProjectilesEmitterSystemGroup _mainGuns = null;

	public ProjectileTarget ProjectileTarget => _projectileTarget;
	public ProjectilesEmitterSystemGroup MainGuns => _mainGuns;
}
