using UnityEngine;

public class ShadowTransform
{
    private TransformCopy self;
    private Transform target;
    public ShadowTransform(Transform target)
    {
        this.target = target;
        this.self = new TransformCopy(target);
    }
    public void Move(float positionDamp, float rotationDamp)
    {
        MovePosition(positionDamp);
        MoveRotation(rotationDamp);
    }
    public TransformCopy WorldToLocal(TransformCopy world)
    {
        return WorldToLocal(self,world);
    }
    public TransformCopy LocalToWorld(TransformCopy local)
    {
        return LocalToWorld(self,local);
    }
    private void MovePosition(float damp)
    {
        var dist = Vector3.Distance(self.position,target.position);
        var delta = damp == 1f ? dist : dist*damp*Time.deltaTime*25;
        self.position = Vector3.MoveTowards(self.position,target.position,delta);
    }
    private void MoveRotation(float damp)
    {
        var angle = Quaternion.Angle(self.rotation,target.rotation);
        var delta = damp == 1f ? angle : angle*damp*Time.deltaTime*25;
        self.rotation = Quaternion.RotateTowards(self.rotation,target.rotation,delta);
    }
    public static TransformCopy WorldToLocal(TransformCopy parent, TransformCopy childWorld)
    {
        var parentRotation = Quaternion.Inverse(parent.rotation);
        var childLocalPosition = parentRotation * (childWorld.position - parent.position);
        var childLocalRotation = parentRotation * childWorld.rotation;
        return new TransformCopy(childLocalPosition,childLocalRotation);
    }
    public static TransformCopy LocalToWorld(TransformCopy parent, TransformCopy childLocal)
    {
        var parentRotation = parent.rotation;
        var pos = parentRotation * childLocal.position + parent.position;
        var ros = parentRotation * childLocal.rotation;
        return new TransformCopy(pos,ros);
    }
}