using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesManager : MonoBehaviour
{
	[SerializeField]
	private ProjectileTargetChannel _projectileTargetChannel = null;

	[SerializeField]
	private ProjectilesPoolChannel[] _poolChannels = new ProjectilesPoolChannel[0];

	[SerializeField]
	private Alignment[] _alignmentsToCheckCollision = new Alignment[0];

	protected void Awake()
	{
		for(int i = 0; i < _poolChannels.Length; i++)
		{
			_poolChannels[i].Init();
		}
	}

	protected void Update()
	{
		for(int i = 0, c = _alignmentsToCheckCollision.Length; i < c; i++)
		{
			Alignment alignment = _alignmentsToCheckCollision[i];
			IReadOnlyList<ProjectileTarget> projectileTargets = _projectileTargetChannel.GetProjectileTargets(alignment);
			for(int j = 0, c2 = projectileTargets.Count; j < c2; j++)
			{
				ProjectileTarget target = projectileTargets[j];
				ForEachProjectile(alignment, (projectile) => 
				{
					if(target.Overlaps(projectile))
					{
						target.ProcessCollisionStep(projectile);
					}
				});
				target.EndCollisionsStep();
			}
		}
	}

	protected void OnDestroy()
	{
		for (int i = _poolChannels.Length - 1; i >= 0; i--)
		{
			_poolChannels[i].Deinit();
		}

		_projectileTargetChannel.Dispose();
	}

	protected void ForEachProjectile(Alignment alignment, Action<Projectile> action)
	{
		for (int i = 0; i < _poolChannels.Length; i++)
		{
			ProjectilesPool pool = _poolChannels[i].ProjectilesPool;
			if (alignment == pool.Prefab.Alignment)
			{
				List<Projectile> projectiles = new List<Projectile>(pool.ClaimedInstances);
				for (int j = 0, c2 = projectiles.Count; j < c2; j++)
				{
					action(projectiles[j]);
				}
			}
		}
	}

	protected void ForEachProjectile(Action<Projectile> action)
	{
		for(int i = 0; i < _poolChannels.Length; i++)
		{
			ProjectilesPool pool = _poolChannels[i].ProjectilesPool;
			List<Projectile> projectiles = new List<Projectile>(pool.ClaimedInstances);
			for (int j = 0, c2 = projectiles.Count; j < c2; j++)
			{
				action(projectiles[j]);
			}
		}
	}
}
