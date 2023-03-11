using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour
{
    public Transform hip;
    public bool isRight;
    [Range(0,2)] public float footMoveTime;
    [HideInInspector] public bool inProcess = false;
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

        float t = 0;
        while (true)
        {
            var speed = footMoveTime;
            if (speed == 0)
                t = 1;
            else
                t += Time.deltaTime / speed;

            var midle = (start + end) / 2;
            midle.y += 0.3f;
            transform.position = Helper.BezierCurve(start, midle, end, t);

            if (t >= 1) break;
            yield return null;
        }

        inProcess = false;
    }
}