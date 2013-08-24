using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour {

    public float speed = 1.0f;

    private Vector3 direction;
    private Vector3 rotation;

    void Start () {
        direction = new Vector3(Random.Range(-1.0f, +1.0f), 
                                0,
                                Random.Range(-1.0f, +1.0f));
    }
	
    void Update () {
        float r = speed * Time.deltaTime;
        transform.Translate(direction * r);
        if (!rigidbody.freezeRotation) {
            rigidbody.AddTorque(rotation);
        }
    }

    void StartPancake()
    {
        float v = 1.0f;
        rigidbody.freezeRotation = false;
        rotation = new Vector3(Random.Range(-v, +v),
                               Random.Range(-v, +v),
                               Random.Range(-v, +v));
    }

    void StopPancake()
    {
        rigidbody.freezeRotation = true;
    }
}
