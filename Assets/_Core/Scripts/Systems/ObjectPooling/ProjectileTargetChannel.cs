using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/ProjectileTargetChannel", fileName = "ProjectileTargetChannel")]
public class ProjectileTargetChannel : ScriptableObject, IDisposable
{
	private Dictionary<Alignment, List<ProjectileTarget>> _projectileTargets = new Dictionary<Alignment, List<ProjectileTarget>>();

	public IReadOnlyList<ProjectileTarget> GetProjectileTargets(Alignment alignment)
	{
		if(_projectileTargets.TryGetValue(alignment, out List<ProjectileTarget> list))
		{
			return list;
		}
		return new List<ProjectileTarget>(0);
	}

	public void Register(ProjectileTarget target)
	{
		if(!_projectileTargets.TryGetValue(target.Alignment, out List<ProjectileTarget> list))
		{
			_projectileTargets[target.Alignment] = list = new List<ProjectileTarget>();
		}

		if(!list.Contains(target))
		{
			list.Add(target);
		}
	}

	public void Unregister(ProjectileTarget target)
	{
		if (_projectileTargets.TryGetValue(target.Alignment, out List<ProjectileTarget> list))
		{
			list.Remove(target);
		}
	}

	public void Dispose()
	{
		_projectileTargets.Clear();
	}
}