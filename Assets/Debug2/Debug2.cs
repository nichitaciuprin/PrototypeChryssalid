using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public static class Debug2
{
    private static List<DebugLine> lines = new List<DebugLine>();
    private static Material material;
    private static int clearedFrame = 0;
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        MaybeClearLines();
        lines.Add(new DebugLine(start, end, color));
    }
    public static void DrawLine(DebugLine line)
    {
        MaybeClearLines();
        lines.Add(line);
    }
    /*private?*/static Debug2()
    {
        material = Resources.Load<Material>("GL");
        Camera.onPostRender += Render;
    }
    private static void Render(Camera camera)
    {
        if (lines.Count == 0) return;

        MaybeClearLines();

        GL.Begin(GL.LINES);
        material.SetPass(0);
        foreach (var line in lines)
        {
            GL.Color(line.color);
            GL.Vertex(line.start);
            GL.Vertex(line.end);
        }
        GL.End();
    }
    private static void MaybeClearLines()
    {
        if (clearedFrame != Time.frameCount)
        {
            clearedFrame = Time.frameCount;
            lines.Clear();
        }
    }
}
public struct DebugLine
{
    public Vector3 start;
    public Vector3 end;
    public Color color;

    public DebugLine(Vector3 start, Vector3 end, Color color)
    {
        this.start = start;
        this.end = end;
        this.color = color;
    }
}
