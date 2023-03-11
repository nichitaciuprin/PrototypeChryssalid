using UnityEngine;

public class TwoLegs : MonoBehaviour
{
    public Hip hip;
    public Foot footL;
    public Foot footR;
    public Transform poleL;
    public Transform poleR;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        UpdateHip(hip,footL,footR);
        UpdatePoles(hip,poleL,poleR);
        UpdateFoots(footL,footR);

        if (!Input.anyKey) return;
        var inputDirection = Other.InputDirection();
        if (inputDirection == Vector2.zero) return;
        if (footL.inProcess) return;
        if (footR.inProcess) return;
        var moveDirection = TwoLegs.MoveDirection(hip,inputDirection);
        var footToMove = TwoLegs.FootToMove(moveDirection,footL,footR);
        var footToStay = footToMove == footL ? footR : footL;
        var isRight = footToMove == footR;
        var movePosition = TwoLegs.TryFindMovePosition(hip,moveDirection,footToStay);
        //var footPlace = Test.TryFindFootMovePoint(hip,moveDirection,footToStay,footToMove,isRight);
        if (movePosition == Vector3.zero) return;
        //InterpolateFoot(footToMove,footToMove.transform.position,footPlace,1);
        footToMove.Move(movePosition);
    }
    public static void UpdatePoles(Hip hip, Transform poleL, Transform poleR)
    {
        var forward = hip.transform.forward;
        forward.y = 0;
        var left = forward + hip.transform.right*-0.07f;
        var right = forward + hip.transform.right*0.07f;
        poleL.position = hip.transform.position + left;
        poleR.position = hip.transform.position + right;
    }
    public static void UpdateFoots(Foot footL, Foot footR)
    {
        footL.twoBoneIK.Update();
        footR.twoBoneIK.Update();
    }    
    public static bool IsFootsIntersect(Foot footL, Foot footR)
    {
        return footL.IsIntersects() || footR.IsIntersects();
    }
    public static bool IsFootOnWrongSide(Transform hip, Foot foot, bool isFootRight)
    {
        var local = hip.InverseTransformPoint(foot.transform.position);
        if (isFootRight)
            return local.x < 0;
        else
            return local.x > 0;
    }
    public static Vector3 TryFindMovePosition(Hip hip, Vector3 moveDirection, Foot footToStay)
    {
        var maxDistance_1 = 1;
        var rayOrigin_1 = hip.transform.position;
        var raycastHit_1 = Other.Raycast(rayOrigin_1,moveDirection,maxDistance_1);
        var vec1 = raycastHit_1.collider == null ? moveDirection*maxDistance_1 : raycastHit_1.point-rayOrigin_1;

        Debug.DrawLine(rayOrigin_1,rayOrigin_1+vec1,Color.blue,3);
        
        var div = 20;
        var maxDistance_2 = 3;
        for (int i = -1; i < div; i++)
        {
            var t = Mathf.InverseLerp(div,0,i);
            var rayOrigin_2 = rayOrigin_1 + vec1 * t;
            var raycastHit_2 = Other.Raycast(rayOrigin_2,Vector3.down,maxDistance_2);
            if (raycastHit_2.collider == null)
                Debug.DrawLine(rayOrigin_2,rayOrigin_2+Vector3.down*maxDistance_2,Color.magenta,3);
            else
                Debug.DrawLine(rayOrigin_2,raycastHit_2.point,Color.magenta,3);
            if (raycastHit_2.collider == null) continue;
            if (IsFootDistanceBad(footToStay.transform.position,raycastHit_2.point)) continue;
            //if (IsBodyPoseBad(hip,footToMove,footToStay,result.point)) continue;
            return raycastHit_2.point;
        }

        return Vector3.zero;
    }
    public static bool IsSurfaceAngleBad(RaycastHit raycastHit)
    {
        var angle3 = Vector3.Angle(raycastHit.normal,Vector3.up);
        return angle3 > 85f;
    }
    public static bool IsBodyPoseBad(Hip hip, Foot footToMove, Foot footToStay, Vector3 footPosition)
    {
        var start = footToMove.transform.position;
        var end = footPosition;
        
        var div = 10;
        var result = false;
        for (int i = 1; i <= div; i++)
        {
            var t = i/div;
            Interpolate(hip,footToMove,footToStay,start,end,t);
            UpdateHip(hip,footToMove,footToStay);
            UpdateFoots(footToMove,footToStay);
            result = IsPoseBad(footToStay,footToMove);
            if (result) break;
        }

        Interpolate(hip,footToMove,footToStay,start,end,0);
        UpdateHip(hip,footToMove,footToStay);
        UpdateFoots(footToMove,footToStay);
        return result;
    }
    public static void Interpolate(Hip hip, Foot footToMove, Foot footToStay, Vector3 start, Vector3 end, float t)
    {
        TwoLegs.InterpolateFoot(footToMove, start, end, t);
    }
    public static void InterpolateFoot(Foot foot, Vector3 start, Vector3 end, float t)
    {
        var midle = (start + end) / 2;
        midle.y += 0.3f;
        foot.transform.position = Other.BezierCurve(start, midle, end, t);
        //foot.fastIKFabric.ResolveIK();
    }
    public static bool IsPoseBad(Foot footL, Foot footR)
    {
        return footL.IsIntersects() || footR.IsIntersects();
    }
    public static Foot FootToMove(Vector3 moveDirection, Foot footL, Foot footR)
    {
        var rot = Quaternion.FromToRotation(Vector3.forward,moveDirection);
        var parent = new TransformCopy(Vector3.zero,rot);
        var child_1 = new TransformCopy(footL.transform);
        var child_2 = new TransformCopy(footR.transform);
        var point_1 = Other.WorldToLocal(parent,child_1);
        var point_2 = Other.WorldToLocal(parent,child_2);
        return point_1.position.z < point_2.position.z ? footL : footR;
    }
    public static Vector3 DirectionAngled(Vector3 moveDirection, float angle1, float angle2)
    {
        var rads = 1.5708f * angle1;
        var gradus = 45 * angle2;
        var vec1 = Vector3.down;
        var vec2 = Vector3.RotateTowards(moveDirection,vec1,rads,1);
        var rot = Quaternion.Euler(0,gradus,0);
        var vec3 = rot * vec2;
        return vec3;
    }
    public static Vector3 MoveDirection(Hip hip, Vector2 inputDirection)
    {
        if (inputDirection == Vector2.zero)
            inputDirection = Vector2.up;
        inputDirection.Normalize();
        var newDirection = new Vector3(inputDirection.x,0,inputDirection.y);
        var axis = hip.transform.rotation.eulerAngles.y;
        var rot = Quaternion.Euler(0,axis,0);
        var moveDirection = rot * newDirection;
        //Debug.DrawLine(hip.transform.position, hip.transform.position+moveDirection,Color.blue,10);
        return moveDirection;
    }
    public static void UpdateHip(Hip hip, Foot footL, Foot footR)
    {
        UpdateHip_Position(hip,footL,footR);
        UpdateHip_Rotation(hip, Other.InputDirection2());
    }
    public static void UpdateHip_Position(Hip hip, Foot footL, Foot footR)
    {
        var targetPosiiton = HipTargetPosition(hip,footL,footR);
        MoveHipToTargetPoisition(hip,targetPosiiton);
    }
    public static void UpdateHip_Rotation(Hip hip, Vector2 inputRotation)
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
    public static void MoveHipToTargetPoisition(Hip hip, Vector3 target)
    {
        var rb = hip.GetComponent<Rigidbody>();
        var dir = target - hip.transform.position;
        rb.velocity = dir*hip.forse;
    }
    public static Vector3 HipTargetPosition(Hip hip, Foot footL, Foot footR)
    {
        var point1 = footL.transform.position;
        var point2 = footR.transform.position;
        var point3 = Other.GetMiddlePoint(point1,point2);

        var pointToLift = point3;
        var otherPoint1 = point1;
        var otherPoint2 = point2;
        var maxAlloweDistance = hip.height;
        var liftedPoint = Other.LiftPoint(pointToLift,otherPoint1,otherPoint2,maxAlloweDistance);

        return liftedPoint;
    }
    public static bool IsFootIntersects(Foot foot)
    {
        return Other.CheckBox(foot.collider_1) || Other.CheckBox(foot.collider_2);
    }
    public static bool IsFootDistanceBad(Vector3 footPos1, Vector3 footPos2)
    {
        return Vector3.Distance(footPos1,footPos2) > 0.7f;
    }

    //maybe delete
    public static Vector3 TryFindFootMovePoint3(Hip hip, Vector3 moveDirection, Foot footToStay, Foot footToMove, bool isRight)
    {
        for (int i = 0; i < 20; i++)
        for (int j = 0; j < 20; j++)
        {
            var angle1 = j == 0 ? 0 : j/20f;
            var angle2 = i == 0 ? 0 : i/20f;
            if (!isRight) angle2*=-1;
            var direction = DirectionAngled(moveDirection,angle1,angle2);
            var result = Other.Raycast(hip.transform.position,direction,300);
            if (result.collider == null) continue;
            if (IsFootDistanceBad(footToStay.transform.position,result.point)) continue;
            if (IsSurfaceAngleBad(result)) continue;
            //if (IsBodyPoseBad(hip,footToMove,footToStay,result.point)) continue;
            return result.point;
            //Debug.DrawLine(hip.transform.position,result.point,Color.blue,5);
            //Debug.DrawRay(hip.transform.position,direction,Color.blue,10);
            //Debug.DrawLine(hip.transform.position,result.point,Color.magenta,10);
            //Debug.DrawLine(hip.transform.position,result.point,Color.magenta,5);
        }
        return Vector3.zero;
    }
}