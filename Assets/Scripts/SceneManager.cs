using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance;
    
    public AudioClip tickSound;
    public AudioClip pancakeSound;

    public int interval = 10;

    private float t0;
    private int count;

    void Awake()
    {
        Instance = this;
    }

    void Start () {
        t0 = Time.time;
        count = interval;
    }
    
    void Update () {
        float t = Time.time;
        if (t0 + 1.0 <= t) {
            t0 = t;
            countDown();
        }
    }

    private void countDown()
    {
        count--;
        if (count == 0) {
            count = interval;
            somethingHappens();
            audio.PlayOneShot(pancakeSound);
        } else {
            audio.PlayOneShot(tickSound);
        }
    }

    private void somethingHappens()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("floor")) {
            FloorBehaviour floor = obj.GetComponent<FloorBehaviour>();
            if (floor) {
                floor.SendMessage("ChangeBehaviour");
            }
        }
    }
}
