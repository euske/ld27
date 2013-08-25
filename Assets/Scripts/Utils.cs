using UnityEngine;
using System.Collections;

public class GUIStyle2
{
    public GUIStyle text;
    public GUIStyle shadow;

    public GUIStyle2(int fontsize, Color color)
    {
        text = new GUIStyle();
        text.fontSize = fontsize;
        text.alignment = TextAnchor.MiddleCenter;
        text.normal.textColor = color;

        shadow = new GUIStyle();
        shadow.fontSize = fontsize;
        shadow.alignment = TextAnchor.MiddleCenter;
        shadow.normal.textColor = Color.black;
    }

    public void Render(Rect rect, string s)
    {
        GUI.Label(rect, s, shadow);
        rect.x -= text.fontSize*0.05f;
        rect.y -= text.fontSize*0.05f;
        GUI.Label(rect, s, text);
    }
}
