using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Foot : MonoBehaviour
{
    public BoxCollider collider_1;
    public BoxCollider collider_2;
    public Transform hip;
    public bool isRight;
    [Range(0,2)] public float footMoveTime;
    [HideInInspector] public bool inProcess = false;
    public TwoBoneIK twoBoneIK;

    private Renderer renderer_1;
    private Renderer renderer_2;

    private void Start()
    {
        renderer_1 = collider_1.GetComponent<Renderer>();
        renderer_2 = collider_2.GetComponent<Renderer>();
    }
    private void Update()
    {
        if (Helper.CheckBox(collider_1)) renderer_1.material.color = Color.yellow; else renderer_1.material.color = Color.green;
        if (Helper.CheckBox(collider_2)) renderer_2.material.color = Color.yellow; else renderer_2.material.color = Color.green;
    }

    public void Move(Vector3 targetPosition)
    {
        StartCoroutine(Move_coroutine(targetPosition));
    }
    public IEnumerator Move_coroutine(Vector3 targetPosition)
    {
        if (inProcess) yield break;
        inProcess = true;

        var start = transform.position;
        var end = targetPosition;

        float t = 0;
        while (true)
        {
            var speed = footMoveTime;
            if (speed == 0)
                t = 1;
            else
                t += Time.deltaTime / speed;

            Krisalid.InterpolateFoot(this,start,end,t);

            if (t >= 1) break;
            yield return null;
        }

        inProcess = false;
    }
}