using UnityEngine;

public class ColliderDraw : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Material material;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        material = boxCollider.GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if (boxCollider == null) return;
        if (material == null) return;

        if (BoxCollides(boxCollider))
            material.color = Color.yellow;
        else
            material.color = Color.green;
    }
    private bool BoxCollides(BoxCollider box)
    {
		var center = box.transform.position;
		var halfExtents = box.transform.localScale / 2;
		var rotation = box.transform.rotation;
        var layer = ~0;

        box.enabled = false;
        Physics.SyncTransforms();
        var result = Physics.CheckBox(center,halfExtents,rotation,layer);
        box.enabled = true;

        return result;
    }
}
