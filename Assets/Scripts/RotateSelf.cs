using System.Collections;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    private float speed;
    private float x;
    private float y;
    private float z;
    private IEnumerator Start()
    {
        while (true)
        {
            speed = Random.Range(0f,200f);
            x = Random.Range(0f,360f);
            y = Random.Range(0f,360f);
            z = Random.Range(0f,360f);
            yield return new WaitForSeconds(3);
        }
    }
    private void Update()
    {
        var save = Other.ChildrenPoint_Save(this.transform);
        var rot = Time.deltaTime*speed;
        this.transform.Rotate(new Vector3(x,y,z),rot);
        Other.ChildrenPoint_Load(this.transform,save);
    }
}
