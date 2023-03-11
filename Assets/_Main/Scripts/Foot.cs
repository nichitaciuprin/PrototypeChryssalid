using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour
{
    public bool inProcess { get; private set; }
    public TwoBoneIK twoBoneIK;

    public void Move(Vector3 targetPosition)
    {
        StartCoroutine(Move_coroutine(targetPosition));
    }
    private IEnumerator Move_coroutine(Vector3 targetPosition)
    {
        if (inProcess) yield break;
        inProcess = true;

        var start = transform.position;
        var end = targetPosition;
        var t = 0f;

        while (true)
        {
            var footMoveTime = 0.2f;
            t = footMoveTime == 0 ? 1 : t + Time.deltaTime / footMoveTime;

            var midle = (start + end) / 2;
            midle.y += 0.3f;
            transform.position = BezierCurve(start, midle, end, t);

            if (t >= 1) break;
            yield return null;
        }

        inProcess = false;
    }
    private Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
		return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
	}
}