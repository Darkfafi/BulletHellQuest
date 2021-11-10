using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ProjectilesEmitterSystem : MonoBehaviour
{
	[Header("Options")]
	[SerializeField, Range(1, 100)]
	private int _emitAmount = 1;
	[SerializeField, Range(0f, 360f)]
	private float _emitterSpreadAngle = 0f;
	[SerializeField]
	private float _emittersRadius = 1f;
	[SerializeField, Tooltip("-1 is an infinite loop")]
	private int _loopCount = -1;
	[SerializeField]
	private float _delayBetweenLoops = 1f;
	[SerializeField]
	private AnimationCurve _movement = AnimationCurve.Constant(0f, 1f, 0f);
	[SerializeField]
	private AnimationCurve _fullPath = AnimationCurve.Linear(0f, 0f, 10f, 10f);

	[Header("Requirements")]
	[SerializeField]
	private Projectile _projectilePrefab = null;


	private List<Projectile> _currentProjectiles = new List<Projectile>();
	private Coroutine _projectilesRoutine = null;

	protected void Start()
	{
		StartEmitter();
	}

	public void StartEmitter()
	{
		if (_projectilesRoutine == null)
		{
			_projectilesRoutine = StartCoroutine(ProjectilesRoutine());
		}
	}

	public void StopEmitter()
	{
		if (_projectilesRoutine != null)
		{
			StopCoroutine(_projectilesRoutine);
			_projectilesRoutine = null;

		}
	}

	private IEnumerator ProjectilesRoutine()
	{
		int loopsFinished = 0;

		do
		{
			for (int i = 0; i < _emitAmount; i++)
			{
				if (TryGetEmitterData(i, out Vector2 emitterPos, out float emitterAngle, out Vector2 fireDirection))
				{
					Projectile projectile = Instantiate(_projectilePrefab, emitterPos, Quaternion.Euler(0f, 0f, emitterAngle));
					projectile.StartCoroutine(ProjectileRoutine(projectile, fireDirection, _movement, _fullPath));
					_currentProjectiles.Add(projectile);
				}
			}
			loopsFinished++;
			yield return new WaitForSeconds(_delayBetweenLoops);
		} while (_loopCount < 0 || loopsFinished < _loopCount);

		StopEmitter();
	}

	private IEnumerator ProjectileRoutine(Projectile projectile, Vector2 direction, AnimationCurve movement, AnimationCurve fullPath)
	{
		Vector2 startPos = projectile.transform.position;
		float perpendicularAngle = Mathf.Atan2(direction.y, direction.x) + (Mathf.PI / 2f);
		Vector2 perpendicularDirection = new Vector2(Mathf.Cos(perpendicularAngle), Mathf.Sin(perpendicularAngle));

		float t = 0f;
		float d = _fullPath[_fullPath.length - 1].time;
		while (t <= d)
		{
			float p = t / d;

			projectile.transform.position =
				startPos +
				(direction * fullPath.Evaluate(t)) +
				(perpendicularDirection * movement.Evaluate(p));

			t += Time.deltaTime;
			yield return null;
		}

		Destroy(projectile.gameObject);
	}

	private bool TryGetEmitterData(int index, out Vector2 emitterPos, out float emitterAngle, out Vector2 emitterDirection)
	{
		if (index >= 0 && index < _emitAmount)
		{
			Vector2 origin = transform.position;
			float step = _emitterSpreadAngle / _emitAmount;
			emitterAngle = (transform.rotation.eulerAngles.z + (step * index)) * Mathf.Deg2Rad;
			emitterPos = origin + new Vector2(Mathf.Cos(emitterAngle), Mathf.Sin(emitterAngle)).normalized * _emittersRadius;
			emitterAngle *= Mathf.Rad2Deg;
			emitterDirection = (emitterPos - origin).normalized;
			return true;
		}
		emitterPos = default;
		emitterAngle = default;
		emitterDirection = default;
		return false;
	}

#if UNITY_EDITOR
	protected void OnDrawGizmosSelected()
	{
		for (int i = 0; i < _emitAmount; i++)
		{
			if (TryGetEmitterData(i, out Vector2 emitterPos, out float emitAngle, out Vector2 _))
				Handles.ConeHandleCap(GetInstanceID(), emitterPos, Quaternion.Euler(-emitAngle, 90f, 0f), 0.2f, EventType.Repaint);
		}
	}
#endif
}


