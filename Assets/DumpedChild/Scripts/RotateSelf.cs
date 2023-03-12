using System.Collections;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    private const float maxSpeed = 200f;
    private float speed;
    private Vector3 nommal;
    private IEnumerator Start()
    {
        while (true)
        {
            speed = Random.Range(0f,maxSpeed);
            nommal = RandomRotationVector();
            yield return new WaitForSeconds(3);
        }
    }
    private void Update()
    {
        var angle = Time.deltaTime*speed;
        var normal = RandomRotationVector();
        transform.Rotate(normal,angle);
    }
    private Vector3 RandomRotationVector()
    {
        var x = Random.Range(0f,1f);
        var y = Random.Range(0f,1f);
        var z = Random.Range(0f,1f);
        return new Vector3(x,y,z);
    }
}
