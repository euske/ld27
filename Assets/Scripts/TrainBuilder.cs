using UnityEngine;
using System.Collections;

public class TrainBuilder : MonoBehaviour {

    public Transform wallPrefab;
    public Transform passengerPrefab;

    public float size = 5.0f;
    public int numPassengers = 10;

    private Transform makeWall(float x, float y, float z)
    {
        Vector3 pos = (Vector3.left*transform.localScale.x*x+
                       Vector3.up*transform.localScale.y*y+
                       Vector3.forward*transform.localScale.z*z);
        Transform wall = (Instantiate(wallPrefab, transform.position+pos, transform.rotation)
                          as Transform);
        wall.parent = transform;
        return wall;
    }

    private Transform makePassenger(float x, float y, float z)
    {
        Vector3 pos = (Vector3.left*transform.localScale.x*x+
                       Vector3.up*transform.localScale.y*y+
                       Vector3.forward*transform.localScale.z*z);
        Transform passenger = (Instantiate(passengerPrefab, 
                                           transform.position+pos, transform.rotation)
                               as Transform);
        passenger.parent = transform;
        return passenger;
    }

    void Awake() {
        float s = size;
        makeWall(-s, 0, 0).localScale = new Vector3(0.2f, 5.0f, s*2);
        //makeWall( 0, 0,-s).localScale = new Vector3(s*2, 5.0f, 0.2f);
        makeWall(+s, 0, 0).localScale = new Vector3(0.2f, 5.0f, s*2);
        makeWall( 0, 0,+s).localScale = new Vector3(s*2, 5.0f, 0.2f);
        makeWall( 0, 0, 0).localScale = new Vector3(s*2, 0.2f, s*2);

        for (int i = 0; i < numPassengers; i++) {
            makePassenger(Random.Range(-s, +s), 0, Random.Range(-s, +s));
        }
    }
}
