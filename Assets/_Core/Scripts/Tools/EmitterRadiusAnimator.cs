using System.Collections;
using UnityEngine;

public class EmitterRadiusAnimator : MonoBehaviour
{
    [SerializeField]
    private ProjectilesEmitterSystem _emitter;

	[SerializeField]
	private AnimationCurve _radiusCurve;

	private Coroutine _routine = null;

	protected void OnEnable()
	{
		if (_emitter != null)
		{
			_emitter.EmitterStartedEvent += OnEmitterStartedEvent;
			_emitter.EmitterStoppedEvent += OnEmitterStoppedEvent;

			if (_emitter.IsRunning)
			{
				StartRadiusRoutine();
			}
		}
	}

	protected void OnDisable()
	{
		StopRadiusRoutine();

		if (_emitter != null)
		{
			_emitter.EmitterStartedEvent -= OnEmitterStartedEvent;
			_emitter.EmitterStoppedEvent -= OnEmitterStoppedEvent;
		}
	}

	private void OnEmitterStartedEvent(ProjectilesEmitterSystem obj)
	{
		StartRadiusRoutine();
	}

	private void OnEmitterStoppedEvent(ProjectilesEmitterSystem obj)
	{
		StopRadiusRoutine();
	}

	private void StartRadiusRoutine()
	{
		StopRadiusRoutine();
		_routine = StartCoroutine(RadiusRoutine());
	}

	private void StopRadiusRoutine()
	{
		if (_routine != null)
		{
			StopCoroutine(_routine);
			_routine = null;

			if(_emitter != null)
			{
				_emitter.ResetRadius();
			}
		}
	}

	private IEnumerator RadiusRoutine()
	{
		yield return _radiusCurve.AnimationCurveRoutine((v, p, t, d) => 
		{
			_emitter.OverrideRadius(v);
		});
		StopRadiusRoutine();
	}

}
