using UnityEngine;

public class Krisalid : MonoBehaviour
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
        var movePosition = TryFindMovePosition(hip,moveDirection,footToStay);
        if (movePosition == Vector3.zero) return;
        footToMove.Move(movePosition);
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
    private Vector3 TryFindMovePosition(Hip hip, Vector3 moveDirection, Foot footToStay)
    {
        var maxDistance_1 = 1;
        var rayOrigin_1 = hip.transform.position;
        var raycastHit_1 = Helper.Raycast(rayOrigin_1,moveDirection,maxDistance_1);
        var vec1 = raycastHit_1.collider == null ? moveDirection*maxDistance_1 : raycastHit_1.point-rayOrigin_1;

        Debug.DrawLine(rayOrigin_1,rayOrigin_1+vec1,Color.blue,3);

        var div = 20;
        var maxDistance_2 = 3;
        for (int i = -1; i < div; i++)
        {
            var t = Mathf.InverseLerp(div,0,i);
            var rayOrigin_2 = rayOrigin_1 + vec1 * t;
            var raycastHit_2 = Helper.Raycast(rayOrigin_2,Vector3.down,maxDistance_2);
            if (raycastHit_2.collider == null)
                Debug.DrawLine(rayOrigin_2,rayOrigin_2+Vector3.down*maxDistance_2,Color.magenta,3);
            else
                Debug.DrawLine(rayOrigin_2,raycastHit_2.point,Color.magenta,3);
            if (raycastHit_2.collider == null) continue;
            if (IsFootDistanceBad(footToStay.transform.position,raycastHit_2.point)) continue;
            return raycastHit_2.point;
        }

        return Vector3.zero;
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
        //Debug.DrawLine(hip.transform.position, hip.transform.position+moveDirection,Color.blue,10);
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
        rb.velocity = dir*hip.forse;
    }
    private Vector3 HipTargetPosition(Hip hip, Foot footL, Foot footR)
    {
        var point1 = footL.transform.position;
        var point2 = footR.transform.position;
        var point3 = (point1+point2)/2;

        var pointToLift = point3;
        var otherPoint1 = point1;
        var otherPoint2 = point2;
        var maxAlloweDistance = hip.height;
        var liftedPoint = Helper.LiftPoint(pointToLift,otherPoint1,otherPoint2,maxAlloweDistance);

        return liftedPoint;
    }
    private bool IsFootDistanceBad(Vector3 footPos1, Vector3 footPos2)
    {
        return Vector3.Distance(footPos1,footPos2) > 0.7f;
    }
}