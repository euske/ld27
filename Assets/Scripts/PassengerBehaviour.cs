using UnityEngine;
using System.Collections;

public class PassengerBehaviour : MonoBehaviour {

    public Transform target;
    public float speed = 1.0f;

    void Start () {
        target = GameObject.FindWithTag("train").GetComponent<Transform>();
    }
	
    void Update () {
        if (target) {
            float r = speed * Time.deltaTime;
            Vector2 p0 = new Vector2(transform.position.x, transform.position.z);
            Vector2 p1 = new Vector2(target.position.x, target.position.z);
            Move((p1-p0)*r);
        }
    }

    private void Move(Vector2 v)
    {
        transform.Translate(new Vector3(v.x, 0, v.y));
    }
}
