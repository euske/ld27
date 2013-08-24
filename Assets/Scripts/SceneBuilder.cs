using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneBuilder : MonoBehaviour
{
    public const float range = 5.0f;
    public const float floorspeed = -30.0f;
    public const float floorlimit = 50.0f;
    public const float interval = 2.0f;

    public Transform[] blockPrefabs;
    public Transform[] foodPrefabs;
    public Transform[] powerupPrefabs;

    public int[][] distribution = {
        // mode 0
        new int[] { -1, },

        // mode 1
        new int[] { -1, -1, -1, -1,
                    0, 1, 2, 3, 4, 
                    10,    
                    20, 21, 22 },
        // mode 2
        new int[] { -1, -1, -1, -1, -1, -1,
                    1, 2, 4,       
                    10, 10, 11, 
                    20, 21, 22, },
    };
    
    private int mode;
    private float mode_end;
    private float t0;
    private List<Transform> objects;
    
    void Start () {
        t0 = Time.time;
        objects = new List<Transform>();
        changeMode();
    }

    private void changeMode()
    {
        mode = Random.Range(0, distribution.Length);
        mode_end = Time.time + Random.Range(1f, 3f);
        print("mode="+mode);
    }

    private Transform pickOne(Transform[] prefabs)
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    private Transform getPrefab(int objtype)
    {
        if (objtype < 0) {
            return null;
        } else if (objtype < 10) {
            return blockPrefabs[objtype];
        } else if (objtype < 20) {
            return foodPrefabs[objtype-10];
        } else if (objtype < 30) {
            return powerupPrefabs[objtype-20];
        }
        return null;
    }

    void Update () {
        float t = Time.time;
        if (mode_end < t) {
            changeMode();
        }

        if (t0 + interval/floorspeed <= t) {
            t0 = t;
            int[] dist1 = distribution[mode];
            int objtype = dist1[Random.Range(0, dist1.Length)];
            Transform prefab = getPrefab(objtype);
            if (prefab != null) {
                Vector3 pos = new Vector3(Random.Range(-range, +range), 0, +floorlimit);
                Transform obj = Instantiate(prefab, pos, transform.rotation) as Transform;
                obj.parent = transform;
                objects.Add(obj);
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
