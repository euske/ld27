using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public float duration = 3.0f;
    public float speed = 10.0f;

    void Start () {
	rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
	Destroy(gameObject, duration);
    }
    
    void OnCollisionEnter(Collision col)
    {
	Destroy(gameObject);
    }
}
