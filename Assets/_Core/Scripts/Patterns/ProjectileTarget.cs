using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTarget : MonoBehaviour
{
	#region Events

	public event Action<Projectile, ProjectileTarget> OnCollisionEnter;
	public event Action<Projectile, ProjectileTarget> OnCollisionStay;
	public event Action<Projectile, ProjectileTarget> OnCollisionExit;

	#endregion

	#region Editor Variables

	[SerializeField]
	private ProjectileTargetChannel _channel = null;

	[SerializeField]
	private Alignment _alignment = default;

	[SerializeField]
	private float _colliderRadius = 1f;

	#endregion

	#region Variables

	private HashSet<Projectile> _previousCollisions = new HashSet<Projectile>();
	private HashSet<Projectile> _newCollisions = new HashSet<Projectile>();

	#endregion

	#region Properties

	public Alignment Alignment => _alignment;

	public float ColliderRadius => _colliderRadius;

	#endregion

	protected void OnEnable()
	{
		if (_channel != null)
		{
			_channel.Register(this);
		}
	}

	protected void OnDestroy()
	{
		if (_channel != null)
		{
			_channel.Unregister(this);
		}
	}

	public void ProcessCollisionStep(Projectile projectile)
	{
		if (_newCollisions.Add(projectile))
		{
			if (!_previousCollisions.Contains(projectile))
			{
				// On Enter
				OnCollisionEnter?.Invoke(projectile, this);
			}

			// On Stay
			OnCollisionStay?.Invoke(projectile, this);
		}
	}

	public void EndCollisionsStep()
	{
		foreach(Projectile previousProjectile in _previousCollisions)
		{
			if(previousProjectile != null && !_newCollisions.Contains(previousProjectile))
			{
				// On Exit
				OnCollisionExit?.Invoke(previousProjectile, this);
			}
		}
		_previousCollisions = _newCollisions;
		_newCollisions = new HashSet<Projectile>();
	}

	public bool Overlaps(Projectile projectile) => (projectile.transform.position - transform.position).magnitude <= projectile.ColliderRadius + _colliderRadius;

	protected void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, _colliderRadius);
	}
}