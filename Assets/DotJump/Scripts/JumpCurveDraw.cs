using UnityEngine;

[RequireComponent(typeof(JumpCurve))]
public class JumpCurveDraw : MonoBehaviour
{
    private JumpCurve jumpCurve;

    private void OnEnable()
    {
        jumpCurve = GetComponent<JumpCurve>();
    }
    private void Update()
    {
        Draw(1000);
    }
    private void Draw(int iterations)
    {
        var dist = DistanceIgnoreYAxis(jumpCurve.start,jumpCurve.end);
        var xs = Points(0,dist,iterations);
        var points = new Vector3[xs.Length];
        for (int i = 0; i < xs.Length; i++)
            points[i] = jumpCurve.Evaluate(xs[i]);
        Draw(points);
    }
    private void Draw(Vector3[] points)
    {
        var length = points.Length;
        if (length < 2) return;
        Vector3 p0 = points[0];
        Vector3 p1;
        for (int i = 1; i < length; i++)
        {
            p1 = points[i];
            Debug.DrawLine(p0,p1,Color.green);
            p0 = p1;
        }
    }
    private float[] Points(float p0, float p1, int iterations)
    {
        var pointsCount = iterations+1;
        var array = new float[pointsCount];
        var min = Mathf.Min(p0,p1);
        var max = Mathf.Max(p0,p1);
        for (int i = 0; i < pointsCount; i++)
        {
            var t = (float)i/iterations;
            array[i] = Mathf.Lerp(min,max,t);
        }
        return array;
    }
    private float DistanceIgnoreYAxis(Vector3 v0, Vector3 v1)
    {
        v0.y = v1.y;
        return Vector3.Distance(v0,v1);
    }
}
