using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public AudioClip destroysound;

    private int score;
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
    }
	
    void Update () {
        
    }

    void GameOver()
    {
        Application.LoadLevel("scene1");
    }

    void UpdateScore(FoodType food)
    {
        score += (food == FoodType.Golden)? 3 : 1;
    }

    void SomethingDestroyed()
    {
        audio.PlayOneShot(destroysound);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(32, 32, 100, 100), ("Score: "+score), style);
    }
}
