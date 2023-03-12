using UnityEngine;

public class Helper
{
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
}