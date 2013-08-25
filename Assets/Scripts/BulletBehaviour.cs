using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public float duration = 3.0f;
    public float speed = 10.0f;

    void Start () {
        rigidbody.AddForce(Vector3.forward * speed, ForceMode.Impulse);
	Destroy(gameObject, duration);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "block" ||
            col.gameObject.tag == "enemy" ||
            col.gameObject.tag == "food" ||
            col.gameObject.tag == "powerup") {
            GameManager.Instance.SendMessage("SomethingDestroyed");
            Destroy(col.gameObject);
        }
        Destroy(gameObject);
    }
}
