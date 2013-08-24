using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public int score;

    void Awake()
    {
        Instance = this;
    }

    void Start() 
    {
        setScore(0);
    }
	
    void Update () {
        
    }

    void ScoreIt() 
    {
        setScore(score+1);
    }

    private void setScore(int v)
    {
        score = v;
        guiText.text = ("Score: " + score);
    }
}
