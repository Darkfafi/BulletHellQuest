using UnityEngine;
public static class Utils
{
	public static Vector2 FromAngleToVector2(this float self)
	{
		self = self * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(self), Mathf.Sin(self)).normalized;
	}
}