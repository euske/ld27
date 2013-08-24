using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

    public float speed = 10.0f;

    void Start () {
        rigidbody.AddForce(transform.up * speed, ForceMode.Impulse);
    }
	
    void Update () {
        
    }
}
