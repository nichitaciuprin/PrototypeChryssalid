using UnityEngine;

[ExecuteInEditMode]
public class DrawBounds : MonoBehaviour
{
    private Collider col;
    private Collider2D col2d;
    private void Start()
    {
        col = this.transform.GetComponent<Collider>();
        col2d = this.transform.GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (col != null) Draw(col);
        if (col2d != null) Draw(col2d);
    }
    private void OnDrawGizmos()
    {
        if (col2d == null) return;
        if (col2d.GetType() != typeof(CircleCollider2D)) return;
        var col = col2d as CircleCollider2D;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, col.radius);
    }
    private void Draw(Collider collider)
    {
        var bounds = collider.bounds;
        Helper.DrawBounds(bounds,Color.green);
        bounds.Expand(col.contactOffset*2);
        Helper.DrawBounds(bounds,Color.red);
    }
    private void Draw(Collider2D collider2D)
    {
        if (collider2D.GetType() == typeof(CircleCollider2D)) return;
        var bounds = collider2D.bounds;
        Helper.DrawBounds(bounds,Color.green);
    }
}
