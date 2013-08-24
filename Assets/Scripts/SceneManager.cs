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
        guiText.text = count.ToString();
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
        guiText.text = count.ToString();
    }

    private void somethingHappens()
    {
        //int action = 6;
        int action = Random.Range(0, 7);

        switch (action) {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("floor")) {
                FloorBehaviour floor = obj.GetComponent<FloorBehaviour>();
                if (floor) {
                    floor.SendMessage("ChangeBehaviour", action);
                }
            }
            break;
        case 7:
            break;
        }
    }
}
