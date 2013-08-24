using UnityEngine;
using System.Collections;

public class SceneBuilder : MonoBehaviour {

    public Transform BlockPrefab;
    public float range = 3.0f;
    public float interval = 0.1f;

    private float t0 = 0f;

    // Use this for initialization
    void Start () {
        t0 = Time.time;
    }
    
    // Update is called once per frame
    void Update () {
        float t = Time.time;
        if (t0 + interval <= t) {
            t0 = t;
            createBlock();
        }
    }
    
    private void createBlock()
    {
        Instantiate(BlockPrefab, randpos(), randrot());
    }

    private Vector3 randpos()
    {
        return new Vector3(Random.Range(-range, +range),
                           Random.Range(-range, +range),                           
                           +20);
    }

    private Quaternion randrot()
    {
        return Quaternion.Euler(Random.Range(0f, 360f),
                                Random.Range(0f, 360f),
                                Random.Range(0f, 360f));
    }
}
