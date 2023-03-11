using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameIndicator : MonoBehaviour
{
    private string msg = "<color=red>" + "------------------------------------------------------------------" + "</color>";
    private float timer = 0;

    private void Update()
    {
        if (timer > 0) return;
        if (Time.deltaTime <= 0.08f) return;
        timer = 3f;
    }
    private void OnGUI()
    {
        if (timer <= 0) return;
        timer -= Time.deltaTime;
        GUI.Label(new Rect(10, 10, 300, 300), msg);
    }
}


//redfords -- orange
//caleb -- purple