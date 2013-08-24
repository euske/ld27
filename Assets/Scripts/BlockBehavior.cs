using UnityEngine;
using System.Collections;

public class BlockBehavior : MonoBehaviour {

    public float speed = 10.0f;
    public float rotvariation = 1.0f;
    public float dirvariation = 3.0f;
    public float limit = 100.0f;

    void Start () {
        rigidbody.AddForce(Vector3.forward*(-speed)+randvec(dirvariation), ForceMode.Impulse);
        rigidbody.AddTorque(randvec(rotvariation), ForceMode.Impulse);
    }
	
    void Update () {
        if (transform.position.x < -limit || +limit < transform.position.x ||
            transform.position.y < -limit || +limit < transform.position.y ||
            transform.position.z < -limit || +limit < transform.position.z) {
            Destroy(gameObject);
        }
    }

    private Vector3 randvec(float v)
    {
        return new Vector3(Random.Range(-v, +v),
                           Random.Range(-v, +v),
                           Random.Range(-v, +v));
    }

}
