using UnityEngine;

public class TwoBoneIK : MonoBehaviour
{
    public Transform target;
    public Transform pole;

    private Transform p0;
    private Transform p1;
    private Transform p2;

    private float boneLength1;
    private float boneLength2;
    private float boneLengthSum;

    private void Start()
    {
        p2 = transform;
        p1 = p2.parent; if (p1 == null) return;
        p0 = p1.parent; if (p0 == null) return;

        boneLength1 = Vector3.Distance(p0.position,p1.position);
        boneLength2 = Vector3.Distance(p1.position,p2.position);
        boneLengthSum = boneLength1+boneLength2;
    }
    public void Update()
    {
        if (p1 == null) return;
        if (p0 == null) return;

        var vec1 = target.position - p0.position;
        var vec1m = vec1.magnitude;

        if (vec1m < boneLengthSum)
        {
            var vec2Nor = Other.GetPerp(vec1,pole.position - p0.position).normalized;
            var mid = vec1m * boneLength1 / boneLengthSum;
            var vec2Mag = Mathf.Sqrt(Mathf.Pow(boneLength1,2) - Mathf.Pow(mid,2));
            var vec2 = vec2Nor*vec2Mag;
            var newP1 = p0.position + vec1.normalized*mid + vec2;
            var newP2 = p0.position + vec1;
            var worldUp = vec2;
            p0.LookAt(newP1,worldUp);
            p1.LookAt(newP2,worldUp);
        }
        else
        {
            var worldUp = pole.position;
            p0.LookAt(target.position,worldUp);
            p1.LookAt(target.position,worldUp);
        }
    }
}
