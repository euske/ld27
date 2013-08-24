using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneBuilder : MonoBehaviour {

    public Transform[] blockPrefabs;
    public Transform[] foodPrefabs;
    public Transform[] powerupPrefabs;

    public float range = 5.0f;
    public float floorspeed = -20.0f;
    public float floorlimit = 50.0f;

    public float interval = 0.2f;

    private float t0;
    private List<Transform> objects;
    
    void Start () {
        t0 = Time.time;
        objects = new List<Transform>();
    }

    private Transform pickOne(Transform[] prefabs)
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    private Transform makeBlock()
    {
        Vector3 pos = new Vector3(Random.Range(-range, +range), 0, +floorlimit);
        return (Instantiate(pickOne(blockPrefabs), pos, transform.rotation) as Transform);
    }
    
    private Transform makeFood()
    {
        Vector3 pos = new Vector3(Random.Range(-range, +range), 0, +floorlimit);
        return (Instantiate(pickOne(foodPrefabs), pos, transform.rotation) as Transform);
    }
    
    private Transform makePowerup()
    {
        Vector3 pos = new Vector3(Random.Range(-range, +range), 0, +floorlimit);
        return (Instantiate(pickOne(powerupPrefabs), pos, transform.rotation) as Transform);
    }
    
    void Update () {
        float t = Time.time;
        if (t0 + interval <= t) {
            t0 = t;
            if (Random.Range(0, 2) == 0) {
                objects.Add(makeBlock());
            }
            if (Random.Range(0, 4) == 0) {
                objects.Add(makeFood());
            }
            if (Random.Range(0, 4) == 0) {
                objects.Add(makePowerup());
            }
        }

        Vector3 v = new Vector3(0, 0, Time.deltaTime*floorspeed);
        List<Transform> destroyed = new List<Transform>();
        foreach (Transform obj in objects) {
            obj.Translate(v);
            if (obj.position.z < -floorlimit) {
                destroyed.Add(obj);
            }
        }
        foreach (Transform obj in destroyed) {
            objects.Remove(obj);
            Destroy(obj.gameObject);
        }
    }
}
