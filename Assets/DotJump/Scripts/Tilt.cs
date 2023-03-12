using UnityEngine;

public class Tilt : MonoBehaviour
{
    //public Vector2 localVelocityTarget;
    private Vector2 localTilt;

    public Vector2 globalDirection;
    public float angle;

    private void Update()
    {
        var axis1 = new Vector2(globalDirection.y,-globalDirection.x);
        var axis2 = new Vector2(axis1.x,axis1.y);
        var axis3 = new Vector3(axis2.x,0,axis2.y);
        transform.rotation = Quaternion.AngleAxis(angle,axis3) * transform.parent.rotation;
    }
    //private void Update()
    //{
    //    var tiltStrength = 3;
    //    var current = localTilt;
    //    var target = movemnt.localVelocityTarget*movemnt.maxSpeed*tiltStrength;
    //    var maxDistanceDelta = Time.deltaTime*1.5f;
    //    localTilt = MoveTowards2(current, target, maxDistanceDelta);
    //    var vec = new Vector3(localTilt.y,0,-localTilt.x);
    //    var rot = Quaternion.Euler(vec);
    //    transform.localRotation = rot;
    //}
    //private void Update2()
    //{
    //    var tiltRotation = movemnt.localVelocityTarget*movemnt.maxSpeed*tiltStrength;
    //    var vec2 = new Vector3(tiltRotation.y,0,-tiltRotation.x);
    //    var rot2 = Quaternion.Euler(vec2);
    //    transform.rotation *= rot2;
    //}
    private Vector2 MoveTowards2(Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        var speed = Vector2.Distance(current,target);
        return Vector2.MoveTowards(current,target,maxDistanceDelta*speed);
    }
}
