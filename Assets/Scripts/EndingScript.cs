using UnityEngine;
using System.Collections;

public class EndingScript : MonoBehaviour {
    
    private GUIStyle2 style;

    void Awake()
    {
        style = new GUIStyle2(120, Color.white);
    }

    void Update () 
    {
        if (Input.GetButtonDown("Jump") ||
            Input.GetButtonDown("Fire1")) {
            Application.LoadLevel("scene0");
        }
    }

    void OnGUI()
    {
        Rect r = new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200);
        int score = (GameManager.Instance != null)? GameManager.Instance.score : -1;
        style.Render(r, "Score: "+score);
    }
}
