using UnityEngine;

public class JumpCurve : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float curve;

    public static JumpCurve Create(Vector3 start, Vector3 end, float curve)
    {
        var rootObject = new GameObject("JumpCurve");
        var component = rootObject.AddComponent<JumpCurve>();
        rootObject.AddComponent<JumpCurveDraw>();
        component.start = start;
        component.end = end;
        component.curve = curve;
        return component;
    }
    public Vector3 Evaluate(float x)
    {
        return F2(start,end,curve,x);
    }
    private Vector3 F2(Vector3 start, Vector3 end, float a, float x)
    {
        return F1(end-start,a,x) + start;
    }
    private Vector3 F1(Vector3 end, float a, float x)
    {
        var vec1 = new Vector2(end.x,end.z);
        var length = vec1.magnitude;
        var end2 = new Vector2(length,end.y);
        var t = x/end2.x;
        vec1 *= t;
        var y = F0(end2,a,x);
        return new Vector3(vec1.x,y,vec1.y);
    }
    private float F0(Vector2 end, float a, float x)
    {
        var a_abs = -Mathf.Abs(a);
        var b = end.y/end.x - a_abs*end.x;
        var y = a_abs*x*x + b*x;
        return y;
    }
}
