using UnityEngine;
using System.Collections;

public class PlatformBuilder : MonoBehaviour {

    public Transform passengerPrefab;
    public int numPassengers = 10;

    private Transform makePassenger(float x, float y, float z)
    {
        Vector3 pos = (Vector3.left*transform.localScale.x*x+
                       Vector3.up*transform.localScale.y*y*0.5f+
                       Vector3.forward*transform.localScale.z*z);
        Transform passenger = (Instantiate(passengerPrefab, 
                                           transform.position+pos, transform.rotation)
                               as Transform);
        passenger.parent = transform;
        return passenger;
    }

    void Awake() {
        for (int i = 0; i < numPassengers; i++) {
            makePassenger(Random.Range(-1.0f, +1.0f), +1.0f, Random.Range(-1.0f, +1.0f));
        }
    }

}
