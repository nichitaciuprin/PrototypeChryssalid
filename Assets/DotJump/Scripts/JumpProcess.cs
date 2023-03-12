using UnityEngine;

public class JumpProcess : MonoBehaviour
{
    public JumpCurve jumpCurve;
    public Transform moveObject;
    public float gravity;
    private float moveObjectPosition;

    public static JumpProcess Create(Vector3 start, Vector3 end, float curve, float gravity, Transform moveObject)
    {
        var rootObject = new GameObject("JumpProcess");
        var component = rootObject.AddComponent<JumpProcess>();
        component.jumpCurve = JumpCurve.Create(start,end,curve);
        component.gravity = gravity;
        component.moveObject = moveObject;
        return component;
    }
    public float Progress()
    {
        var dist = DistanceIgnoreYAxis(jumpCurve.start,jumpCurve.end);
        var t = moveObjectPosition/dist;
        return t;
    }
    public Vector2 Direction()
    {
        var dir = jumpCurve.end - jumpCurve.start;
        dir.y = 0;
        return new Vector2(dir.x,dir.z);
    }
    private void Update()
    {
        moveObject.position = jumpCurve.Evaluate(moveObjectPosition);
        var speed = HorSpeed(jumpCurve.curve,gravity);
        moveObjectPosition += Time.deltaTime*speed;
        var dist = DistanceIgnoreYAxis(jumpCurve.start,jumpCurve.end);
        if (moveObjectPosition >= dist)
        {
            Destroy(gameObject);
            Destroy(jumpCurve.gameObject);
        }
    }
    private static float HorSpeed(float a, float g)
    {
        var a_abs = Mathf.Abs(a);
        var t = Mathf.Sqrt(2*a_abs/g);
        return 1/t;
    }
    private float DistanceIgnoreYAxis(Vector3 v0, Vector3 v1)
    {
        v0.y = v1.y;
        return Vector3.Distance(v0,v1);
    }
}
