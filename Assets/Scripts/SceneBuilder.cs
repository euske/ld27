using UnityEngine;
using System.Collections;

public class SceneBuilder : MonoBehaviour {

    public Transform floorPrefab;
    public float range = 3.0f;
    public int numFloors = 10;
    
    void Start () {
        for (int i = 0; i < numFloors; i++) {
            createFloor();
        }
    }
    
    void Update () {
    }
    
    private void createFloor()
    {
        Vector3 scale = floorPrefab.transform.localScale;
        Vector3 pos = new Vector3(Random.Range(-range, +range) * scale.x,
                                  Random.Range(-range, +range) * scale.y,
                                  Random.Range(-range, +range) * scale.z);
        Instantiate(floorPrefab, pos, transform.rotation);
    }
}
