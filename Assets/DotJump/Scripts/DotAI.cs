using System.Collections;
using UnityEngine;

public class DotAI : MonoBehaviour
{
    public JumpRotation jumpRotation;

    private IEnumerator Start()
    {
        while(true)
        {
            var target = RandomTarget();
            yield return Jump(target);
        }
    }
    private IEnumerator Jump(Vector3 target)
    {
        var start = transform.position;
        var end = target;
        var curve = 0.05f;
        var gravity = 10f;
        var jumpProcess = JumpProcess.Create(start,end,curve,gravity,transform);
        jumpRotation.jumpProcess = jumpProcess;
        while(jumpProcess != null) yield return null;
    }
    private Vector3 RandomTarget()
    {
        var x = Random.Range(-10,10);
        var z = Random.Range(-10,10);
        var start = transform.position;
        return new Vector3(x,0,z);
    }
}
