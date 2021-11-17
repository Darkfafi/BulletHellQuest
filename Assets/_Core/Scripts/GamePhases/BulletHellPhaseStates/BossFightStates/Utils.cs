using System.Collections;
using UnityEngine;
public static class Utils
{
	public delegate void AnimationCurveHandler(float value, float progress, float time, float duration);

	public static Vector2 FromAngleToVector2(this float self)
	{
		self = self * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(self), Mathf.Sin(self)).normalized;
	}

	public static IEnumerator AnimationCurveRoutine(this AnimationCurve curve, AnimationCurveHandler progressAction)
	{
		float t = 0f;
		float d = curve.GetDuration();
		while(t <= d)
		{
			float p = Mathf.Clamp01(t / d);

			progressAction(curve.Evaluate(t), p, t, d);

			if (Mathf.Approximately(p, 1f))
			{
				break;
			}

			t = Mathf.Clamp(t + Time.deltaTime, 0f, d);
			yield return null;
		}
	}

	public static float GetDuration(this AnimationCurve curve)
	{
		return curve.length == 0 ? 0f : curve[curve.length - 1].time;
	}
}