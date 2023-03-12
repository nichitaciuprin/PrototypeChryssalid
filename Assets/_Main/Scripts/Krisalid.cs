using UnityEngine;

public class Krisalid : MonoBehaviour
{
    public Hip hip;
    public Foot footL;
    public Foot footR;
    public Transform poleL;
    public Transform poleR;

    private const float debugLifeTime = 3f;
    private const float maxDistanceFootFoot = 0.7f;
    private const float maxDistanceHipFoot = 0.9f;
    private const float force = 30f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        UpdateHip_Position(hip,footL,footR);
        UpdateHip_Rotation(hip, Helper.InputDirection2());
        UpdatePoles(hip,poleL,poleR);
        footL.twoBoneIK.Update();
        footR.twoBoneIK.Update();
        if (!Input.anyKey) return;
        var inputDirection = Helper.InputDirection();
        if (inputDirection == Vector2.zero) return;
        if (footL.inProcess) return;
        if (footR.inProcess) return;
        var moveDirection = MoveDirection(hip,inputDirection);
        var footToMove = FootToMove(moveDirection,footL,footR);
        var footToStay = footToMove == footL ? footR : footL;
        var isRight = footToMove == footR;
        var movePosition = TryFindFootPosition(moveDirection,footToStay.transform.position);
        if (movePosition == null) return;
        footToMove.Move(movePosition.Value);
    }

    private void UpdatePoles(Hip hip, Transform poleL, Transform poleR)
    {
        var forward = hip.transform.forward;
        forward.y = 0;
        var left  = forward - hip.transform.right * 0.07f;
        var right = forward + hip.transform.right * 0.07f;
        poleL.position = hip.transform.position + left;
        poleR.position = hip.transform.position + right;
    }
    private Vector3? TryFindFootPosition(Vector3 moveDirection, Vector3 footToStay)
    {
        var end = hip.transform.position;
        var start = Raycast1(end,moveDirection,1);
        return TryFindFootPosition(start,end,0.05f,footToStay);
    }
    private Vector3? TryFindFootPosition(Vector3 start, Vector3 end, float divisionLength, Vector3 footToStay)
    {
        var diff = end-start;
        var direction = diff.normalized;
        var count = (int)(diff.magnitude/divisionLength);
        var distance = 10f;
        var rayCastDirection = Vector3.down;
        for (int i = 0; i < count; i++)
        {
            var origin = start + direction*divisionLength*i;
            var point = Raycast2(origin,rayCastDirection,distance);
            if (point == null) continue;
            if (!FootDistanceOk(footToStay,point.Value)) continue;
            return point;
        }
        return null;
    }
    private Foot FootToMove(Vector3 moveDirection, Foot footL, Foot footR)
    {
        var rot = Quaternion.FromToRotation(Vector3.forward,moveDirection);
        var parent = new TransformCopy(Vector3.zero,rot);
        var child_1 = new TransformCopy(footL.transform);
        var child_2 = new TransformCopy(footR.transform);
        var point_1 = Helper.WorldToLocal(parent,child_1);
        var point_2 = Helper.WorldToLocal(parent,child_2);
        return point_1.position.z < point_2.position.z ? footL : footR;
    }
    private Vector3 MoveDirection(Hip hip, Vector2 inputDirection)
    {
        if (inputDirection == Vector2.zero)
            inputDirection = Vector2.up;
        inputDirection.Normalize();
        var newDirection = new Vector3(inputDirection.x,0,inputDirection.y);
        var axis = hip.transform.rotation.eulerAngles.y;
        var rot = Quaternion.Euler(0,axis,0);
        var moveDirection = rot * newDirection;
        return moveDirection;
    }
    private void UpdateHip_Position(Hip hip, Foot footL, Foot footR)
    {
        var targetPosiiton = HipTargetPosition(hip,footL,footR);
        MoveHipToTargetPoisition(hip,targetPosiiton);
    }
    private void UpdateHip_Rotation(Hip hip, Vector2 inputRotation)
    {
        var x = hip.rot.x;
        var y = hip.rot.y;
        x -= inputRotation.y;
        y += inputRotation.x;
        x = Mathf.Clamp(x,-90,90);
        hip.rot.x = x;
        hip.rot.y = y;
        hip.transform.rotation = Quaternion.Euler(x,y,0);
    }
    private void MoveHipToTargetPoisition(Hip hip, Vector3 target)
    {
        var rb = hip.GetComponent<Rigidbody>();
        var dir = target - hip.transform.position;
        rb.velocity = dir*force;
    }
    private Vector3 HipTargetPosition(Hip hip, Foot footL, Foot footR)
    {
        var pos1 = footL.transform.position;
        var pos2 = footR.transform.position;
        var pos3 = (pos1+pos2)/2;
        var maxAlloweDistance = maxDistanceHipFoot;
        var liftedPoint = LiftPoint(pos3,pos1,pos2,maxAlloweDistance);
        return liftedPoint;
    }
    private bool FootDistanceOk(Vector3 footPosition_1, Vector3 footPosition_2)
    {
        return Vector3.Distance(footPosition_1,footPosition_2) < maxDistanceFootFoot;
    }
    private Vector3 LiftPoint(Vector3 positionToLift, Vector3 position1, Vector3 position2, float maxAlloweDistance)
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
    private Vector3 Raycast1(Vector3 origin, Vector3 direction, float maxDist)
    {
        var raycastHit = Helper.Raycast(origin,direction,maxDist);
        var point = raycastHit.collider == null ? origin+direction*maxDist : raycastHit.point;
        Debug.DrawLine(origin,point,Color.green,debugLifeTime);
        return point;
    }
    private Vector3? Raycast2(Vector3 origin, Vector3 direction, float maxDist)
    {
        var raycastHit = Helper.Raycast(origin,direction,maxDist);
        Vector3 point;
        if (raycastHit.collider == null)
        {
            point = origin+direction*maxDist;
            Debug.DrawLine(origin,point,Color.red,debugLifeTime);
        }
        else
        {
            point = raycastHit.point;
            Debug.DrawLine(origin,point,Color.red,debugLifeTime);
        }
        return point;
    }
}