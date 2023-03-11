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
    public static Vector2 InputDirection()
    {
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
    public static Vector3 LiftPoint(Vector3 positionToLift, Vector3 position1, Vector3 position2, float maxAlloweDistance)
    {
        var amount = 0.005f;
        while (true)
        {
            positionToLift.y += amount;
            if (Vector3.Distance(positionToLift,position1) > maxAlloweDistance) break;
            if (Vector3.Distance(positionToLift,position2) > maxAlloweDistance) break;
        }
        positionToLift.y -= amount;
        return positionToLift;
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