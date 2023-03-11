using UnityEngine;

public static class Bezier
{
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}
	public static void Draw(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color)
	{
		var start = p0;
		var iterations = 100;
		for (int i = 1; i < iterations+1; i++)
		{
			var t = Mathf.InverseLerp(0,iterations,i);
			var end = GetPoint(p0,p1,p2,p3,t);
			Debug2.DrawLine(start,end,color);
			start = end;
		}
	}
	public static float SimpleDistance(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		var start = p0;
		var iterations = 1000;
		var dist = 0f;
		for (int i = 1; i < iterations+1; i++)
		{
			var t = Mathf.InverseLerp(0,iterations,i);
			var end = GetPoint(p0,p1,p2,p3,t);
			dist += Vector3.Distance(start,end);
			start = end;
		}
		return dist;
	}
}