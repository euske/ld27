using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance;
    
    public AudioClip tickSound;
    public float interval = 1.0f;

    private float t0 = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start () {
        t0 = Time.time;
    }
    
    void Update () {
        float t = Time.time;
        if (t0 + interval <= t) {
            t0 = t;
            countDown();
        }
    }

    private void countDown()
    {
        audio.PlayOneShot(tickSound);
    }
}
