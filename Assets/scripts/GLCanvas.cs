using UnityEngine;
using System.Collections.Generic;

public class GLCanvas : MonoBehaviour {

    public static List<GameObject> subscribers = new List<GameObject>();
    public Material mat;

    void OnPostRender()
    {
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        Camera cam = GetComponent<Camera>();
        GL.LoadProjectionMatrix(cam.projectionMatrix);
        GL.modelview = cam.worldToCameraMatrix;

        subscribers.RemoveAll(x => x == null);

        foreach (GameObject o in subscribers)
            o.SendMessage("onCanvas");

        //GL.End();
        GL.PopMatrix();
    }


    public static void rect(float x, float y, float width, float height, Color color, float layer)
    {
        GL.Begin(GL.QUADS);
        GL.Color(color);
        GL.Vertex3(x, y, layer);
        GL.Vertex3(x + width, y, layer);
        GL.Vertex3(x + width, y + height, layer);
        GL.Vertex3(x, y + height, layer);
        GL.End();
    }
}
