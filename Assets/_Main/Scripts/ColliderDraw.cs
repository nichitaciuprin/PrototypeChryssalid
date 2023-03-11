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

        material.color = BoxCollides(boxCollider) ? Color.yellow : Color.green;
    }
    private bool BoxCollides(BoxCollider box)
    {
		var center = box.transform.position;
		var halfExtents = box.transform.localScale / 2;
		var rotation = box.transform.rotation;
        var layerMask = ~0;

        box.enabled = false;
        Physics.SyncTransforms();
        var result = Physics.CheckBox(center,halfExtents,rotation,layerMask);
        box.enabled = true;

        return result;
    }
}
