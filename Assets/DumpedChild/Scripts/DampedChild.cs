using UnityEngine;

public class DampedChild : MonoBehaviour
{
    [Range(0f,1f)] public float position;
    [Range(0f,1f)] public float rotation;
    private TransformCopy self;
    private TransformCopy selfLocal;
    private ShadowTransform shadowParent;
    
    private void Update()
    {
        if (transform.parent == null)
        {
            shadowParent = null;
            return;
        }
        if (shadowParent == null)
        {
            self = new TransformCopy(transform);
            shadowParent = new ShadowTransform(transform.parent);
            selfLocal = shadowParent.WorldToLocal(self);
        }

        shadowParent.Move(position,rotation);
        self = shadowParent.LocalToWorld(selfLocal);

        var save = ChildrenSave(transform);
        self.Apply(transform);
        ChildrenLoad(transform,save);
    }
    public static TransformCopy[] ChildrenSave(Transform parent)
    {
        var count = parent.childCount;
        var duno = new TransformCopy[count];
        for (int i = 0; i < count; i++)
            duno[i] = new TransformCopy(parent.GetChild(i));
        return duno;
    }
    public static void ChildrenLoad(Transform parent, TransformCopy[] save)
    {
        for (int i = 0; i < save.Length; i++)
            save[i].Apply(parent.transform.GetChild(i));
    }
}