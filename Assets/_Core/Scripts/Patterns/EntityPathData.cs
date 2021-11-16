using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct EntityPathData
{
	public AnimationCurve FullPath;
	public AnimationCurve Movement;

	public static EntityPathData Default(float pathEndTime = 1f, float pathEndValue = 1f)
	{
		return new EntityPathData(AnimationCurve.Linear(0f, 0f, pathEndTime, pathEndValue), AnimationCurve.Constant(0f, 1f, 0f));
	}

	public EntityPathData(AnimationCurve fullPath, AnimationCurve movement)
	{
		FullPath = fullPath;
		Movement = movement;
	}

	public Vector2 Evaluate(float p, Vector2 direction)
	{
		float d = FullPath[FullPath.length - 1].time;
		float pFP = d * p;
		float perpendicularAngle = Mathf.Atan2(direction.y, direction.x) + (Mathf.PI / 2f);
		Vector2 perpendicularDirection = new Vector2(Mathf.Cos(perpendicularAngle), Mathf.Sin(perpendicularAngle));
		return (direction * FullPath.Evaluate(pFP)) +
				(perpendicularDirection * Movement.Evaluate(p));
	}

	public IEnumerator PathRoutine<T>(T target, Vector2 direction, Action<float> reachedEndCallback)
		where T : MonoBehaviour
	{
		return PathRoutine(target.transform, direction, (p) => 
		{
			reachedEndCallback?.Invoke(p);
		});
	}

	public IEnumerator PathRoutine(Transform target, Vector2 direction, Action<float> progressCallback)
	{
		Vector2 startPos = target.transform.position;
		float perpendicularAngle = Mathf.Atan2(direction.y, direction.x) + (Mathf.PI / 2f);
		Vector2 perpendicularDirection = new Vector2(Mathf.Cos(perpendicularAngle), Mathf.Sin(perpendicularAngle));

		float t = 0f;
		float d = FullPath[FullPath.length - 1].time;
		while (t <= d)
		{
			float p = Mathf.Clamp01(t / d);

			target.transform.position =
				startPos +
				(direction * FullPath.Evaluate(t)) +
				(perpendicularDirection * Movement.Evaluate(p));

			progressCallback?.Invoke(p);

			if(Mathf.Approximately(p, 1f))
			{
				break;
			}

			t = Mathf.Clamp(t + Time.deltaTime, 0f, d);
			yield return null;
		}
	}
}


