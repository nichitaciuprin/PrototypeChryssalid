using UnityEngine;

[ExecuteInEditMode]
public class Curve : MonoBehaviour
{
    public Transform p0,p1,p2,p3;
    public Color color;
    public bool drawLines;
    public float dist;

    private void Update()
    {
        if (p0 == null) return;
        if (p1 == null) return;
        if (p2 == null) return;
        if (p3 == null) return;

        Bezier.Draw(p0.position, p1.position, p2.position, p3.position, color);

        dist = Bezier.SimpleDistance(p0.position, p1.position, p2.position, p3.position);

        if (!drawLines) return;
        Debug2.DrawLine(p0.position, p1.position, Color.white);
        Debug2.DrawLine(p1.position, p2.position, Color.white);
        Debug2.DrawLine(p2.position, p3.position, Color.white);
    }
}
