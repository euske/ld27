using UnityEngine;
using System.Collections;

public class EndingScript : MonoBehaviour {
    
    private GUIStyle style;

    void Awake()
    {
        style = new GUIStyle();
        style.fontSize = 120;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
    }

    void Update () 
    {
        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonDown("Vertical") ||
            Input.GetButtonDown("Jump") ||
            Input.GetButtonDown("Fire1")) {
            Application.LoadLevel("scene0");
        }
    }

    void OnGUI()
    {
        Rect r = new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200);
        int score = GameManager.Instance.score;
        GUI.Label(r, "Score: "+score, style);
    }
}
