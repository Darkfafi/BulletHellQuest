using System.Collections;
using UnityEngine;

public class BossBehaviourState : BossFightStateBase
{
	#region Editor Variables

	[SerializeField]
	private EntityPathData _pathToStartPos = EntityPathData.Default();

	[SerializeField, Range(0f, 360)]
	private float _directionAngle = 0f;

	[SerializeField]
	private EntityPathData _bossPathData = EntityPathData.Default();

	[SerializeField]
	private ProjectilesEmitterSystemGroup _emitter = null;

	[SerializeField]
	private bool _attachEmitterToBossShip = true;

	[SerializeField, Range(0f, 1f)]
	private float _startEmitterTime = 0f;

	[SerializeField, Range(0f, 1f)]
	private float _endEmitterTime = 1f;

	#endregion

	#region Variables

	private Coroutine _behaviourRoutine = null;
	private Vector2? _attachedEmitterPrePos = null;
	private Transform _preEmitterParent = null;

	#endregion

	protected override void OnEnter()
	{
		_behaviourRoutine = StartCoroutine(BossBehaviourRoutine());
	}

	protected override void OnExit()
	{
		if (_behaviourRoutine != null)
		{
			StopCoroutine(_behaviourRoutine);
			_behaviourRoutine = null;
		}

		if (_emitter != null)
		{
			_emitter.StopEmitters();

			if (_attachEmitterToBossShip)
			{
				if (_attachedEmitterPrePos.HasValue)
				{
					_emitter.transform.SetParent(_preEmitterParent);
					_emitter.transform.localPosition = _attachedEmitterPrePos.Value;
				}
				else
				{
					_emitter = null;
				}
			}
		}
	}

	protected IEnumerator BossBehaviourRoutine()
	{
		if (_attachEmitterToBossShip)
		{
			if (_emitter == null)
			{
				_emitter = _emitter == null ? StateParent.BossInstance.MainGuns : _emitter;
			}
			else
			{
				_attachedEmitterPrePos = _emitter.transform.localPosition;
				_preEmitterParent = _emitter.transform.parent;
				_emitter.transform.SetParent(StateParent.BossInstance.MainGuns.transform, false);
				_emitter.transform.localPosition = Vector3.zero;
			}
		}

		Vector2 dirToStart = transform.position - StateParent.BossInstance.transform.position;
		yield return _pathToStartPos.PathRoutine(StateParent.BossInstance, dirToStart, null);

		float angle = _directionAngle + transform.rotation.eulerAngles.z;
		int emitStage = 0;

		yield return _bossPathData.PathRoutine(StateParent.BossInstance,
			angle.FromAngleToVector2(), (p) =>
			{
				if (_emitter != null)
				{
					switch (emitStage)
					{
						case 0:
							if (p >= _startEmitterTime)
							{
								_emitter.StartEmitters();
								emitStage = 1;
							}
							break;
						case 1:
							if (p >= _endEmitterTime)
							{
								_emitter.StopEmitters();
								emitStage = 2;
							}
							break;
					}
				}

				if (Mathf.Approximately(p, 1f))
				{
					StateParent.GoToNextState();
				}
			}
		);
	}

#if UNITY_EDITOR
	protected void OnDrawGizmos()
	{
		Vector2 origin = transform.position;
		Vector2 pos = origin;

		float angle = _directionAngle + transform.rotation.eulerAngles.z;
		Vector2 dir = angle.FromAngleToVector2();

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(pos, 0.05f);

		Gizmos.color = Color.cyan;
		for (int t = 0; t <= 100; t++)
		{
			float p = t / 100f;
			Vector2 endPos = origin + _bossPathData.EvaluateDirection(p, dir);
			Gizmos.DrawLine(pos, endPos);
			pos = endPos;
		}
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(pos, 0.05f);
	}
#endif
}
