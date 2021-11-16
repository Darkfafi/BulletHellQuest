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
	private EntityPathData _projectilePath = EntityPathData.Default(10f, 10f);

	[Header("Requirements")]
	[SerializeField]
	private ProjectilesPoolChannel _projectilesPoolChannel = null;

	private ProjectilesPool _projectilesPool = null;
	private List<Projectile> _currentProjectiles = new List<Projectile>();
	private Coroutine _projectilesRoutine = null;

	protected void Awake()
	{
		if (_projectilesPoolChannel != null)
		{
			_projectilesPool = _projectilesPoolChannel.ProjectilesPool;
		}
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
					Projectile projectile = _projectilesPool.Pop(null);
					projectile.transform.position = emitterPos;
					projectile.transform.rotation = Quaternion.Euler(0f, 0f, emitterAngle);
					projectile.StartCoroutine(_projectilePath.PathRoutine(projectile, fireDirection, (p)=> 
					{
						if (Mathf.Approximately(p, 1f))
						{
							_projectilesPool.Push(projectile);
						}
					}));
					_currentProjectiles.Add(projectile);
				}
			}
			loopsFinished++;
			yield return new WaitForSeconds(_delayBetweenLoops);
		} while (_loopCount < 0 || loopsFinished < _loopCount);

		StopEmitter();
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
			{
				Handles.ConeHandleCap(GetInstanceID(), emitterPos, Quaternion.Euler(-emitAngle, 90f, 0f), 0.2f, EventType.Repaint);
			}
		}
	}
#endif
}


