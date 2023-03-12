using System.Collections.Generic;
using UnityEngine;

public static class GLLines
{
    private static List<Line> lines = new List<Line>();
    private static List<Line> linesScreen = new List<Line>();
    private static Material material;
    private static int clearedFrame = 0;

    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        MaybeClearLines();
        lines.Add(new Line(start, end, color));
    }
    public static void DrawLineScreen(Vector2 start, Vector2 end, Color color)
    {
        MaybeClearLines();
        linesScreen.Add(new Line(start, end, color));
    }
    static GLLines()
    {
        if (material != null) return;
        var shader = Resources.Load<Shader>("Empty");
        material = new Material(shader);
        Camera.onPostRender += Render;
    }
    private static void Render(Camera camera)
    {
        if (material == null) return;
        MaybeClearLines();
        material.SetPass(0);

        GL.PushMatrix();
        DrawLines(lines);
        GL.PopMatrix();

        GL.PushMatrix();
        GL.LoadPixelMatrix();
        DrawLines(linesScreen);
        GL.PopMatrix();
    }
    private static void DrawLines(List<Line> list)
    {
        if (list.Count == 0) return;
        GL.Begin(GL.LINES);
        foreach (var line in list)
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
            linesScreen.Clear();
        }
    }
    private struct Line
    {
        public Vector3 start;
        public Vector3 end;
        public Color color;

        public Line(Vector3 start, Vector3 end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }
    }
}