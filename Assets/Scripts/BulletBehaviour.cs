using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public const float duration = 3.0f;
    public const float speed = 10.0f;
    public const float rot = 10.0f;

    void Start () {
        rigidbody.AddForce(Vector3.forward * speed, ForceMode.Impulse);
        rigidbody.AddTorque(randvec(rot), ForceMode.Impulse);
	Destroy(gameObject, duration);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "block" ||
            col.gameObject.tag == "enemy" ||
            col.gameObject.tag == "food" ||
            col.gameObject.tag == "powerup") {
            SceneBuilder.Instance.SendMessage("SomethingDestroyed");
            Destroy(col.gameObject);
        }
        Destroy(gameObject);
    }

    private Vector3 randvec(float v)
    {
        return new Vector3(Random.Range(-v, +v),
                           Random.Range(-v, +v),                           
                           Random.Range(-v, +v));
    }
}
