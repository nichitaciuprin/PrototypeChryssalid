using UnityEngine;

public class Helper
{
    public static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
		return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
	}
	public static RaycastHit Raycast(Vector3 origin, Vector3 direction, float maxDistance)
    {
        RaycastHit hitInfo;
        int layerMask = ~LayerMask.GetMask("Ignore Raycast");
        Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);
        return hitInfo;
    }
	public static void DrawBounds(Bounds b, Color color,float delay=0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, color, delay);
        Debug.DrawLine(p2, p3, color, delay);
        Debug.DrawLine(p3, p4, color, delay);
        Debug.DrawLine(p4, p1, color, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, color, delay);
        Debug.DrawLine(p6, p7, color, delay);
        Debug.DrawLine(p7, p8, color, delay);
        Debug.DrawLine(p8, p5, color, delay);

        // sides
        Debug.DrawLine(p1, p5, color, delay);
        Debug.DrawLine(p2, p6, color, delay);
        Debug.DrawLine(p3, p7, color, delay);
        Debug.DrawLine(p4, p8, color, delay);
    }
	public static bool CheckBox(BoxCollider box)
    {
        if (box.center != Vector3.zero) throw new System.Exception("Duno");
        if (box.size != new Vector3(1,1,1)) throw new System.Exception("Duno");

		var center = box.transform.position;
		var halfExtents = box.transform.localScale / 2;
		var rotation = box.transform.rotation;
        var layer = ~0;

        box.enabled = false;
        Physics.SyncTransforms();
        var result = Physics.CheckBox(center,halfExtents,rotation,layer);
        box.enabled = true;

        return result;
    }
    public static Vector2 InputDirection()
    {
        //return new Vector2(0,1);
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var vector = new Vector2(x,y);
        if (vector.magnitude > 1)
            vector.Normalize();
        return vector;
    }
    public static Vector2 InputDirection2()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");
        var vector = new Vector2(x,y);
        return vector;
    }
    public static TransformCopy WorldToLocal(TransformCopy parent, TransformCopy childWorld)
    {
        var parentRotation = Quaternion.Inverse(parent.rotation);
        var childLocalPosition = parentRotation * (childWorld.position - parent.position);
        var childLocalRotation = parentRotation * childWorld.rotation;
        return new TransformCopy(childLocalPosition,childLocalRotation);
    }
    public static TransformCopy[] ChildrenPoint_Save(Transform parent)
    {
        var count = parent.childCount;
        var duno = new TransformCopy[count];
        for (int i = 0; i < count; i++)
            duno[i] = new TransformCopy(parent.GetChild(i));
        return duno;
    }
    public static void ChildrenPoint_Load(Transform parent, TransformCopy[] save)
    {
        for (int i = 0; i < save.Length; i++)
            save[i].Apply(parent.transform.GetChild(i));
    }
    public static Vector3 RandomVector()
    {
        var x = Random.Range(-1,1f);
        var y = Random.Range(-1,1f);
        var z = Random.Range(-1,1f);
        return new Vector3(x,y,z).normalized;
    }
    public static Vector3 GetPerp(Vector3 main, Vector3 pole)
    {
        return Vector3.ProjectOnPlane(pole,main);
    }
    public static Vector3 LiftPoint(Vector3 pointToLift, Vector3 otherPoint1, float maxAlloweDistance)
    {
        var amount = 0.005f;
        while (true)
        {
            pointToLift.y += amount;
            var IsDistanceOk1 = Helper.IsDistanceOk(pointToLift,otherPoint1,maxAlloweDistance); if (!IsDistanceOk1) break;
            var IsDistanceOk2 = Helper.IsDistanceOk(pointToLift,otherPoint1,maxAlloweDistance); if (!IsDistanceOk2) break;
        }
        pointToLift.y -= amount;
        return pointToLift;
    }
    public static Vector3 LiftPoint(Vector3 pointToLift, Vector3 otherPoint1, Vector3 otherPoint2, float maxAlloweDistance)
    {
        var amount = 0.005f;
        while (true)
        {
            pointToLift.y += amount;
            var IsDistanceOk1 = Helper.IsDistanceOk(pointToLift,otherPoint1,maxAlloweDistance); if (!IsDistanceOk1) break;
            var IsDistanceOk2 = Helper.IsDistanceOk(pointToLift,otherPoint2,maxAlloweDistance); if (!IsDistanceOk2) break;
        }
        pointToLift.y -= amount;
        return pointToLift;
    }
    public static Vector3 GetMiddlePoint(Vector3 start, Vector3 end)
    {
        var vec = start - end;
        var half = vec / 2;
        var middle = end + half;
        return middle;
    }
    public static bool IsDistanceOk(Vector3 point1, Vector3 point2, float maxAlloweDistanceBetweenPoints)
    {
        var distanceBetweenPoints = Vector3.Distance(point1,point2);
        return distanceBetweenPoints < maxAlloweDistanceBetweenPoints;
    }
    public static void DrawPoint(Vector3 point, Color color)
    {
        var start = point;
        var length = 0.04f;
        for (int i = 0; i < 30; i++)
        {
            var randVec = Helper.RandomVector().normalized;
            var end = point+randVec*length;
            //var duration = 0.2f;
            Debug2.DrawLine(start,end,color);
        }
    }
}
public struct TransformCopy
{
    public Vector3 position;
    public Quaternion rotation;
    public TransformCopy(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
    }
    public TransformCopy(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
    public void Apply(Transform transform)
    {
        transform.position = this.position;
        transform.rotation = this.rotation;
    }
}