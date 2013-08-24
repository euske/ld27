using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public int score;
    public int health;
    
    private GUIStyle style;

    void Awake()
    {
        Instance = this;
        style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
    }

    void Start() 
    {
        score = 0;
        health = 0;
    }
	
    void Update () {
        
    }

    void ScoreIt() 
    {
        score++;
    }

    void SetHealth(int v)
    {
        health = v;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(32, 32, 100, 100), ("Score: "+score), style);
        GUI.Label(new Rect(32, 64, 100, 100), ("Health: "+health), style);
    }
}
