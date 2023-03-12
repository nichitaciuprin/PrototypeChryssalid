using UnityEngine;

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
