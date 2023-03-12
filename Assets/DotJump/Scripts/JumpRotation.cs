using UnityEngine;

public class JumpRotation : MonoBehaviour
{
    public JumpProcess jumpProcess;
    public Tilt tilt;

    private void Update()
    {
        //if (jumpProcess == null) return;
        //var dir = jumpProcess.Direction();
        //dir.Normalize();
        //var rotAxis = new Vector3(dir.y,0,-dir.x);
        //var t = jumpProcess.Progress();
        //var angle = 360*t;
        //var rot1 = Quaternion.AngleAxis(angle,rotAxis);
        //var rot2 = transform.parent.rotation;
        //transform.rotation = rot1 * rot2;

        if (jumpProcess == null) return;
        var dir = jumpProcess.Direction();
        dir.Normalize();
        var rotAxis = new Vector3(dir.y,0,-dir.x);
        var t = jumpProcess.Progress();
        var angle = 360*t;
        tilt.angle = angle;
        tilt.globalDirection = dir;
    }
}
